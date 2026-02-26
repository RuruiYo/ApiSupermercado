using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SupermercadoAPI.Models
{
    public class HistorialMovimiento
    {
        [Key]
        public int ID_Movimiento { get; set; }
        public DateTime FechaHora { get; set; } = DateTime.Now;
        public string TipoMovimiento { get; set; } = string.Empty;
        public int CantidadMovida { get; set; }
        public string? Observaciones { get; set; }
        public int ID_Producto { get; set; }
        public int? ID_Lote_Afectado { get; set; }
        public int ID_Usuario_Responsable { get; set; }

        [ForeignKey("ID_Producto")]
        public Producto? Producto { get; set; }

        [ForeignKey("ID_Lote_Afectado")]
        public InventarioLote? LoteAfectado { get; set; }

        [ForeignKey("ID_Usuario_Responsable")]
        public Usuario? UsuarioResponsable { get; set; }
    }
}
