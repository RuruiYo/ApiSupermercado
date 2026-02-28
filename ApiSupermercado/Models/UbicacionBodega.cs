using System.ComponentModel.DataAnnotations;

namespace SupermercadoAPI.Models
{
    public class UbicacionBodega
    {
        [Key]
        public int ID_Ubicacion { get; set; }

        [Required]
        [MaxLength(100)]
        public string NombreUbicacion { get; set; } = string.Empty;

        public ICollection<Producto> Productos { get; set; } = new List<Producto>();
    }
}
