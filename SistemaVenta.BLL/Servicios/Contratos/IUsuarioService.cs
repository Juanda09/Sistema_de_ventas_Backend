using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SistemaVenta.DTO;

namespace SistemaVenta.BLL.Servicios.Contratos
{
    public interface IUsuarioService
    {
        Task<List<UsuarioDTO>> Lista();
        Task<SesionDTO> ValidarSesion(string correo , string clave);
        Task<UsuarioDTO> Crear(UsuarioCreacionDTO modelo);

        Task<bool> Editar(UsuarioEdicionDTO modelo,int id);

        Task<bool> Eliminar(int id);

    }
}
