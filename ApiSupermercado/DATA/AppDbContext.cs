using Microsoft.EntityFrameworkCore;
using SupermercadoAPI.Models; 

namespace SupermercadoAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Rol> Roles { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Proveedor> Proveedores { get; set; }

      
        public DbSet<Producto> Productos { get; set; }
        public DbSet<InventarioLote> Inventario_Lotes { get; set; }
        public DbSet<HistorialMovimiento> Historial_Movimientos { get; set; }

        
        public DbSet<VentaFisica> Ventas_Fisicas { get; set; }
        public DbSet<DetalleVentaFisica> Detalles_Venta_Fisica { get; set; }

        
        public DbSet<PedidoWebPickup> Pedidos_Web_Pickup { get; set; }
        public DbSet<DetallePedidoWeb> Detalles_Pedido_Web { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            
            modelBuilder.Entity<PedidoWebPickup>()
                .HasOne(p => p.Cliente)
                .WithMany()
                .HasForeignKey(p => p.ID_Usuario_Cliente);

            modelBuilder.Entity<PedidoWebPickup>()
                .HasOne(p => p.CajeroAtendio)
                .WithMany()
                .HasForeignKey(p => p.ID_Usuario_Cajero_Atendio);

            foreach (var relationship in modelBuilder.Model.GetEntityTypes()
                .SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }
        }
    }
}