using Microsoft.EntityFrameworkCore;
using ChurrascosAPI.Models.Base;
using ChurrascosAPI.Models.Productos;
using ChurrascosAPI.Models.Inventario;
using ChurrascosAPI.Models.Ventas;
using ChurrascosAPI.Models.Enums;


namespace ChurrascosAPI.Data
{
    public class ChurrascosContext : DbContext
    {
        public ChurrascosContext(DbContextOptions<ChurrascosContext> options) : base(options) { }

        public DbSet<Churrasco> Churrascos { get; set; }
        public DbSet<DulceTipico> DulcesTipicos { get; set; }
        public DbSet<Combo> Combos { get; set; }
        public DbSet<Guarnicion> Guarniciones { get; set; }
        public DbSet<GuarnicionChurrasco> GuarnicionesChurrasco { get; set; }
        public DbSet<ComboItem> ComboItems { get; set; }
        public DbSet<InventarioItem> Inventario { get; set; }
        public DbSet<MovimientoInventario> MovimientosInventario { get; set; }
        public DbSet<Venta> Ventas { get; set; }
        public DbSet<VentaItem> VentasItems { get; set; }
        public DbSet<Sucursal> Sucursales { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Producto>()
                .HasDiscriminator<string>("ProductoTipo")
                .HasValue<Churrasco>("Churrasco")
                .HasValue<DulceTipico>("DulceTipico")
                .HasValue<Combo>("Combo");

            modelBuilder.Entity<GuarnicionChurrasco>()
                .HasKey(gc => new { gc.ChurrascoId, gc.GuarnicionId });

            modelBuilder.Entity<GuarnicionChurrasco>()
                .HasOne(gc => gc.Churrasco)
                .WithMany(c => c.Guarniciones)
                .HasForeignKey(gc => gc.ChurrascoId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<GuarnicionChurrasco>()
                .HasOne(gc => gc.Guarnicion)
                .WithMany()
                .HasForeignKey(gc => gc.GuarnicionId)
                .OnDelete(DeleteBehavior.Restrict);


            modelBuilder.Entity<ComboItem>()
                .HasOne(ci => ci.Combo)
                .WithMany(c => c.Items)
                .HasForeignKey(ci => ci.ComboId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ComboItem>()
                .HasOne(ci => ci.Producto)
                .WithMany()
                .HasForeignKey(ci => ci.ProductoId)
                .OnDelete(DeleteBehavior.Restrict); 

            modelBuilder.Entity<VentaItem>()
                .HasOne(vi => vi.Venta)
                .WithMany(v => v.Items)
                .HasForeignKey(vi => vi.VentaId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<VentaItem>()
                .HasOne(vi => vi.Producto)
                .WithMany()
                .HasForeignKey(vi => vi.ProductoId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<MovimientoInventario>()
                .HasOne(mi => mi.InventarioItem)
                .WithMany(ii => ii.Movimientos)
                .HasForeignKey(mi => mi.InventarioItemId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Sucursal>()
                .Property(s => s.DiasLaborales)
                .HasConversion(
                    v => string.Join(',', v ?? new List<string>()),
                    v => v.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList()
                );

            ConfigurarPrecisionDecimales(modelBuilder);

            ConfigurarIndices(modelBuilder);

 
            SeedData(modelBuilder);
        }

        private void ConfigurarIndices(ModelBuilder modelBuilder)
        {
            // Índices para Producto
            modelBuilder.Entity<Producto>()
                .HasIndex(p => p.Nombre);

            modelBuilder.Entity<Producto>()
                .HasIndex(p => p.Disponible);

            // Índices para Inventario
            modelBuilder.Entity<InventarioItem>()
                .HasIndex(i => i.Tipo);

            modelBuilder.Entity<InventarioItem>()
                .HasIndex(i => i.Cantidad);

            // Índices para Ventas
            modelBuilder.Entity<Venta>()
                .HasIndex(v => v.Fecha);

            modelBuilder.Entity<Venta>()
                .HasIndex(v => v.Estado);

            modelBuilder.Entity<Venta>()
                .HasIndex(v => v.TipoVenta);

            // Índice para número de orden único
            modelBuilder.Entity<Venta>()
                .HasIndex(v => v.NumeroOrden)
                .IsUnique();
        }

        private void ConfigurarPrecisionDecimales(ModelBuilder modelBuilder)
        {
            // Producto base
            modelBuilder.Entity<Producto>()
                .Property(p => p.Precio)
                .HasPrecision(18, 2);

            // Churrasco
            modelBuilder.Entity<Churrasco>()
                .Property(c => c.PrecioBase)
                .HasPrecision(18, 2);

            // DulceTipico
            modelBuilder.Entity<DulceTipico>()
                .Property(d => d.PrecioUnidad)
                .HasPrecision(18, 2);

            // Guarnicion
            modelBuilder.Entity<Guarnicion>()
                .Property(g => g.PrecioExtra)
                .HasPrecision(18, 2);

            // GuarnicionChurrasco
            modelBuilder.Entity<GuarnicionChurrasco>()
                .Property(gc => gc.PrecioEspecial)
                .HasPrecision(18, 2);

            // InventarioItem
            modelBuilder.Entity<InventarioItem>()
                .Property(i => i.Cantidad)
                .HasPrecision(18, 3);

            modelBuilder.Entity<InventarioItem>()
                .Property(i => i.PrecioUnitario)
                .HasPrecision(18, 2);

            modelBuilder.Entity<InventarioItem>()
                .Property(i => i.CostoPromedio)
                .HasPrecision(18, 2);

            // Venta
            modelBuilder.Entity<Venta>()
                .Property(v => v.Subtotal)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Venta>()
                .Property(v => v.Total)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Venta>()
                .Property(v => v.Descuento)
                .HasPrecision(18, 2);

            // VentaItem
            modelBuilder.Entity<VentaItem>()
                .Property(vi => vi.PrecioUnitario)
                .HasPrecision(18, 2);

            modelBuilder.Entity<VentaItem>()
                .Property(vi => vi.Subtotal)
                .HasPrecision(18, 2);

            // ComboItem
            modelBuilder.Entity<ComboItem>()
                .Property(ci => ci.PrecioUnitario)
                .HasPrecision(18, 2);

            modelBuilder.Entity<ComboItem>()
                .Property(ci => ci.PrecioOriginal)
                .HasPrecision(18, 2);

            // Combo
            modelBuilder.Entity<Combo>()
                .Property(c => c.MontoDescuento)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Combo>()
                .Property(c => c.PorcentajeDescuento)
                .HasPrecision(5, 2);
        }

        private void SeedData(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Guarnicion>().HasData(
                new Guarnicion
                {
                    Id = 1,
                    Nombre = "Frijoles",
                    PrecioExtra = 0,
                    Disponible = true,
                    CantidadStock = 100,
                    StockMinimo = 20,
                    FechaCreacion = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                },
                new Guarnicion
                {
                    Id = 2,
                    Nombre = "Chile de Árbol",
                    PrecioExtra = 0,
                    Disponible = true,
                    CantidadStock = 50,
                    StockMinimo = 10,
                    FechaCreacion = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                },
                new Guarnicion
                {
                    Id = 3,
                    Nombre = "Cebollín",
                    PrecioExtra = 0,
                    Disponible = true,
                    CantidadStock = 30,
                    StockMinimo = 10,
                    FechaCreacion = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                },
                new Guarnicion
                {
                    Id = 4,
                    Nombre = "Tortillas",
                    PrecioExtra = 0,
                    Disponible = true,
                    CantidadStock = 200,
                    StockMinimo = 50,
                    FechaCreacion = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                },
                new Guarnicion
                {
                    Id = 5,
                    Nombre = "Chirmol",
                    PrecioExtra = 0,
                    Disponible = true,
                    CantidadStock = 40,
                    StockMinimo = 15,
                    FechaCreacion = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                }
            );

            modelBuilder.Entity<InventarioItem>().HasData(
                // Carnes
                new InventarioItem
                {
                    Id = 1,
                    Nombre = "Carne Puyazo",
                    Tipo = TipoInventario.Carne,
                    Cantidad = 50,
                    Unidad = "libras",
                    StockMinimo = 10,
                    StockMaximo = 100,
                    PrecioUnitario = 25.00m,
                    CostoPromedio = 20.00m,
                    PuntoReorden = 15,
                    UltimaActualizacion = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                },
                new InventarioItem
                {
                    Id = 2,
                    Nombre = "Carne Culotte",
                    Tipo = TipoInventario.Carne,
                    Cantidad = 40,
                    Unidad = "libras",
                    StockMinimo = 8,
                    StockMaximo = 80,
                    PrecioUnitario = 30.00m,
                    CostoPromedio = 24.00m,
                    PuntoReorden = 12,
                    UltimaActualizacion = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                },
                new InventarioItem
                {
                    Id = 3,
                    Nombre = "Costilla",
                    Tipo = TipoInventario.Carne,
                    Cantidad = 35,
                    Unidad = "libras",
                    StockMinimo = 5,
                    StockMaximo = 60,
                    PrecioUnitario = 22.00m,
                    CostoPromedio = 18.00m,
                    PuntoReorden = 10,
                    UltimaActualizacion = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                },

                // Combustible
                new InventarioItem
                {
                    Id = 4,
                    Nombre = "Carbón",
                    Tipo = TipoInventario.Combustible,
                    Cantidad = 20,
                    Unidad = "sacos",
                    StockMinimo = 5,
                    StockMaximo = 50,
                    PrecioUnitario = 8.00m,
                    CostoPromedio = 6.00m,
                    PuntoReorden = 8,
                    UltimaActualizacion = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                },
                new InventarioItem
                {
                    Id = 5,
                    Nombre = "Leña",
                    Tipo = TipoInventario.Combustible,
                    Cantidad = 15,
                    Unidad = "sacos",
                    StockMinimo = 3,
                    StockMaximo = 30,
                    PrecioUnitario = 6.00m,
                    CostoPromedio = 4.50m,
                    PuntoReorden = 5,
                    UltimaActualizacion = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                },

                // Empaques
                new InventarioItem
                {
                    Id = 6,
                    Nombre = "Cajas 6 dulces",
                    Tipo = TipoInventario.Empaque,
                    Cantidad = 100,
                    Unidad = "unidades",
                    StockMinimo = 20,
                    StockMaximo = 200,
                    PrecioUnitario = 0.50m,
                    CostoPromedio = 0.30m,
                    PuntoReorden = 30,
                    UltimaActualizacion = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                },
                new InventarioItem
                {
                    Id = 7,
                    Nombre = "Cajas 12 dulces",
                    Tipo = TipoInventario.Empaque,
                    Cantidad = 80,
                    Unidad = "unidades",
                    StockMinimo = 15,
                    StockMaximo = 150,
                    PrecioUnitario = 0.75m,
                    CostoPromedio = 0.50m,
                    PuntoReorden = 25,
                    UltimaActualizacion = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                },
                new InventarioItem
                {
                    Id = 8,
                    Nombre = "Cajas 24 dulces",
                    Tipo = TipoInventario.Empaque,
                    Cantidad = 60,
                    Unidad = "unidades",
                    StockMinimo = 10,
                    StockMaximo = 120,
                    PrecioUnitario = 1.00m,
                    CostoPromedio = 0.70m,
                    PuntoReorden = 20,
                    UltimaActualizacion = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                }
            );
        }
    }
}