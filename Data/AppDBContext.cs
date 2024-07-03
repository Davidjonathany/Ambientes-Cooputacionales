using Microsoft.EntityFrameworkCore;
using ProyectoFinalVentasMVC.Models;

namespace ProyectoFinalVentasMVC.Data
{
    public class AppDBContext : DbContext
    {
        public AppDBContext(DbContextOptions<AppDBContext> options) : base(options)
        {
        }

        // DbSet para cada modelo
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Factura> Facturas { get; set; }
        public DbSet<FacturaDetalle> FacturaDetalles { get; set; }
        public DbSet<Producto> Productos { get; set; }
        public DbSet<TipoProducto> TiposProductos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuración de Usuario
            modelBuilder.Entity<Usuario>(tb =>
            {
                tb.HasKey(col => col.Id);
                tb.Property(col => col.Id)
                    .UseIdentityColumn()
                    .ValueGeneratedOnAdd();

                tb.Property(col => col.Nombre).HasMaxLength(50);
                tb.Property(col => col.Correo).HasMaxLength(50);
                tb.Property(col => col.Clave).HasMaxLength(50);
                tb.Property(col => col.Rol).HasMaxLength(50).IsRequired();

                tb.ToTable("Usuario");
            });

            // Configuración de Cliente
            modelBuilder.Entity<Cliente>(tb =>
            {
                tb.HasKey(col => col.Id);
                tb.Property(col => col.Id)
                    .UseIdentityColumn()
                    .ValueGeneratedOnAdd();

                tb.Property(col => col.Nombres).HasMaxLength(50);
                tb.Property(col => col.Apellidos).HasMaxLength(50);
                tb.Property(col => col.Cedula);
                tb.Property(col => col.Direccion).HasMaxLength(50);
                tb.Property(col => col.Telefono);
                tb.Property(col => col.Correo).HasMaxLength(50);

                tb.ToTable("Cliente");
            });

            // Configuración de Factura
            modelBuilder.Entity<Factura>(tb =>
            {
                tb.HasKey(col => col.Id);
                tb.Property(col => col.Id)
                    .UseIdentityColumn()
                    .ValueGeneratedOnAdd();

                tb.Property(col => col.Fecha).IsRequired();
                tb.Property(col => col.IdCliente).IsRequired();
                tb.Property(col => col.Total);
                tb.Property(col => col.Iva);
                tb.Property(col => col.SubTotal);

                tb.HasOne(f => f.Cliente)
                    .WithMany()
                    .HasForeignKey(f => f.IdCliente);

                tb.HasMany(f => f.Detalles) // Relación uno a muchos con FacturaDetalle
                    .WithOne(fd => fd.Factura)
                    .HasForeignKey(fd => fd.IdFactura)
                    .OnDelete(DeleteBehavior.Cascade); // Eliminación en cascada si se elimina la factura

                tb.ToTable("Factura");
            });

            // Configuración de FacturaDetalle
            modelBuilder.Entity<FacturaDetalle>(tb =>
            {
                tb.HasKey(col => col.Id);
                tb.Property(col => col.Id)
                    .UseIdentityColumn()
                    .ValueGeneratedOnAdd();

                tb.Property(col => col.IdFactura).IsRequired();
                tb.Property(col => col.IdProducto).IsRequired();
                tb.Property(col => col.Cantidad).IsRequired();
                tb.Property(col => col.Descuento);
                tb.Property(col => col.Total);

                tb.HasOne(fd => fd.Factura)
                    .WithMany(f => f.Detalles)
                    .HasForeignKey(fd => fd.IdFactura);

                tb.HasOne(fd => fd.Producto)
                    .WithMany()
                    .HasForeignKey(fd => fd.IdProducto);

                tb.ToTable("FacturaDetalle");
            });



            // Configuración de Producto

            modelBuilder.Entity<Producto>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id)
                .UseIdentityColumn()
                .ValueGeneratedOnAdd();
                entity.Property(e => e.Nombre).HasMaxLength(50);
                entity.Property(e => e.CodigoBarras).HasMaxLength(50);
                entity.Property(e => e.Iva).IsRequired();
                entity.Property(col => col.Precio).IsRequired();

                // Configuración de la relación uno a uno con TipoProducto
                entity.HasOne(e => e.TipoProducto)
                      .WithMany() // No especificamos la navegación inversa aquí porque es uno a uno
                      .HasForeignKey(e => e.IdTipo)
                      .IsRequired()
                      .OnDelete(DeleteBehavior.Restrict); // Restringir la eliminación para mantener la integridad

                entity.ToTable("Producto");
            });

            // Configuración de TipoProducto con eliminación en cascada
            modelBuilder.Entity<TipoProducto>(tb =>
            {
                tb.HasKey(col => col.TipoId);
                tb.Property(col => col.TipoId)
                    .UseIdentityColumn()
                    .ValueGeneratedOnAdd();

                tb.Property(col => col.Tipo).HasMaxLength(100).IsRequired();

                // Configuración de la relación uno a muchos con Producto
                tb.HasMany(tp => tp.Productos)
                  .WithOne(p => p.TipoProducto)
                  .HasForeignKey(p => p.IdTipo)
                  .OnDelete(DeleteBehavior.Cascade); // Eliminación en cascada para eliminar productos si se elimina el tipo de producto

                tb.ToTable("TipoProducto");
            });

            modelBuilder.Entity<Usuario>().ToTable("Usuario");
            modelBuilder.Entity<Cliente>().ToTable("Cliente");
            modelBuilder.Entity<Factura>().ToTable("Factura");
            modelBuilder.Entity<FacturaDetalle>().ToTable("FacturaDetalle");
            modelBuilder.Entity<Producto>().ToTable("Producto");
            modelBuilder.Entity<TipoProducto>().ToTable("TipoProducto");
        }
    }
}
