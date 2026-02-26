using System.ComponentModel.DataAnnotations;

namespace SupermercadoAPI.Models
{
    public class Categoria
    {
        [Key]
        public int ID_Categoria { get; set; }
        public string NombreCategoria { get; set; } = string.Empty;

        public ICollection<Producto> Productos { get; set; } = new List<Producto>();
    }
}
