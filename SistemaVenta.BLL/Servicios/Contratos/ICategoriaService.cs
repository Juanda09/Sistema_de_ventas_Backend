using SistemaVenta.DTO;

namespace SistemaVenta.BLL.Servicios.Contratos
{
    public interface ICategoriaService
    {
        Task<List<CategoriaDTO>> Lista();
        Task<CategoriaDTO> Crear(CategoriaCreacionDTO modelo);
        Task<bool> Eliminar(int id);
    }
}
