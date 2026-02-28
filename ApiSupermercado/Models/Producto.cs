using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SupermercadoAPI.Models
{
    public class Producto
    {
        [Key]
        public int ID_Producto { get; set; }
        public string SKU_CodigoInterno { get; set; } = string.Empty;
        public string NombreProducto { get; set; } = string.Empty;
        public string? Descripcion { get; set; }
        public decimal PrecioVenta { get; set; }
        public int Stock_Bodega_Total { get; set; } = 0;
        public int Stock_Estante_Total { get; set; } = 0;
        public int Stock_Reservado_Total { get; set; } = 0;
        public string? ImagenUrl { get; set; }

        public int ID_Categoria { get; set; }
        public int ID_Ubicacion { get; set; }

        [ForeignKey("ID_Categoria")]
        public Categoria? Categoria { get; set; }

        [ForeignKey("ID_Ubicacion")]
        public UbicacionBodega? Ubicacion { get; set; }
    }
}
