using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SistemaVenta.BLL.Servicios.Contratos;
using SistemaVenta.DAL.Repositorios.Contratos;
using SistemaVenta.DTO;
using SistemaVenta.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaVenta.BLL.Servicios
{
    public class VentaService:IVentaService
    {
        private readonly IGenericRepository<DetalleVenta> _DetalleVentaRepositorio;
        private readonly IVentaRepository _VentaRepositorio;
        private readonly IMapper _mapper;

        public VentaService(IGenericRepository<DetalleVenta> detalleVentaRepositorio, IVentaRepository ventaRepositorio, IMapper mapper)
        {
            _DetalleVentaRepositorio = detalleVentaRepositorio;
            _VentaRepositorio = ventaRepositorio;
            _mapper = mapper;
        }

        public async Task<List<VentaDTO>> Historial(string buscarPor, string numeroVenta, string fechainicio, string fechafin)
        {
            IQueryable<Venta> query = await _VentaRepositorio.Consultar();
            var ListaResultado = new List<Venta>();
            try
            {
                if (buscarPor == "fecha")
                {
                    DateTime fecha_inicio = DateTime.ParseExact(fechainicio, "dd/MM/yyyy", new CultureInfo("es-CO"));
                    DateTime fecha_fin = DateTime.ParseExact(fechafin, "dd/MM/yyyy", new CultureInfo("es-CO"));
                    ListaResultado = await query.Where(
                        v => v.FechaRegistro.Value.Date >= fecha_inicio && v.FechaRegistro.Value.Date <= fecha_fin)
                        .Include(dv => dv.DetalleVenta)
                        .ThenInclude(p => p.IdProductoNavigation).ToListAsync();
                }
                else
                {
                    ListaResultado = await query.Where(v=> v.NumeroDocumento == numeroVenta)
                       .Include(dv => dv.DetalleVenta)
                       .ThenInclude(p => p.IdProductoNavigation).ToListAsync();
                }
                return _mapper.Map<List<VentaDTO>>(ListaResultado);
            }
            catch
            {
                throw;
            }
        }

        public async Task<VentaDTO> Registrar(VentaDTO venta)
        {
            try
            {
                var VentaGenerada = await _VentaRepositorio.Registrar(_mapper.Map<Venta>(venta));
                if (VentaGenerada.IdVenta == 0)           
                {
                    throw new TaskCanceledException("No se pudo registrar venta");
                }
                return _mapper.Map<VentaDTO>(VentaGenerada);
            }
            catch
            {
                throw;
            }
        }

        public async Task<List<ReporteDTO>> Reporte(string fechainicio, string fechafin)
        {
            IQueryable<DetalleVenta> query = await _DetalleVentaRepositorio.Consultar();
            var ListaResultado = new List<DetalleVenta>();
            try
            {
                DateTime fecha_inicio = DateTime.ParseExact(fechainicio, "dd/MM/yyyy", new CultureInfo("es-CO"));
                DateTime fecha_fin = DateTime.ParseExact(fechafin, "dd/MM/yyyy", new CultureInfo("es-CO"));
                ListaResultado = await query
                    .Include(p => p.IdProductoNavigation)
                    .Include(v => v.IdVentaNavigation)
                    .Where(dv =>
                    dv.IdVentaNavigation.FechaRegistro.Value.Date >= fecha_inicio.Date &&
                    dv.IdVentaNavigation.FechaRegistro.Value.Date <= fecha_fin.Date
                    ).ToListAsync();
            }
            catch 
            {
                throw;
            }
            return _mapper.Map<List<ReporteDTO>>(ListaResultado);
        }
    }
}
