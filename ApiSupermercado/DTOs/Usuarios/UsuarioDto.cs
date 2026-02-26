using System.ComponentModel.DataAnnotations;

namespace SupermercadoAPI.DTOs.Usuarios
{
    public class UsuarioResponseDto
    {
        public int ID_Usuario { get; set; }
        public string NombreCompleto { get; set; } = string.Empty;
        public string Correo_Usuario { get; set; } = string.Empty;
        public bool EstadoActivo { get; set; }
        public string NombreRol { get; set; } = string.Empty;
    }

    public class UsuarioEstadoDto
    {
        [Required(ErrorMessage = "El estado es obligatorio.")]
        public bool EstadoActivo { get; set; }
    }
}
