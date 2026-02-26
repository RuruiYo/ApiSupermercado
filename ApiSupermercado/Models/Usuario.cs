using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SupermercadoAPI.Models
{
    public class Usuario
    {
        [Key]
        public int ID_Usuario { get; set; }
        public string NombreCompleto { get; set; } = string.Empty;
        public string Correo_Usuario { get; set; } = string.Empty;
        public string Contrasena { get; set; } = string.Empty;
        public bool EstadoActivo { get; set; } = true;
        public int ID_Rol { get; set; }

        [ForeignKey("ID_Rol")]
        public Rol? Rol { get; set; }
    }
}
