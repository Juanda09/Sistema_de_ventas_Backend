using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using SistemaVenta.BLL.Servicios.Contratos;
using SistemaVenta.DTO;
using SistemaVenta.API.Utilidad;

namespace SistemaVenta.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioService _UsuarioService;

        public UsuarioController(IUsuarioService usuarioService)
        {
            _UsuarioService = usuarioService;
        }

        [HttpGet]
        [Route("Lista")]
        public async Task<IActionResult> Lista()
        {
            try
            {
                var usuarios = await _UsuarioService.Lista();
                return StatusCode(StatusCodes.Status200OK, new Response<List<UsuarioDTO>>
                {
                    status = true,
                    Value = usuarios,
                    msg = "Usuarios obtenidos correctamente"
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
        [Route("IniciarSesion")]
        public async Task<IActionResult> IniciarSesion([FromBody] LoginDTO login)
        {
            try
            {
                var sesion = await _UsuarioService.ValidarSesion(login.Correo, login.Clave);
                return StatusCode(StatusCodes.Status200OK, new Response<SesionDTO>
                {
                    status = true,
                    Value = sesion,
                    msg = "Inicio de sesión exitoso"
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
        public async Task<IActionResult> Guardar([FromBody] UsuarioDTO usuario)
        {
            try
            {
                var nuevoUsuario = await _UsuarioService.Crear(usuario);
                return StatusCode(StatusCodes.Status201Created, new Response<UsuarioDTO>
                {
                    status = true,
                    Value = nuevoUsuario,
                    msg = "Usuario creado exitosamente"
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
        public async Task<IActionResult> Editar(int id, [FromBody] UsuarioDTO usuario)
        {
            try
            {

                bool resultado = await _UsuarioService.Editar(usuario,id);
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
                var exito = await _UsuarioService.Eliminar(id);
                rsp.status = true;
                rsp.Value = exito;
                rsp.msg = "Usuario eliminado correctamente";
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
