using Microsoft.EntityFrameworkCore;
using SupermercadoAPI.Models;

namespace SupermercadoAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Rol>                 Roles                 { get; set; }
        public DbSet<Usuario>             Usuarios              { get; set; }
        public DbSet<Categoria>           Categorias            { get; set; }
        public DbSet<Proveedor>           Proveedores           { get; set; }
        public DbSet<UbicacionBodega>     Ubicaciones_Bodega    { get; set; }
        public DbSet<Producto>            Productos             { get; set; }
        public DbSet<InventarioLote>      Inventario_Lotes      { get; set; }
        public DbSet<HistorialMovimiento> Historial_Movimientos { get; set; }
        public DbSet<Caja>                Cajas                 { get; set; }
        public DbSet<VentaFisica>         Ventas_Fisicas        { get; set; }
        public DbSet<DetalleVentaFisica>  Detalles_Venta_Fisica { get; set; }
        public DbSet<PedidoWebPickup>     Pedidos_Web_Pickup    { get; set; }
        public DbSet<DetallePedidoWeb>    Detalles_Pedido_Web   { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // PedidoWebPickup: dos FK a Usuario
            modelBuilder.Entity<PedidoWebPickup>()
                .HasOne(p => p.Cliente).WithMany()
                .HasForeignKey(p => p.ID_Usuario_Cliente)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<PedidoWebPickup>()
                .HasOne(p => p.CajeroAtendio).WithMany()
                .HasForeignKey(p => p.ID_Usuario_Cajero_Atendio)
                .OnDelete(DeleteBehavior.Restrict);

            // VentaFisica → Cajero
            modelBuilder.Entity<VentaFisica>()
                .HasOne(v => v.Cajero).WithMany()
                .HasForeignKey(v => v.ID_Usuario_Cajero)
                .OnDelete(DeleteBehavior.Restrict);

            // VentaFisica → Caja
            modelBuilder.Entity<VentaFisica>()
                .HasOne(v => v.Caja).WithMany(c => c.Ventas)
                .HasForeignKey(v => v.ID_Caja)
                .OnDelete(DeleteBehavior.Restrict);

            // Caja → Cajero
            modelBuilder.Entity<Caja>()
                .HasOne(c => c.Cajero).WithMany()
                .HasForeignKey(c => c.ID_Usuario_Cajero)
                .OnDelete(DeleteBehavior.Restrict);

            // InventarioLote → Usuario
            modelBuilder.Entity<InventarioLote>()
                .HasOne(l => l.UsuarioRecibio).WithMany()
                .HasForeignKey(l => l.ID_Usuario_Recibio)
                .OnDelete(DeleteBehavior.Restrict);

            // HistorialMovimiento → Usuario
            modelBuilder.Entity<HistorialMovimiento>()
                .HasOne(m => m.UsuarioResponsable).WithMany()
                .HasForeignKey(m => m.ID_Usuario_Responsable)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
