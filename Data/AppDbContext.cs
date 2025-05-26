using Microsoft.EntityFrameworkCore;
using FullApi.Models;

namespace FacturaElectronicaApi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Producto> Productos { get; set; }
        public DbSet<Factura> Facturas { get; set; }
        public DbSet<DetalleFactura> DetalleFacturas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Clave compuesta para DetalleFactura
            modelBuilder.Entity<DetalleFactura>()
                .HasKey(df => new { df.FacturaId, df.ProductoId });

            // Factura - Cliente (muchas facturas por cliente)
            modelBuilder.Entity<Factura>()
                .HasOne(f => f.Cliente)
                .WithMany(c => c.Facturas)
                .HasForeignKey(f => f.ClienteId);

            // DetalleFactura - Factura (muchos detalles por factura)
            modelBuilder.Entity<DetalleFactura>()
                .HasOne(df => df.Factura)
                .WithMany(f => f.Detalles)
                .HasForeignKey(df => df.FacturaId)
                .OnDelete(DeleteBehavior.Restrict); 

            // DetalleFactura - Producto (muchos detalles por producto)
            modelBuilder.Entity<DetalleFactura>()
                .HasOne(df => df.Producto)
                .WithMany(p => p.DetallesFactura) 
                .HasForeignKey(df => df.ProductoId)
                .OnDelete(DeleteBehavior.Restrict); 
        }
    }
}
