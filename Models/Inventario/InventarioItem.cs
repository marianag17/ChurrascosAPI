using ChurrascosAPI.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace ChurrascosAPI.Models.Inventario
{
    public class InventarioItem
    {
        public int Id { get; set; }
        
        [Required]
        [StringLength(100)]
        public string Nombre { get; set; } = string.Empty;
        
        [Required]
        public TipoInventario Tipo { get; set; }
        
        [Range(0, double.MaxValue)]
        public decimal Cantidad { get; set; }
        
        [Required]
        [StringLength(20)]
        public string Unidad { get; set; } = string.Empty;
        
        [Range(0, double.MaxValue)]
        public decimal StockMinimo { get; set; }
        
        [Range(0, double.MaxValue)]
        public decimal StockMaximo { get; set; }
        
        [Range(0, double.MaxValue)]
        public decimal PrecioUnitario { get; set; }
        
        public DateTime UltimaActualizacion { get; set; } = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        
        [StringLength(100)]
        public string? Proveedor { get; set; }
        
        [StringLength(50)]
        public string? CodigoProveedor { get; set; }
        
        public DateTime? FechaVencimiento { get; set; }
        
        [StringLength(50)]
        public string? UbicacionAlmacen { get; set; }
        
        [Range(0, double.MaxValue)]
        public decimal CostoPromedio { get; set; }
        
        [Range(0, double.MaxValue)]
        public decimal PuntoReorden { get; set; }
        
        public List<MovimientoInventario> Movimientos { get; set; } = new();
        
        public bool Activo { get; set; } = true;
        

        [StringLength(300)]
        public string? Notas { get; set; }
    }
    

    public class MovimientoInventario
    {
        public int Id { get; set; }
        public int InventarioItemId { get; set; }
        public InventarioItem InventarioItem { get; set; } = null!;
        
        [Required]
        public TipoMovimiento TipoMovimiento { get; set; }
        
        [Range(0, double.MaxValue)]
        public decimal Cantidad { get; set; }
        
        [Range(0, double.MaxValue)]
        public decimal CantidadAnterior { get; set; }
        
        [Range(0, double.MaxValue)]
        public decimal CantidadNueva { get; set; }
        
        public DateTime Fecha { get; set; } = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        
        [StringLength(200)]
        public string? Motivo { get; set; }
        
        [StringLength(50)]
        public string? UsuarioResponsable { get; set; }
        
        public int? ReferenciaId { get; set; }
        
        [StringLength(20)]
        public string? TipoReferencia { get; set; } 
    }
    
    public enum TipoMovimiento
    {
        Entrada = 0,
        Salida = 1,
        Ajuste = 2,
        Merma = 3,
        Transferencia = 4
    }
}