using System.ComponentModel.DataAnnotations;

namespace ChurrascosAPI.Models.Ventas
{
    public class Sucursal
    {
        public int Id { get; set; }
        
        [Required]
        [StringLength(100)]
        public string Nombre { get; set; } = string.Empty;
        
        [Required]
        [StringLength(200)]
        public string Direccion { get; set; } = string.Empty;
        
        [Required]
        [StringLength(20)]
        public string Telefono { get; set; } = string.Empty;
        
        [StringLength(100)]
        public string? Email { get; set; }
        
        [StringLength(100)]
        public string? Gerente { get; set; }
        
        public bool Activa { get; set; } = true;
        
        public double? Latitud { get; set; }
        
        public double? Longitud { get; set; }
        
        [StringLength(5)]
        public string? HorarioApertura { get; set; }
        
        [StringLength(5)]
        public string? HorarioCierre { get; set; }
        
        public List<string>? DiasLaborales { get; set; }
        
        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;
        
        public DateTime? FechaModificacion { get; set; }
        
    }
}

