using System.ComponentModel.DataAnnotations;

namespace SupermercadoAPI.DTOs.Movimientos
{
    public class TrasladoDto
    {
        [Required(ErrorMessage = "El lote es obligatorio.")]
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un lote válido.")]
        public int ID_Lote { get; set; }

        [Required(ErrorMessage = "La cantidad es obligatoria.")]
        [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser mayor a 0.")]
        public int Cantidad { get; set; }

        [MaxLength(255, ErrorMessage = "Las observaciones no pueden superar 255 caracteres.")]
        public string? Observaciones { get; set; }
    }

    public class DescartarDto
    {
        [Required(ErrorMessage = "El lote es obligatorio.")]
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un lote válido.")]
        public int ID_Lote { get; set; }

        [Required(ErrorMessage = "La cantidad es obligatoria.")]
        [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser mayor a 0.")]
        public int Cantidad { get; set; }

        [MaxLength(255, ErrorMessage = "Las observaciones no pueden superar 255 caracteres.")]
        public string? Observaciones { get; set; }
    }

    public class MovimientoResponseDto
    {
        public int ID_Movimiento { get; set; }
        public DateTime FechaHora { get; set; }
        public string TipoMovimiento { get; set; } = string.Empty;
        public int CantidadMovida { get; set; }
        public string? Observaciones { get; set; }
        public string NombreProducto { get; set; } = string.Empty;
        public string CodigoLote { get; set; } = string.Empty;
        public string NombreUsuario { get; set; } = string.Empty;
    }
}
