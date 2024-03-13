using SistemaVenta.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaVenta.BLL.Servicios.Contratos
{
    public interface IVentaService
    {
        Task<VentaDTO> Registrar(VentaDTO venta);
        Task<List<VentaDTO>> Historial(string buscarPor,string numeroVenta,string fechainicio,string fechafin);
        Task<List<ReporteDTO>> Reporte(string fechainicio , string fechafin);
    }
}
