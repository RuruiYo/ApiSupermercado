using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SupermercadoAPI.Models
{
    public class InventarioLote
    {
        [Key]
        public int ID_Lote { get; set; }
        public string CodigoLoteFisico { get; set; } = string.Empty;
        public DateOnly FechaProduccion { get; set; }
        public DateOnly FechaVencimiento { get; set; }
        public DateTime FechaIngreso { get; set; } = DateTime.Now;
        public int CantidadOriginal { get; set; }
        public int UnidadesEnBodega { get; set; } = 0;
        public int UnidadesEnEstante { get; set; } = 0;
        public int UnidadesVendidas { get; set; } = 0;
        public int ID_Producto { get; set; }
        public int ID_Proveedor { get; set; }
        public int ID_Usuario_Recibio { get; set; }

        [ForeignKey("ID_Producto")]
        public Producto? Producto { get; set; }

        [ForeignKey("ID_Proveedor")]
        public Proveedor? Proveedor { get; set; }

        [ForeignKey("ID_Usuario_Recibio")]
        public Usuario? UsuarioRecibio { get; set; }
    }
}
