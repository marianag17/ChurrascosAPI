using ChurrascosAPI.Models.Base;
using ChurrascosAPI.Models.Productos;
using System.ComponentModel.DataAnnotations;

namespace ChurrascosAPI.Models.Ventas
{
    public class ComboItem
    {
        public int Id { get; set; }
        
        public int ComboId { get; set; }
        public Combo Combo { get; set; } = null!;
        
        public int ProductoId { get; set; }
        public Producto Producto { get; set; } = null!;
        
        [Range(1, int.MaxValue)]
        public int Cantidad { get; set; }
        
        [Range(0, double.MaxValue)]
        public decimal PrecioUnitario { get; set; }
        
        [Range(0, double.MaxValue)]
        public decimal PrecioTotal => PrecioUnitario * Cantidad;

        public bool EsObligatorio { get; set; } = true;

        public bool PermiteAlternativas { get; set; } = false;
        

        public int OrdenPresentacion { get; set; } = 0;
        
        [StringLength(50)]
        public string? CategoriaCombo { get; set; }
        
        [StringLength(200)]
        public string? DescripcionEnCombo { get; set; }
        
        [StringLength(300)]
        public string? OpcionesPersonalizacion { get; set; }
        
        [Range(0, 100)]
        public decimal PorcentajeDescuentoItem { get; set; } = 0;
        
        [Range(0, double.MaxValue)]
        public decimal PrecioOriginal { get; set; }
        
        public decimal AhorroGenerado => (PrecioOriginal * Cantidad) - PrecioTotal;
    }
}