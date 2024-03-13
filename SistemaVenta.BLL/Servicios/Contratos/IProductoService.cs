using SistemaVenta.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaVenta.BLL.Servicios.Contratos
{
    public interface IProductoService
    {
        Task<List<ProductoDTO>> Lista();
        Task<ProductoDTO> Crear(ProductosCreacionDTO modelo);

        Task<bool> Editar(ProductosCreacionDTO modelo,int id);

        Task<bool> Eliminar(int id);
    }
}
