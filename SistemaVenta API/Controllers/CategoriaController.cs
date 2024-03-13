using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using SistemaVenta.BLL.Servicios.Contratos;
using SistemaVenta.DTO;
using SistemaVenta.API.Utilidad;

namespace SistemaVenta.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriaController : ControllerBase
    {
        private readonly ICategoriaService _CategoriaService;

        public CategoriaController(ICategoriaService categoriaService)
        {
            _CategoriaService = categoriaService;
        }

        [HttpGet]
        [Route("Lista")]
        public async Task<IActionResult> Lista()
        {
            try
            {
                var usuarios = await _CategoriaService.Lista();
                return StatusCode(StatusCodes.Status200OK, new Response<List<CategoriaDTO>>
                {
                    status = true,
                    Value = usuarios,
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

        [HttpPost]
        [Route("Guardar")]
        public async Task<IActionResult> Guardar([FromBody] CategoriaCreacionDTO categoria)
        {
            try
            {
                var nuevacategoria = await _CategoriaService.Crear(categoria);
                return StatusCode(StatusCodes.Status201Created, new Response<CategoriaDTO>
                {
                    status = true,
                    Value = nuevacategoria,
                    msg = "Categoria creada exitosamente"
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
        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> Eliminar(int id)
        {
            var rsp = new Response<bool>();
            try
            {
                var exito = await _CategoriaService.Eliminar(id);
                rsp.status = true;
                rsp.Value = exito;
                rsp.msg = "Categoria eliminada correctamente";
                return Ok(rsp);
            }
            catch (Exception ex)
            {
                rsp.status = false;
                rsp.msg = $"Error interno del servidor: {ex.Message}";
                return StatusCode(StatusCodes.Status500InternalServerError, rsp);
            }
        }
    }
}
