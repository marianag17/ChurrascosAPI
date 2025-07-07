using ChurrascosAPI.Models.Base;
using System.ComponentModel.DataAnnotations;

namespace ChurrascosAPI.Models.Ventas
{
    public class VentaItem
    {
        public int Id { get; set; }
        
        public int VentaId { get; set; }
        public Venta Venta { get; set; } = null!;
        
        public int ProductoId { get; set; }
        public Producto Producto { get; set; } = null!;
        
        [Range(1, int.MaxValue)]
        public int Cantidad { get; set; }
        
        [Range(0, double.MaxValue)]
        public decimal PrecioUnitario { get; set; }
        
        [Range(0, double.MaxValue)]
        public decimal Subtotal { get; set; }
        
        [Range(0, 100)]
        public decimal PorcentajeDescuento { get; set; } = 0;
        
        [Range(0, double.MaxValue)]
        public decimal MontoDescuento { get; set; } = 0;
        
        [StringLength(200)]
        public string? NotasEspeciales { get; set; }
        
        [StringLength(50)]
        public string? TerminoCoccionSolicitado { get; set; }
        
        [StringLength(200)]
        public string? GuarnicionesSeleccionadas { get; set; }
        
        public EstadoItemVenta EstadoItem { get; set; } = EstadoItemVenta.Pendiente;
        
        public DateTime? HoraInicioPreparacion { get; set; }
        public DateTime? HoraFinPreparacion { get; set; }
        
        [Range(0, 60)]
        public int? TiempoPreparacionMinutos { get; set; }
        
        [Range(-10, 100)]
        public decimal? TemperaturaServicio { get; set; }
        
        [Range(0, 5000)]
        public int? CaloriasCalculadas { get; set; }
        
        [Range(0, double.MaxValue)]
        public decimal? CostoUnitario { get; set; }
        
        public decimal MargenGanancia => CostoUnitario.HasValue && CostoUnitario > 0 
            ? ((PrecioUnitario - CostoUnitario.Value) / PrecioUnitario) * 100 
            : 0;
    }
    
    public enum EstadoItemVenta
    {
        Pendiente = 0,
        EnPreparacion = 1,
        Listo = 2,
        Servido = 3,
        Cancelado = 4
    }
}