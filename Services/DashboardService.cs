using Microsoft.EntityFrameworkCore;
using ChurrascosAPI.Data;
using ChurrascosAPI.Models;
using ChurrascosAPI.Models.Productos;

namespace ChurrascosAPI.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly ChurrascosContext _context;

        public DashboardService(ChurrascosContext context)
        {
            _context = context;
        }

        public async Task<DashboardData> GetDashboardDataAsync()
        {
            var hoy = DateTime.Today;
            var inicioMes = new DateTime(hoy.Year, hoy.Month, 1);
            var finMes = inicioMes.AddMonths(1).AddDays(-1);

            var ventasHoy = await _context.Ventas
                .Where(v => v.Fecha.Date == hoy)
                .SumAsync(v => v.Total);

            var ventasMes = await _context.Ventas
                .Where(v => v.Fecha >= inicioMes && v.Fecha <= finMes)
                .SumAsync(v => v.Total);

            var cantidadVentasHoy = await _context.Ventas
                .CountAsync(v => v.Fecha.Date == hoy);

            var cantidadVentasMes = await _context.Ventas
                .CountAsync(v => v.Fecha >= inicioMes && v.Fecha <= finMes);

            // Productos mas vendidos
            var churrascosMasVendidos = await _context.VentasItems
                .Include(vi => vi.Producto)
                .Where(vi => vi.Producto is Churrasco && vi.Venta.Fecha >= inicioMes)
                .GroupBy(vi => vi.Producto.Nombre)
                .Select(g => new ProductoVendido 
                { 
                    Nombre = g.Key, 
                    Cantidad = g.Sum(vi => vi.Cantidad),
                    Ingresos = g.Sum(vi => vi.Subtotal)
                })
                .OrderByDescending(p => p.Cantidad)
                .Take(5)
                .ToListAsync();

            var dulcesMasVendidos = await _context.VentasItems
                .Include(vi => vi.Producto)
                .Where(vi => vi.Producto is DulceTipico && vi.Venta.Fecha >= inicioMes)
                .GroupBy(vi => vi.Producto.Nombre)
                .Select(g => new ProductoVendido 
                { 
                    Nombre = g.Key, 
                    Cantidad = g.Sum(vi => vi.Cantidad),
                    Ingresos = g.Sum(vi => vi.Subtotal)
                })
                .OrderByDescending(p => p.Cantidad)
                .Take(5)
                .ToListAsync();

            var guarnicionesFrecuentes = await _context.GuarnicionesChurrasco
                .Include(gc => gc.Guarnicion)
                .Include(gc => gc.Churrasco)
                .Where(gc => _context.VentasItems
                    .Any(vi => vi.ProductoId == gc.ChurrascoId && vi.Venta.Fecha >= inicioMes))
                .GroupBy(gc => gc.Guarnicion.Nombre)
                .Select(g => new GuarnicionPopular 
                { 
                    Nombre = g.Key, 
                    Veces = g.Count()
                })
                .OrderByDescending(g => g.Veces)
                .Take(5)
                .ToListAsync();

            var ingresosChurrascos = await _context.VentasItems
                .Include(vi => vi.Producto)
                .Where(vi => vi.Producto is Churrasco && vi.Venta.Fecha >= inicioMes)
                .SumAsync(vi => vi.Subtotal);

            var ingresosDulces = await _context.VentasItems
                .Include(vi => vi.Producto)
                .Where(vi => vi.Producto is DulceTipico && vi.Venta.Fecha >= inicioMes)
                .SumAsync(vi => vi.Subtotal);

            var ingresosCombos = await _context.VentasItems
                .Include(vi => vi.Producto)
                .Where(vi => vi.Producto is Combo && vi.Venta.Fecha >= inicioMes)
                .SumAsync(vi => vi.Subtotal);

            var itemsBajoStock = await _context.Inventario
                .Where(i => i.Cantidad <= i.StockMinimo)
                .Select(i => new ItemBajoStock 
                { 
                    Nombre = i.Nombre, 
                    CantidadActual = i.Cantidad, 
                    StockMinimo = i.StockMinimo,
                    Tipo = i.Tipo.ToString()
                })
                .ToListAsync();

            var ventasPorDia = new List<VentaDiaria>();
            for (int i = 6; i >= 0; i--)
            {
                var fecha = hoy.AddDays(-i);
                var ventasDelDia = await _context.Ventas
                    .Where(v => v.Fecha.Date == fecha)
                    .SumAsync(v => v.Total);
                
                ventasPorDia.Add(new VentaDiaria 
                { 
                    Fecha = fecha.ToString("dd/MM"), 
                    Total = ventasDelDia 
                });
            }

            return new DashboardData
            {
                VentasHoy = ventasHoy,
                VentasMes = ventasMes,
                CantidadVentasHoy = cantidadVentasHoy,
                CantidadVentasMes = cantidadVentasMes,
                ChurrascosMasVendidos = churrascosMasVendidos,
                DulcesMasVendidos = dulcesMasVendidos,
                GuarnicionesFrecuentes = guarnicionesFrecuentes,
                IngresosChurrascos = ingresosChurrascos,
                IngresosDulces = ingresosDulces,
                IngresosCombos = ingresosCombos,
                ItemsBajoStock = itemsBajoStock,
                VentasPorDia = ventasPorDia,
                FechaActualizacion = DateTime.Now
            };
        }
    }


    public class DashboardData
    {
        public decimal VentasHoy { get; set; }
        public decimal VentasMes { get; set; }
        public int CantidadVentasHoy { get; set; }
        public int CantidadVentasMes { get; set; }
        public List<ProductoVendido> ChurrascosMasVendidos { get; set; } = new();
        public List<ProductoVendido> DulcesMasVendidos { get; set; } = new();
        public List<GuarnicionPopular> GuarnicionesFrecuentes { get; set; } = new();
        public decimal IngresosChurrascos { get; set; }
        public decimal IngresosDulces { get; set; }
        public decimal IngresosCombos { get; set; }
        public List<ItemBajoStock> ItemsBajoStock { get; set; } = new();
        public List<VentaDiaria> VentasPorDia { get; set; } = new();
        public DateTime FechaActualizacion { get; set; }
    }

    public class ProductoVendido
    {
        public string Nombre { get; set; } = string.Empty;
        public int Cantidad { get; set; }
        public decimal Ingresos { get; set; }
    }

    public class GuarnicionPopular
    {
        public string Nombre { get; set; } = string.Empty;
        public int Veces { get; set; }
    }

    public class ItemBajoStock
    {
        public string Nombre { get; set; } = string.Empty;
        public decimal CantidadActual { get; set; }
        public decimal StockMinimo { get; set; }
        public string Tipo { get; set; } = string.Empty;
    }

    public class VentaDiaria
    {
        public string Fecha { get; set; } = string.Empty;
        public decimal Total { get; set; }
    }
}