using System.ComponentModel.DataAnnotations;

namespace ChurrascosAPI.Models.Inventario
{
    public class Guarnicion
    {
        public int Id { get; set; }
        
        [Required]
        [StringLength(50)]
        public string Nombre { get; set; } = string.Empty;
        
        [Range(0, double.MaxValue)]
        public decimal PrecioExtra { get; set; } = 0;
        
        public bool Disponible { get; set; } = true;
        
        [Range(0, int.MaxValue)]
        public int CantidadStock { get; set; }
        
        [Range(0, int.MaxValue)]
        public int StockMinimo { get; set; } = 5;
        
        [StringLength(200)]
        public string? Descripcion { get; set; }
        
        // Tiempo de preparación en minutos
        [Range(0, 60)]
        public int TiempoPreparacionMinutos { get; set; } = 5;
        
        // Calorias por porción
        [Range(0, 1000)]
        public int? CaloriasPorPorcion { get; set; }
        
        // Indica si es una guarnición premium (costo extra)
        public bool EsPremium { get; set; } = false;
        
        // Unidad de medida (porciones, gramos, etc.)
        [StringLength(20)]
        public string UnidadMedida { get; set; } = "porción";
        
        public DateTime FechaCreacion { get; set; } = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        
        public DateTime? FechaModificacion { get; set; }
    }
}