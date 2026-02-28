using System.ComponentModel.DataAnnotations;

namespace SupermercadoAPI.DTOs.Ubicaciones
{
    public class UbicacionCreateDto
    {
        [Required(ErrorMessage = "El nombre de la ubicación es obligatorio.")]
        [MaxLength(100, ErrorMessage = "El nombre no puede superar 100 caracteres.")]
        public string NombreUbicacion { get; set; } = string.Empty;
    }

    public class UbicacionResponseDto
    {
        public int    ID_Ubicacion    { get; set; }
        public string NombreUbicacion { get; set; } = string.Empty;
        public int    TotalProductos  { get; set; }
    }
}
