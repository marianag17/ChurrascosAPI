using System.ComponentModel.DataAnnotations;

namespace ChurrascosAPI.Models.Base
{
    public abstract class Producto
    {
        public int Id { get; set; }
        
        [Required]
        [StringLength(100)]
        public string Nombre { get; set; } = string.Empty;
        
        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "El precio debe ser mayor a 0")]
        public decimal Precio { get; set; }
        
        [StringLength(500)]
        public string Descripcion { get; set; } = string.Empty;
        
        public bool Disponible { get; set; } = true;
        
        public DateTime FechaCreacion { get; set; } = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        
        public DateTime? FechaModificacion { get; set; }
    }
}