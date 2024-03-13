using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using SistemaVenta.BLL.Servicios.Contratos;
using SistemaVenta.DTO;
using SistemaVenta.API.Utilidad;

namespace SistemaVenta.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly IDashBoardService _DashboardService;

        public DashboardController(IDashBoardService dashboardService)
        {
            _DashboardService = dashboardService;
        }

        [HttpGet]
        [Route("Resumen")]
        public async Task<IActionResult> Resumen()
        {
            try
            {
                var Resumen = await _DashboardService.Resumen();
                return StatusCode(StatusCodes.Status200OK, new Response<DashBoardDTO>
                {
                    status = true,
                    Value = Resumen,
                    msg = "Categorias obtenidas correctamente"
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
