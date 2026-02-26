using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SupermercadoAPI.Models
{
    public class PedidoWebPickup
    {
        [Key]
        public int ID_PedidoWeb { get; set; }
        public DateTime FechaHoraPedido { get; set; } = DateTime.Now;
        public decimal TotalPedido { get; set; }
        public string EstadoPedido { get; set; } = "PENDIENTE";
        public int ID_Usuario_Cliente { get; set; }
        public int? ID_Usuario_Cajero_Atendio { get; set; }

        [ForeignKey("ID_Usuario_Cliente")]
        public Usuario? Cliente { get; set; }

        [ForeignKey("ID_Usuario_Cajero_Atendio")]
        public Usuario? CajeroAtendio { get; set; }

        public ICollection<DetallePedidoWeb> Detalles { get; set; } = new List<DetallePedidoWeb>();
    }
}
