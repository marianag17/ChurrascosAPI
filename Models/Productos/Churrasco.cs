using ChurrascosAPI.Models.Base;
using ChurrascosAPI.Models.Enums;
using ChurrascosAPI.Models.Inventario;
using System.ComponentModel.DataAnnotations;

namespace ChurrascosAPI.Models.Productos
{
    public class Churrasco : Producto
    {
        [Required]
        public TipoCarne TipoCarne { get; set; }
        
        [Required]
        public TerminoCoccion TerminoCoccion { get; set; }
        
        [Required]
        public TipoPlato TipoPlato { get; set; }
        
        [Range(1, 10)]
        public int CantidadPorciones { get; set; } = 1;
        
        public List<GuarnicionChurrasco> Guarniciones { get; set; } = new();
        
        [Range(0.01, double.MaxValue)]
        public decimal PrecioBase { get; set; }
        
        public decimal? TemperaturaRecomendada { get; set; }
        
        public TimeSpan? TiempoPreparacion { get; set; }
        
        [StringLength(200)]
        public string? NotasEspeciales { get; set; }
    }
}