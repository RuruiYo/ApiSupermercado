using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SupermercadoAPI.Models
{
    public class Caja
    {
        [Key]
        public int ID_Caja { get; set; }

        [Required]
        [MaxLength(50)]
        public string NombreCaja { get; set; } = string.Empty;

        public int? ID_Usuario_Cajero { get; set; }

        public decimal SaldoInicial { get; set; } = 0;

        public bool EstadoActiva { get; set; } = true;

        [ForeignKey("ID_Usuario_Cajero")]
        public Usuario? Cajero { get; set; }

        public ICollection<VentaFisica> Ventas { get; set; } = new List<VentaFisica>();
    }
}
