using System.ComponentModel.DataAnnotations;

namespace SupermercadoAPI.DTOs.Auth
{
    public class RegistroDto
    {
        [Required(ErrorMessage = "El nombre completo es obligatorio.")]
        [MaxLength(150, ErrorMessage = "El nombre no puede superar 150 caracteres.")]
        public string NombreCompleto { get; set; } = string.Empty;

        [Required(ErrorMessage = "El correo es obligatorio.")]
        [EmailAddress(ErrorMessage = "El correo no tiene un formato válido.")]
        [MaxLength(100, ErrorMessage = "El correo no puede superar 100 caracteres.")]
        public string Correo_Usuario { get; set; } = string.Empty;

        [Required(ErrorMessage = "La contraseña es obligatoria.")]
        [MinLength(6, ErrorMessage = "La contraseña debe tener al menos 6 caracteres.")]
        public string Contrasena { get; set; } = string.Empty;
    }
}
