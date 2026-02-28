using System.ComponentModel.DataAnnotations;

namespace SupermercadoAPI.DTOs.Proveedores
{
    public class ProveedorCreateDto
    {
        [Required(ErrorMessage = "El nombre de la empresa es obligatorio.")]
        [MaxLength(150, ErrorMessage = "El nombre no puede superar 150 caracteres.")]
        public string NombreEmpresa { get; set; } = string.Empty;

        [MaxLength(100)]
        public string? ContactoAsignado { get; set; }

        [MaxLength(20)]
        public string? Telefono { get; set; }
    }

    public class ProveedorResponseDto
    {
        public int     ID_Proveedor      { get; set; }
        public string  NombreEmpresa     { get; set; } = string.Empty;
        public string? ContactoAsignado  { get; set; }
        public string? Telefono          { get; set; }
        public bool    EstadoActivo      { get; set; }
    }

    public class ProveedorEstadoDto
    {
        public bool EstadoActivo { get; set; }
    }
}
