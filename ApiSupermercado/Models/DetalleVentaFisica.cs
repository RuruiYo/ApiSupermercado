using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SupermercadoAPI.Models
{
    public class DetalleVentaFisica
    {
        [Key]
        public int ID_DetalleVenta { get; set; }
        public int CantidadComprada { get; set; }
        public decimal PrecioAlMomento { get; set; }
        public int ID_Venta { get; set; }
        public int ID_Producto { get; set; }

        [ForeignKey("ID_Venta")]
        public VentaFisica? Venta { get; set; }

        [ForeignKey("ID_Producto")]
        public Producto? Producto { get; set; }
    }
}
