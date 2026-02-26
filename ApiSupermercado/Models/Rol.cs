using System.ComponentModel.DataAnnotations;

namespace SupermercadoAPI.Models
{
    public class Rol
    {
        [Key]
        public int ID_Rol { get; set; }
        public string NombreRol { get; set; } = string.Empty;

        public ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
    }
}
