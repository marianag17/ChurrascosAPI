using ChurrascosAPI.Models.Productos;
using System.ComponentModel.DataAnnotations;

namespace ChurrascosAPI.Models.Inventario
{
    public class GuarnicionChurrasco
    {
        public int ChurrascoId { get; set; }
        public Churrasco Churrasco { get; set; } = null!;
        
        public int GuarnicionId { get; set; }
        public Guarnicion Guarnicion { get; set; } = null!;
        
        [Range(1, 10)]
        public int Cantidad { get; set; } = 1;
        
        // Indica si es una porción extra (con costo adicional)
        public bool EsExtra { get; set; } = false;
        
        // Precio específico para esta combinación (puede ser diferente al precio base)
        [Range(0, double.MaxValue)]
        public decimal? PrecioEspecial { get; set; }
        
        // Notas especiales para la preparación
        [StringLength(100)]
        public string? NotasPreparacion { get; set; }
        
        // Orden de preferencia (para mostrar primero las guarniciones más populares)
        public int OrdenPreferencia { get; set; } = 0;
    }
}