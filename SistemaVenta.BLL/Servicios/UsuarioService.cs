using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SistemaVenta.BLL.Servicios.Contratos;
using SistemaVenta.DAL.Repositorios.Contratos;
using SistemaVenta.DTO;
using SistemaVenta.Model;
using System.Security.Cryptography;
using System.Text;


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
                // Crear un objeto de tipo SHA1 para calcular el hash de la contraseña
                using (SHA1 sha1 = SHA1.Create())
                {
                    // Convertir la contraseña a un array de bytes
                    byte[] claveBytes = Encoding.UTF8.GetBytes(modelo.Clave);

                    // Calcular el hash de la contraseña
                    byte[] hashClave = sha1.ComputeHash(claveBytes);

                    // Convertir el hash a una cadena hexadecimal y truncarlo a 40 caracteres
                    string claveHash = BitConverter.ToString(hashClave).Replace("-", "").ToLower().Substring(0, 40);

                    // Asignar el hash truncado al modelo
                    modelo.Clave = claveHash;
                }
                // Verificar si ya existe un usuario con el mismo correo
                var usuarioExistente = await _UsuarioRepositorio.Obtener(u => u.Correo == modelo.Correo);
                if (usuarioExistente != null)
                {
                    throw new TaskCanceledException("El usuario ya existe");
                }
                // Crear el usuario en la base de datos
                var usuarioCreado = await _UsuarioRepositorio.Crear(_mapper.Map<Usuario>(modelo));
                if (usuarioCreado.IdUsuario == 0)
                {
                    throw new TaskCanceledException("No se pudo crear el usuario");
                }

                // Consultar el usuario recién creado para obtener su información completa
                var query = await _UsuarioRepositorio.Consultar(u => u.IdUsuario == usuarioCreado.IdUsuario);
                Usuario usuarioCompleto = query.Include(rol => rol.IdRolNavigation).FirstOrDefault();

                // Mapear el usuario completo a un DTO y devolverlo
                return _mapper.Map<UsuarioDTO>(usuarioCompleto);
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> Editar(UsuarioDTO modelo,int id)
        {
            try
            {
                // Obtener el usuario existente de la base de datos
                var usuarioEncontrado = await _UsuarioRepositorio.Obtener(u => u.IdUsuario == id);
                if (usuarioEncontrado == null)
                {
                    throw new TaskCanceledException("Usuario no encontrado");
                }

                // Actualizar solo las propiedades necesarias
                if (!string.IsNullOrEmpty(modelo.NombreCompleto))
                {
                    usuarioEncontrado.NombreCompleto = modelo.NombreCompleto;
                }

                if (!string.IsNullOrEmpty(modelo.Correo))
                {
                    usuarioEncontrado.Correo = modelo.Correo;
                }

                if (modelo.IdRol.HasValue)
                {
                    usuarioEncontrado.IdRol = modelo.IdRol.Value;
                }

                if (!string.IsNullOrEmpty(modelo.Clave))
                {
                    // Encriptar la nueva contraseña si se proporciona
                    using (SHA1 sha1 = SHA1.Create())
                    {
                        // Convertir la contraseña a un array de bytes
                        byte[] claveBytes = Encoding.UTF8.GetBytes(modelo.Clave);

                        // Calcular el hash de la contraseña
                        byte[] hashClave = sha1.ComputeHash(claveBytes);

                        // Convertir el hash a una cadena hexadecimal y truncarlo a 40 caracteres
                        string claveHash = BitConverter.ToString(hashClave).Replace("-", "").ToLower().Substring(0, 40);

                        // Asignar el hash truncado al usuario encontrado
                        usuarioEncontrado.Clave = claveHash;
                    }
                }

                if (modelo.EsActivo.HasValue)
                {
                    usuarioEncontrado.EsActivo = Convert.ToBoolean(modelo.EsActivo.Value);
                }

                // Actualizar el usuario en la base de datos
                bool respuesta = await _UsuarioRepositorio.Editar(usuarioEncontrado);
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
                var usuarioEncontrado = await _UsuarioRepositorio.Obtener(u => u.IdUsuario == id);
                if (usuarioEncontrado == null)
                {
                    throw new TaskCanceledException("Usuario no encontrado");
                }

                bool respuesta = await _UsuarioRepositorio.Eliminar(usuarioEncontrado);
                if (!respuesta)
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
                // Crear un objeto de tipo SHA1 para calcular el hash de la contraseña
                using (SHA1 sha1 = SHA1.Create())
                {
                    // Convertir la contraseña a un array de bytes
                    byte[] claveBytes = Encoding.UTF8.GetBytes(clave);

                    // Calcular el hash de la contraseña
                    byte[] hashClave = sha1.ComputeHash(claveBytes);

                    // Convertir el hash a una cadena hexadecimal y truncarlo a 40 caracteres
                    string claveHash = BitConverter.ToString(hashClave).Replace("-", "").ToLower().Substring(0, 40);

                    // Consultar el usuario por correo y hash de contraseña
                    var queryUsuario = await _UsuarioRepositorio.Consultar(u => u.Correo == correo && u.Clave == claveHash);

                    if (queryUsuario.FirstOrDefault() == null)
                    {
                        throw new TaskCanceledException("El usuario no existe o la contraseña es incorrecta");
                    }

                    // Obtener el primer usuario que coincida
                    Usuario devolverUsuario = queryUsuario.Include(rol => rol.IdRolNavigation).FirstOrDefault();

                    // Mapear el usuario a un DTO y devolverlo
                    return _mapper.Map<SesionDTO>(devolverUsuario);
                }
            }
            catch
            {
                throw;
            }
        }
    }
}
