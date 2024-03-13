using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using SistemaVenta.BLL.Servicios.Contratos;
using SistemaVenta.DTO;
using SistemaVenta.API.Utilidad;
using SistemaVenta.BLL.Servicios;

namespace SistemaVenta.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VentaController : ControllerBase
    {
        private readonly IVentaService _ventaService;

        public VentaController(IVentaService ventaService)
        {
            _ventaService = ventaService;
        }
        [HttpPost]
        [Route("Registrar")]
        public async Task<IActionResult> Registrar([FromBody] VentaDTO venta)
        {
            try
            {
                var nuevaventa = await _ventaService.Registrar(venta);
                return StatusCode(StatusCodes.Status201Created, new Response<VentaDTO>
                {
                    status = true,
                    Value = nuevaventa,
                    msg = "Producto creado exitosamente"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response<object>
                {
                    status = false,
                    msg = $"Error interno del servidor: {ex.Message}"
                });
            }
        }
        [HttpGet]
        [Route("Historial")]
        public async Task<IActionResult> Historial(string buscarpor,string? numeroVenta,string? fechainicio, string? fechafin)
        {
            try
            {
                numeroVenta = numeroVenta is null ? "" : numeroVenta;
                fechainicio = fechainicio is null ? "" : fechainicio;
                fechafin = fechafin is null ? "" : fechafin;
                var listaventa = await _ventaService.Historial(buscarpor,numeroVenta,fechainicio,fechafin);
                return StatusCode(StatusCodes.Status200OK, new Response<List<VentaDTO>>
                {
                    status = true,
                    Value = listaventa,
                    msg = "Historial obtenido correctamente"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response<object>
                {
                    status = false,
                    msg = $"Error interno del servidor: {ex.Message}"
                });
            }
        }
        [HttpGet]
        [Route("Reporte")]
        public async Task<IActionResult> Reporte(string fechainicio, string fechafin)
        {
            try
            {

                var Reporte = await _ventaService.Reporte(fechainicio, fechafin);
                return StatusCode(StatusCodes.Status200OK, new Response<List<ReporteDTO>>
                {
                    status = true,
                    Value = Reporte,
                    msg = "Reporte obtenido correctamente"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response<object>
                {
                    status = false,
                    msg = $"Error interno del servidor: {ex.Message}"
                });
            }
        }
    }
}