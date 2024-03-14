using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SistemaVenta.BLL.Servicios.Contratos;
using SistemaVenta.DAL.Repositorios.Contratos;
using SistemaVenta.DTO;
using SistemaVenta.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaVenta.BLL.Servicios
{
    public class ProductoService:IProductoService
    {
        private readonly IGenericRepository<Producto> _ProductoRepositorio;
        private readonly IMapper _mapper;

        public ProductoService(IGenericRepository<Producto> productoRepositorio, IMapper mapper)
        {
            _ProductoRepositorio = productoRepositorio;
            _mapper = mapper;
        }

        public async Task<ProductoDTO> Crear(ProductoDTO modelo)
        {
            try
            {
                var ProductoExistente = await _ProductoRepositorio.Obtener(u=> u.Nombre == modelo.Nombre);
                if (ProductoExistente != null) 
                {
                    throw new TaskCanceledException("El producto ya existe");
                }
                var ProductoCreado = await _ProductoRepositorio.Crear(_mapper.Map<Producto>(modelo));
                if (ProductoCreado.IdProducto == 0)
                {
                    throw new TaskCanceledException("El producto no se pudo crear");
                }
                return _mapper.Map<ProductoDTO>(ProductoCreado);
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> Editar(ProductoDTO modelo, int id)
        {
            try
            {
                var productoModelo = _mapper.Map<ProductoDTO>(modelo);
                var productoEncontrado = await _ProductoRepositorio.Obtener(u => u.IdProducto == id);
                if (productoEncontrado == null)
                {
                    throw new TaskCanceledException("No se encontro el producto");
                }
                productoEncontrado.Nombre = productoModelo.Nombre;
                productoEncontrado.IdCategoria = productoModelo.IdCategoria;
                productoEncontrado.Stock = productoModelo.Stock;
                productoEncontrado.Precio = Convert.ToDecimal(productoModelo.Precio);
                productoEncontrado.EsActivo = Convert.ToBoolean(productoModelo.EsActivo);
                bool respuesta = await _ProductoRepositorio.Editar(productoEncontrado);
                if(!respuesta)
                {
                    throw new TaskCanceledException("No se puede editar el producto");
                }
                return respuesta;
            }
            catch
            {
                throw;
            }

        }

        public async Task<bool> Eliminar(int id)
        {
            var ProductoEncontrado = await _ProductoRepositorio.Obtener(u => u.IdProducto == id);
            if (ProductoEncontrado == null)
            {
                throw new TaskCanceledException("Usuario no encontrado");
            }
            bool respuesta = await _ProductoRepositorio.Eliminar(ProductoEncontrado);
            if (respuesta == false)
            {
                throw new TaskCanceledException("No se pudo eliminar");
            }
            return respuesta;
        }

        public async Task<List<ProductoDTO>> Lista()
        {
            try 
            {
                var queryproducto = await _ProductoRepositorio.Consultar();
                var listaproductos= queryproducto.Include(cat => cat.IdCategoriaNavigation).ToList();
                return _mapper.Map<List<ProductoDTO>>(listaproductos);
            }
            catch 
            {
                throw;
            }
        }
    }
}
