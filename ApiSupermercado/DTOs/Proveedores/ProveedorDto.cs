using System.ComponentModel.DataAnnotations;

namespace SupermercadoAPI.DTOs.Proveedores
{
    public class ProveedorResponseDto
    {
        public int ID_Proveedor { get; set; }
        public string NombreEmpresa { get; set; } = string.Empty;
        public string? ContactoAsignado { get; set; }
        public string? Telefono { get; set; }
        public bool EstadoActivo { get; set; }
    }

    public class ProveedorEstadoDto
    {
        [Required(ErrorMessage = "El estado es obligatorio.")]
        public bool EstadoActivo { get; set; }
    }
}
