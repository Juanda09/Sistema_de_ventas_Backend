using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using SistemaVenta.DTO;
using SistemaVenta.Model;

namespace SistemaVenta.Utility
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            #region Rol
            CreateMap<Rol, RolDTO>().ReverseMap();
            #endregion

            #region Menu
            CreateMap<Menu,MenuDTO>().ReverseMap();
            #endregion

            #region Usuario
            CreateMap<Usuario, UsuarioDTO>()
                .ForMember(destino => destino.RolDescripcion,
                opt => opt.MapFrom(origen => origen.IdRolNavigation.Nombre))
                .ForMember(destino => destino.EsActivo,
                opt => opt.MapFrom(origen => origen.EsActivo == true ? 1:0)
                );

            CreateMap<Usuario, SesionDTO>()
                .ForMember(destino => destino.RolDescripcion,
                opt => opt.MapFrom(origen => origen.IdRolNavigation.Nombre));

            CreateMap<UsuarioDTO, Usuario>()
                .ForMember(destino => destino.IdRolNavigation,
                opt => opt.Ignore())
                .ForMember(destino => destino.EsActivo,
                opt => opt.MapFrom(origen => origen.EsActivo == 1 ? true : false)
                );
            #endregion

            #region Categoria
            CreateMap<Categoria, CategoriaDTO>().ReverseMap();
            #endregion

            #region Producto
            CreateMap<Producto, ProductoDTO>()
                .ForMember(destino => 
                destino.DescripcionCategoria,
                opt => opt.MapFrom(Origen => Origen.IdCategoriaNavigation.Nombre)
                )
                .ForMember(destino =>
                destino.Precio,
                opt => opt.MapFrom(Origen => Convert.ToString(Origen.Precio.Value, new CultureInfo("es-CO"))
                ))
                .ForMember(destino => destino.EsActivo,
                opt => opt.MapFrom(origen => origen.EsActivo == true ? 1 : 0)
                );
            CreateMap<ProductoDTO, Producto>()
                .ForMember(destino =>
                destino.IdCategoriaNavigation,
                opt => opt.Ignore()
                )
                .ForMember(destino =>
                destino.Precio,
                opt => opt.MapFrom(Origen => Convert.ToDecimal(Origen.Precio, new CultureInfo("es-CO"))
                ))
                .ForMember(destino => destino.EsActivo,
                opt => opt.MapFrom(origen => origen.EsActivo == 1 ?true : false)
                );
            #endregion

            #region Venta
            CreateMap<Venta, VentaDTO>()
                .ForMember(destino =>
                destino.TotalTexto,
                opt => opt.MapFrom(Origen => Convert.ToString(Origen.Total.Value, new CultureInfo("es-CO"))
                ))
                .ForMember(destino =>
                destino.FechaRegistro,
                opt => opt.MapFrom(Origen => Convert.ToString(Origen.FechaRegistro.Value.ToString("dd/MM/yyyy"))
                ));
            CreateMap<VentaDTO, Venta>()
                .ForMember(destino =>
                destino.Total,
                opt => opt.MapFrom(Origen => Convert.ToDecimal(Origen.TotalTexto, new CultureInfo("es-CO"))
                ));
            #endregion

            #region Detalle Venta
            CreateMap<DetalleVenta, DetalleVentaDTO>()
                .ForMember(destino => destino.DescripcionProducto,
                opt => opt.MapFrom(origen => origen.IdProductoNavigation.Nombre))
                .ForMember(destino =>
                destino.PrecioTexto,
                opt => opt.MapFrom(Origen => Convert.ToString(Origen.Precio.Value, new CultureInfo("es-CO"))
                ))
                .ForMember(destino =>
                destino.TotalTexto,
                opt => opt.MapFrom(Origen => Convert.ToString(Origen.Total.Value, new CultureInfo("es-CO"))
                ));
            CreateMap<DetalleVentaDTO, DetalleVenta>()
                .ForMember(destino =>
                destino.Precio,
                opt => opt.MapFrom(Origen => Convert.ToDecimal(Origen.PrecioTexto, new CultureInfo("es-CO"))
                ))
                .ForMember(destino =>
                destino.Total,
                opt => opt.MapFrom(Origen => Convert.ToDecimal(Origen.TotalTexto, new CultureInfo("es-CO"))
                ));
            #endregion

            #region Reportes
            CreateMap<DetalleVenta, ReporteDTO>()
             .ForMember(destino =>
                destino.FechaRegistro,
                opt => opt.MapFrom(Origen => Convert.ToString(Origen.IdVentaNavigation.FechaRegistro.Value.ToString("dd/MM/yyyy"))
                ))
             .ForMember(destino => 
             destino.NumeroDocumento,
             opt => opt.MapFrom(Origen => Origen.IdVentaNavigation.NumeroDocumento))
            .ForMember(destino =>
                destino.TipoPago,
                opt => opt.MapFrom(Origen => Origen.IdVentaNavigation.TipoPago)
                )
             .ForMember(destino =>
                destino.TotalVenta,
                opt => opt.MapFrom(Origen => Convert.ToString(Origen.IdVentaNavigation.Total.Value, new CultureInfo("es-CO"))
                ))
              .ForMember(destino =>
                destino.Producto,
                opt => opt.MapFrom(Origen => Origen.IdProductoNavigation.Nombre)
                )
                .ForMember(destino =>
                destino.TipoPago,
                opt => opt.MapFrom(Origen => Origen.IdVentaNavigation.TipoPago)
                )
                .ForMember(destino =>
                destino.Precio,
                opt => opt.MapFrom(Origen => Convert.ToString(Origen.Precio.Value, new CultureInfo("es-CO"))
                ))
                .ForMember(destino =>
                destino.Total,
                opt => opt.MapFrom(Origen => Convert.ToString(Origen.Total.Value, new CultureInfo("es-CO"))
                ));
            #endregion
        }
    }
}
