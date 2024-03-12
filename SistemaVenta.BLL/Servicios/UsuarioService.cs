using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SistemaVenta.BLL.Servicios.Contratos;
using SistemaVenta.DAL.Repositorios.Contratos;
using SistemaVenta.DTO;
using SistemaVenta.Model;


namespace SistemaVenta.BLL.Servicios
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IGenericRepository<Usuario> _UsuarioRepositorio;
        private readonly IMapper _mapper;

        public UsuarioService(IGenericRepository<Usuario> usuarioRepositorio, IMapper mapper)
        {
            _UsuarioRepositorio = usuarioRepositorio;
            _mapper = mapper;
        }

        public async Task<UsuarioDTO> Crear(UsuarioDTO modelo)
        {
            try
            {
                var usarioexistente = await _UsuarioRepositorio.Obtener(u=>u.Correo == modelo.Correo);
                if (usarioexistente != null)
                {
                    throw new TaskCanceledException("El Usuario ya existe");
                }
                var UsuarioCreado = await _UsuarioRepositorio.Crear(_mapper.Map<Usuario>(modelo));
                if(UsuarioCreado.IdUsuario == 0)
                {
                    throw new TaskCanceledException("No se pudo crear el usuario");
                }
                var query = await _UsuarioRepositorio.Consultar(u=>u.IdUsuario == UsuarioCreado.IdUsuario);
                UsuarioCreado = query.Include(rol => rol.IdRolNavigation).FirstOrDefault();
                return _mapper.Map<UsuarioDTO>(UsuarioCreado);
            }
            catch 
            {
                throw;
            }
        }

        public async Task<bool> Editar(UsuarioDTO modelo)
        {
            try
            {
                var UsuarioModelo = _mapper.Map<Usuario>(modelo);
                var UsuarioEncontrado = await _UsuarioRepositorio.Obtener(u => u.IdUsuario == UsuarioModelo.IdUsuario);
                if (UsuarioEncontrado == null)
                {
                    throw new TaskCanceledException("Usuario no encontrado");
                }
                UsuarioEncontrado.NombreCompleto = UsuarioModelo.NombreCompleto;
                UsuarioEncontrado.Correo = UsuarioModelo.Correo;
                UsuarioEncontrado.IdRol = UsuarioModelo.IdRol;
                UsuarioEncontrado.Clave = UsuarioModelo.Clave;
                UsuarioEncontrado.EsActivo = UsuarioModelo.EsActivo;
                bool respuesta = await _UsuarioRepositorio.Editar(UsuarioEncontrado);
                if (!respuesta)
                {
                    throw new TaskCanceledException("No se pudo actualizar el usuario");
                }
                return respuesta;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public async Task<bool> Eliminar(int id)
        {
            try
            {
                var UsuarioEncontrado = await _UsuarioRepositorio.Obtener(u=> u.IdUsuario == id);
                if (UsuarioEncontrado == null)
                {
                    throw new TaskCanceledException("Usuario no encontrado");
                }
                bool respuesta = await _UsuarioRepositorio.Eliminar(UsuarioEncontrado);
                if (respuesta == false)
                {
                    throw new TaskCanceledException("No se pudo eliminar");
                }
                return respuesta;
            }
            catch
            {
                throw;
            }
        }

        public async Task<List<UsuarioDTO>> Lista()
        {
            try
            {
                var queryUsuario = await _UsuarioRepositorio.Consultar();
                var listaUsuarios = queryUsuario.Include(rol => rol.IdRolNavigation).ToList();
                return _mapper.Map<List<UsuarioDTO>>(listaUsuarios);
            }
            catch
            {
                throw;
            }
        }

        public async Task<SesionDTO> ValidarSesion(string correo, string clave)
        {
            try 
            {
                var queryUsuario = await _UsuarioRepositorio.Consultar(u => u.Correo == correo && u.Clave == clave);
                if (queryUsuario.FirstOrDefault() == null)
                {
                    throw new TaskCanceledException("El usuario no existe");
                }
                Usuario devolverUsuario = queryUsuario.Include(rol => rol.IdRolNavigation).FirstOrDefault();
                return _mapper.Map<SesionDTO>(devolverUsuario);
            }
            catch 
            {
                throw;
            }
        }
    }
}
