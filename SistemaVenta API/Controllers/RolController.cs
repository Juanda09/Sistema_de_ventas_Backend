using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using SistemaVenta.BLL.Servicios.Contratos;
using SistemaVenta.DTO;
using SistemaVenta.API.Utilidad;

namespace SistemaVenta.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolController : ControllerBase
    {
        private readonly IRolService _RolService;

        public RolController(IRolService rolService)
        {
            _RolService = rolService;
        }

        [HttpGet]
        [Route("Lista")]
        public async Task<IActionResult> Lista()
        {
            try
            {
                var roles = await _RolService.Lista();
                return StatusCode(StatusCodes.Status200OK, new Response<List<RolDTO>>
                {
                    status = true,
                    Value = roles,
                    msg = "Roles obtenidos"
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
