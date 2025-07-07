using ChurrascosAPI.Models.Base;
using ChurrascosAPI.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace ChurrascosAPI.Models.Productos
{
    public class DulceTipico : Producto
    {
        [Required]
        public TipoDulce TipoDulce { get; set; }
        
        [Range(0, int.MaxValue)]
        public int CantidadEnStock { get; set; }
        
        [Required]
        public ModalidadVenta ModalidadVenta { get; set; }
        
        [Range(1, 24)]
        public int? CapacidadCaja { get; set; }

        public decimal PrecioUnidad { get; set; }
        
        public DateTime? FechaVencimiento { get; set; }
        
        [StringLength(100)]
        public string? Proveedor { get; set; }

        [StringLength(300)]
        public string? Ingredientes { get; set; }

        public decimal? PesoGramos { get; set; }
    }
}