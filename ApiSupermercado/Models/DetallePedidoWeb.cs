using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SupermercadoAPI.Models
{
    public class DetallePedidoWeb
    {
        [Key]
        public int ID_DetallePedido { get; set; }
        public int CantidadPedida { get; set; }
        public decimal PrecioAlMomento { get; set; }
        public int ID_PedidoWeb { get; set; }
        public int ID_Producto { get; set; }

        [ForeignKey("ID_PedidoWeb")]
        public PedidoWebPickup? PedidoWeb { get; set; }

        [ForeignKey("ID_Producto")]
        public Producto? Producto { get; set; }
    }
}
