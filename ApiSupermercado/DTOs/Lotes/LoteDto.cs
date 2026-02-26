using System.ComponentModel.DataAnnotations;

namespace SupermercadoAPI.DTOs.Lotes
{
    public class LoteCreateDto
    {
        [Required(ErrorMessage = "El código de lote es obligatorio.")]
        [MaxLength(50, ErrorMessage = "El código no puede superar 50 caracteres.")]
        public string CodigoLoteFisico { get; set; } = string.Empty;

        [Required(ErrorMessage = "La fecha de producción es obligatoria.")]
        public DateOnly FechaProduccion { get; set; }

        [Required(ErrorMessage = "La fecha de vencimiento es obligatoria.")]
        public DateOnly FechaVencimiento { get; set; }

        [Required(ErrorMessage = "La cantidad es obligatoria.")]
        [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser mayor a 0.")]
        public int CantidadOriginal { get; set; }

        [Required(ErrorMessage = "El producto es obligatorio.")]
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un producto válido.")]
        public int ID_Producto { get; set; }

        [Required(ErrorMessage = "El proveedor es obligatorio.")]
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un proveedor válido.")]
        public int ID_Proveedor { get; set; }
    }

    public class LoteResponseDto
    {
        public int ID_Lote { get; set; }
        public string CodigoLoteFisico { get; set; } = string.Empty;
        public DateOnly FechaProduccion { get; set; }
        public DateOnly FechaVencimiento { get; set; }
        public DateTime FechaIngreso { get; set; }
        public int CantidadOriginal { get; set; }
        public int UnidadesEnBodega { get; set; }
        public int UnidadesEnEstante { get; set; }
        public int UnidadesVendidas { get; set; }
        public string NombreProducto { get; set; } = string.Empty;
        public string NombreProveedor { get; set; } = string.Empty;
    }
}
