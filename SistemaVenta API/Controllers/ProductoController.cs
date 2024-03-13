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
    public class ProductoController : ControllerBase
    {
        private readonly IProductoService _ProductoService;

        public ProductoController(IProductoService productoService)
        {
            _ProductoService = productoService;
        }

        [HttpGet]
        [Route("Lista")]
        public async Task<IActionResult> Lista()
        {
            try
            {
                var productos = await _ProductoService.Lista();
                return StatusCode(StatusCodes.Status200OK, new Response<List<ProductoDTO>>
                {
                    status = true,
                    Value = productos,
                    msg = "Productos obtenidos correctamente"
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
        public async Task<IActionResult> Guardar([FromBody] ProductosCreacionDTO producto)
        {
            try
            {
                var nuevoproducto = await _ProductoService.Crear(producto);
                return StatusCode(StatusCodes.Status201Created, new Response<ProductoDTO>
                {
                    status = true,
                    Value = nuevoproducto,
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
        [HttpPatch]
        [Route("{id:int}")]
        public async Task<IActionResult> Editar(int id, [FromBody] ProductosCreacionDTO producto)
        {
            try
            {

                bool resultado = await _ProductoService.Editar(producto, id);
                if (resultado)
                {
                    return StatusCode(StatusCodes.Status200OK, new Response<object>
                    {
                        status = true,
                        Value = resultado,
                        msg = "Usuario actualizado correctamente"
                    });
                }
                else
                {
                    return StatusCode(StatusCodes.Status404NotFound, new Response<object>
                    {
                        status = false,
                        msg = "Usuario no encontrado"
                    });
                }
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
                var exito = await _ProductoService.Eliminar(id);
                rsp.status = true;
                rsp.Value = exito;
                rsp.msg = "Producto eliminado correctamente";
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
