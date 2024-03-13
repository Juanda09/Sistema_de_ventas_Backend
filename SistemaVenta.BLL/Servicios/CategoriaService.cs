using AutoMapper;
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
    public class CategoriaService:ICategoriaService
    {
        private readonly IGenericRepository<Categoria> _CategoriaRepositorio;
        private readonly IMapper _mapper;

        public CategoriaService(IGenericRepository<Categoria> categoriaRepositorio, IMapper mapper)
        {
            _CategoriaRepositorio = categoriaRepositorio;
            _mapper = mapper;
        }

        public async Task<CategoriaDTO> Crear(CategoriaCreacionDTO modelo)
        {
            try
            {
                var CategoriaExistente = await _CategoriaRepositorio.Obtener(u=> u.Nombre == modelo.Nombre);
                if (CategoriaExistente != null)
                {
                    throw new TaskCanceledException("La categoria ya existe");
                }
                var Categorianueva = await _CategoriaRepositorio.Crear(_mapper.Map<Categoria>(modelo));
                if (Categorianueva.IdCategoria == 0)
                {
                    throw new TaskCanceledException("La categoria no pudo ser creada");
                }
                return _mapper.Map<CategoriaDTO>(Categorianueva);
            }
            catch 
            {
                throw;
            }
        }

        public async Task<bool> Eliminar(int id)
        {
            var CategoriaEncontrada = await _CategoriaRepositorio.Obtener(u => u.IdCategoria == id);
            if (CategoriaEncontrada != null)
            {
                throw new TaskCanceledException("La categoria no fue encontrada");
            }
            bool respuesta = await _CategoriaRepositorio.Eliminar(CategoriaEncontrada);
            if (!respuesta)
            {
                throw new TaskCanceledException("La categoria no se pudo eliminar");
            }
            return respuesta;
        }

        public async Task<List<CategoriaDTO>> Lista()
        {
            try
            {
                var Listarcategorias = await _CategoriaRepositorio.Consultar();
                return _mapper.Map<List<CategoriaDTO>>(Listarcategorias.ToList());
            }
            catch
            {
                throw;
            }
        }
    }
}
