using ChurrascosAPI.Models.Base;
using ChurrascosAPI.Models.Enums;
using ChurrascosAPI.Models.Ventas;
using System.ComponentModel.DataAnnotations;

namespace ChurrascosAPI.Models.Productos
{
    public class Combo : Producto
    {
        [Required]
        public TipoCombo TipoCombo { get; set; }
        
        public List<ComboItem> Items { get; set; } = new();
        
        [Range(0, 100)]
        public decimal PorcentajeDescuento { get; set; }
        
        [Range(0, double.MaxValue)]
        public decimal MontoDescuento { get; set; }
        
        public bool EsTemporada { get; set; } = false;
        
        public DateTime? FechaInicioVigencia { get; set; }
        
        public DateTime? FechaFinVigencia { get; set; }
        
        [Range(1, int.MaxValue)]
        public int CantidadMinimaPedido { get; set; } = 1;
        
        [Range(1, int.MaxValue)]
        public int? CantidadMaximaPedido { get; set; }

        [StringLength(200)]
        public string? CondicionesEspeciales { get; set; }
        

        public decimal PrecioSinDescuento => Items?.Sum(i => i.PrecioUnitario * i.Cantidad) ?? 0;

        public decimal PrecioFinal => PrecioSinDescuento - MontoDescuento - (PrecioSinDescuento * PorcentajeDescuento / 100);
    }
}