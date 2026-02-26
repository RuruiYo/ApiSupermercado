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
        public string EstadoVenta { get; set; } = "APROBADA";
        public int ID_Usuario_Cajero { get; set; }

        [ForeignKey("ID_Usuario_Cajero")]
        public Usuario? Cajero { get; set; }

        public ICollection<DetalleVentaFisica> Detalles { get; set; } = new List<DetalleVentaFisica>();
    }
}
