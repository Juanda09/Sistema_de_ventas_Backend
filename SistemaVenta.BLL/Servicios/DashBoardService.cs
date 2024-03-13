using AutoMapper;
using SistemaVenta.BLL.Servicios.Contratos;
using SistemaVenta.DAL.Repositorios.Contratos;
using SistemaVenta.DTO;
using SistemaVenta.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace SistemaVenta.BLL.Servicios
{
    public class DashBoardService : IDashBoardService
    {
        private readonly IGenericRepository<Producto> _ProductoRepositorio;
        private readonly IVentaRepository _VentaRepositorio;
        private readonly IMapper _mapper;

        public DashBoardService(IGenericRepository<Producto> productoRepositorio, IVentaRepository ventaRepositorio, IMapper mapper)
        {
            _ProductoRepositorio = productoRepositorio;
            _VentaRepositorio = ventaRepositorio;
            _mapper = mapper;
        }
        private IQueryable<Venta> retornarVentas(IQueryable<Venta> tablaventa, int restarCantidadDias)
        {
            DateTime? ultimafecha = tablaventa.OrderByDescending(v => v.FechaRegistro).Select(V => V.FechaRegistro).First();
            ultimafecha = ultimafecha.Value.AddDays(restarCantidadDias);
            return tablaventa.Where(v => v.FechaRegistro.Value.Date >= ultimafecha.Value.Date);

        }
        private async Task<int> TotalVentasUltimaSemana()
        {
            int total = 0;
            IQueryable<Venta> _ventaQuery = await _VentaRepositorio.Consultar();
            if (_ventaQuery.Count() > 0)
            {
                var tablaventas = retornarVentas(_ventaQuery, -7);
                total = tablaventas.Count();
            }
            return total;
        }
        private async Task<string> TotalingresosUltimasemana()
        {
            decimal resultado = 0;
            IQueryable<Venta> _ventaquery = await _VentaRepositorio.Consultar();
            if (_ventaquery.Count() > 0)
            {
                var tablaventa = retornarVentas(_ventaquery, -7);
                resultado = tablaventa.Select(v => v.Total).Sum(v => v.Value);
            }
            return Convert.ToString(resultado, new CultureInfo("es-CO"));
        }
        private async Task<int> TotalProductos()
        {
            IQueryable<Producto> _productoQuery = await _ProductoRepositorio.Consultar();
            int total = _productoQuery.Count();
            return total;

        }
        private async Task<Dictionary<string, int>> VentasUltimaSemana()
        {
            Dictionary<string, int> resultado = new Dictionary<string, int>();
            IQueryable<Venta> _VentaQuery = await _VentaRepositorio.Consultar();
            if(_VentaQuery.Count() > 0)
            {
                var tablaventa = retornarVentas(_VentaQuery, -7);
                resultado= tablaventa.GroupBy(v=>v.FechaRegistro.Value.Date).OrderBy(g=>g.Key)
                    .Select(dv=> new {fecha = dv.Key.ToString("dd/MM/yyyy"),total = dv.Count()})
                    .ToDictionary(keySelector:r=>r.fecha,elementSelector: r => r.total);
            }
            return resultado;
        }

        public async Task<DashBoardDTO> Resumen()
        {
            DashBoardDTO viewDashBoard = new DashBoardDTO();
            try
            {
                viewDashBoard.TotalVentas = await TotalVentasUltimaSemana();
                viewDashBoard.TotalIngresos = await TotalingresosUltimasemana();
                viewDashBoard.TotalProductos = await TotalProductos();

                List<VentasSemanaDTO> ListaVenta = new List<VentasSemanaDTO>();
                foreach(KeyValuePair<string,int> item in await VentasUltimaSemana())
                {
                    ListaVenta.Add(new VentasSemanaDTO()
                    {
                        Fecha = item.Key,
                        Total = item.Value,
                    });
                }

                viewDashBoard.VentasUltimaSemana = ListaVenta;
                return viewDashBoard;
            }
            catch
            {
                throw;
            }
        }
    } 
}
