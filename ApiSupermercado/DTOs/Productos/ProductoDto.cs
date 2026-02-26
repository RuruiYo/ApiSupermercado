using System.ComponentModel.DataAnnotations;

namespace SupermercadoAPI.DTOs.Productos
{
    public class ProductoCreateDto
    {
        [Required(ErrorMessage = "El SKU es obligatorio.")]
        [MaxLength(50, ErrorMessage = "El SKU no puede superar 50 caracteres.")]
        public string SKU_CodigoInterno { get; set; } = string.Empty;

        [Required(ErrorMessage = "El nombre del producto es obligatorio.")]
        [MaxLength(150, ErrorMessage = "El nombre no puede superar 150 caracteres.")]
        public string NombreProducto { get; set; } = string.Empty;

        [MaxLength(255, ErrorMessage = "La descripción no puede superar 255 caracteres.")]
        public string? Descripcion { get; set; }

        [MaxLength(100)]
        public string UbicacionBodega { get; set; } = "Pasillo N/A";

        [Required(ErrorMessage = "El precio es obligatorio.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El precio debe ser mayor a 0.")]
        public decimal PrecioVenta { get; set; }

        [MaxLength(500, ErrorMessage = "La URL de imagen no puede superar 500 caracteres.")]
        public string? ImagenUrl { get; set; }

        [Required(ErrorMessage = "La categoría es obligatoria.")]
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar una categoría válida.")]
        public int ID_Categoria { get; set; }
    }

    public class ProductoUpdateDto
    {
        [Required(ErrorMessage = "El nombre del producto es obligatorio.")]
        [MaxLength(150, ErrorMessage = "El nombre no puede superar 150 caracteres.")]
        public string NombreProducto { get; set; } = string.Empty;

        [MaxLength(255, ErrorMessage = "La descripción no puede superar 255 caracteres.")]
        public string? Descripcion { get; set; }

        [MaxLength(100)]
        public string UbicacionBodega { get; set; } = "Pasillo N/A";

        [Required(ErrorMessage = "El precio es obligatorio.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El precio debe ser mayor a 0.")]
        public decimal PrecioVenta { get; set; }

        [MaxLength(500, ErrorMessage = "La URL de imagen no puede superar 500 caracteres.")]
        public string? ImagenUrl { get; set; }

        [Required(ErrorMessage = "La categoría es obligatoria.")]
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar una categoría válida.")]
        public int ID_Categoria { get; set; }
    }

    public class ProductoResponseDto
    {
        public int ID_Producto { get; set; }
        public string SKU_CodigoInterno { get; set; } = string.Empty;
        public string NombreProducto { get; set; } = string.Empty;
        public string? Descripcion { get; set; }
        public string UbicacionBodega { get; set; } = string.Empty;
        public decimal PrecioVenta { get; set; }
        public int Stock_Bodega_Total { get; set; }
        public int Stock_Estante_Total { get; set; }
        public int Stock_Reservado_Total { get; set; }
        public string? ImagenUrl { get; set; }
        public string NombreCategoria { get; set; } = string.Empty;
        public int ID_Categoria { get; set; }
    }
}
