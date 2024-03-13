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
    public class MenuService: IMenuService
    {
        private readonly IGenericRepository<Usuario> _UsuarioRepositorio;
        private readonly IGenericRepository<MenuRol> _MenuRolRepositorio;
        private readonly IGenericRepository<Menu> _MenuRepositorio;
        private readonly IMapper _mapper;

        public MenuService(IGenericRepository<Usuario> usuarioRepositorio, IGenericRepository<MenuRol> menuRolRepositorio, IGenericRepository<Menu> menuRepositorio, IMapper mapper)
        {
            _UsuarioRepositorio = usuarioRepositorio;
            _MenuRolRepositorio = menuRolRepositorio;
            _MenuRepositorio = menuRepositorio;
            _mapper = mapper;
        }

        public async Task<List<MenuDTO>> Lista(int idUsuario)
        {
            IQueryable<Usuario> tbusuarios = await _UsuarioRepositorio.Consultar(u => u.IdUsuario == idUsuario);
            IQueryable<MenuRol> tbMenuRol = await _MenuRolRepositorio.Consultar();
            IQueryable<Menu> tbMenu = await _MenuRepositorio.Consultar();
            try
            {
                IQueryable<Menu> tbResultado =(from u in tbusuarios join mr in tbMenuRol on u.IdRol equals mr.IdRol
                                               join m in tbMenu on mr.IdMenu equals m.IdMenu
                                               select m).AsQueryable();
                var listamenus = tbResultado.ToList();
                return _mapper.Map<List<MenuDTO>>(listamenus);
            }
            catch
            {
                throw;
            }
        }
    }
}
