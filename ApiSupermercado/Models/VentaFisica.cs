using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SupermercadoAPI.Models
{
    public class VentaFisica
    {
        [Key]
        public int ID_Venta { get; set; }

        public DateTime FechaHora { get; set; } = DateTime.Now;

        public decimal TotalVenta { get; set; }

        public decimal MontoRecibido { get; set; } = 0;

        public decimal Cambio { get; set; } = 0;

        [MaxLength(10)]
        public string TipoPago { get; set; } = "Efectivo"; // "Efectivo" | "Tarjeta"

        public int ID_Usuario_Cajero { get; set; }

        public int? ID_Caja { get; set; }

        [ForeignKey("ID_Usuario_Cajero")]
        public Usuario? Cajero { get; set; }

        [ForeignKey("ID_Caja")]
        public Caja? Caja { get; set; }

        public ICollection<DetalleVentaFisica> Detalles { get; set; } = new List<DetalleVentaFisica>();
    }
}
