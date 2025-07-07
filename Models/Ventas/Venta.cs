using ChurrascosAPI.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace ChurrascosAPI.Models.Ventas
{
    public class Venta
    {
        public int Id { get; set; }
        
        public DateTime Fecha { get; set; } = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        
        public List<VentaItem> Items { get; set; } = new();
        
        [Range(0, double.MaxValue)]
        public decimal Subtotal { get; set; }
        
        [Range(0, double.MaxValue)]
        public decimal Impuestos { get; set; }
        
        [Range(0, double.MaxValue)]
        public decimal Descuento { get; set; }
        
        [Range(0, double.MaxValue)]
        public decimal Total { get; set; }
        
        [Required]
        public TipoVenta TipoVenta { get; set; }
        
        [Required]
        public EstadoVenta Estado { get; set; } = EstadoVenta.Pendiente;
        
        [StringLength(300)]
        public string? NotasEspeciales { get; set; }
        
        [StringLength(100)]
        public string? NombreCliente { get; set; }
        
        [StringLength(20)]
        public string? TelefonoCliente { get; set; }
        
        [StringLength(200)]
        public string? DireccionEntrega { get; set; }
        
        public int? NumeroMesa { get; set; }
        
        [StringLength(50)]
        public string? AreaRestaurante { get; set; } // 
        
        public DateTime? HoraInicioPedido { get; set; }
        public DateTime? HoraInicioPreparacion { get; set; }
        public DateTime? HoraFinalizacionPreparacion { get; set; }
        public DateTime? HoraEntrega { get; set; }

        [StringLength(30)]
        public string? MetodoPago { get; set; }
        

        [Range(0, double.MaxValue)]
        public decimal Propina { get; set; }
        
        [StringLength(50)]
        public string? Mesero { get; set; }
        
        [StringLength(50)]
        public string? Cocinero { get; set; }
        
        [StringLength(50)]
        public string? Repartidor { get; set; }
        
        public DateTime? FechaEstimadaEntrega { get; set; }
        
        [Range(0, double.MaxValue)]
        public decimal? CostoEnvio { get; set; }
        
        [Range(1, 5)]
        public int? CalificacionServicio { get; set; }
        
        [StringLength(300)]
        public string? ComentariosCliente { get; set; }
        
        public DateTime FechaCreacion { get; set; } = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        public DateTime? FechaModificacion { get; set; }
        
        [StringLength(50)]
        public string? UsuarioCreacion { get; set; }
        
        [StringLength(50)]
        public string? UsuarioModificacion { get; set; }
        
        [StringLength(20)]
        public string NumeroOrden { get; set; } = string.Empty;
    }
}