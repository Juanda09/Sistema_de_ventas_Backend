using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using SistemaVenta.BLL.Servicios.Contratos;
using SistemaVenta.DTO;
using SistemaVenta.API.Utilidad;

namespace SistemaVenta.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MenuController : ControllerBase
    {
        private readonly IMenuService _MenuService;

        public MenuController(IMenuService menuService)
        {
            _MenuService = menuService;
        }

        [HttpGet]
        [Route("Menu")]
        public async Task<IActionResult> Menu(int idUsuario)
        {
            try
            {
                var Menu = await _MenuService.Lista(idUsuario);
                return StatusCode(StatusCodes.Status200OK, new Response<List<MenuDTO>>
                {
                    status = true,
                    Value = Menu,
                    msg = "Menu obtenido correctamente"
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
