using SistemaVenta.DAL.DBContext;
using SistemaVenta.DAL.Repositorios.Contratos;
using SistemaVenta.Model;

namespace SistemaVenta.DAL.Repositorios
{
    public class VentaRepository : GenericRepository<Venta>, IVentaRepository
    {
        private readonly DbventaContext _dbventaContext;

        public VentaRepository(DbventaContext dbventaContext) : base(dbventaContext)
        {
            _dbventaContext = dbventaContext;
        }

        public async Task<Venta> Registrar(Venta modelo)
        {
            // Se inicializa una nueva instancia de Venta
            Venta VentaGenerada = new Venta();

            // Se inicia una transacción en el contexto de la base de datos
            using (var trasaction = _dbventaContext.Database.BeginTransaction())
            {
                try
                {
                    // Por cada detalle de venta en el modelo
                    foreach (DetalleVenta dv in modelo.DetalleVenta)
                    {
                        // Se encuentra el producto correspondiente en la base de datos
                        Producto producto_encontrado = _dbventaContext.Productos
                            .Where(p => p.IdProducto == dv.IdProducto)
                            .First();

                        // Se actualiza el stock del producto
                        producto_encontrado.Stock = producto_encontrado.Stock - dv.Cantidad;
                        _dbventaContext.Productos.Update(producto_encontrado);
                    }

                    // Se guardan los cambios en la base de datos
                    await _dbventaContext.SaveChangesAsync();

                    // Se obtiene el número de documento correlativo de la base de datos
                    NumeroDocumento correlativo = _dbventaContext.NumeroDocumentos.First();
                    correlativo.UltimoNumero = correlativo.UltimoNumero + 1;
                    correlativo.FechaRegistro = DateTime.Now;

                    // Se actualiza el número de documento correlativo
                    _dbventaContext.NumeroDocumentos.Update(correlativo);

                    // Se guardan los cambios en la base de datos
                    await _dbventaContext.SaveChangesAsync();

                    // Se genera un número de venta con un formato específico
                    int cantidadDigitos = 4;
                    string ceros = string.Concat(Enumerable.Repeat("0", cantidadDigitos));
                    string numeroVenta = ceros + correlativo.UltimoNumero.ToString();
                    numeroVenta = numeroVenta.Substring(numeroVenta.Length - cantidadDigitos, cantidadDigitos);

                    // Se asigna el número de venta al modelo de venta
                    modelo.NumeroDocumento = numeroVenta;

                    // Se añade el modelo de venta al contexto de la base de datos
                    await _dbventaContext.Venta.AddAsync(modelo);

                    // Se guardan los cambios en la base de datos
                    await _dbventaContext.SaveChangesAsync();

                    // Se asigna el modelo de venta generado para devolverlo al final del método
                    VentaGenerada = modelo;

                    // Se confirma la transacción
                    trasaction.Commit();
                }
                catch (Exception ex)
                {
                    // En caso de error, se hace un rollback de la transacción
                    trasaction.Rollback();
                    throw; // Se lanza la excepción para ser manejada en un nivel superior
                }

                // Se devuelve el modelo de venta generado
                return VentaGenerada;
            }
        }
    }
}
