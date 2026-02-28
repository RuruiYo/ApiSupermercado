using System.ComponentModel.DataAnnotations;

namespace SupermercadoAPI.DTOs.Productos
{
    public class ProductoCreateDto
    {
        [Required(ErrorMessage = "El SKU es obligatorio.")]
        [MaxLength(50)]
        public string SKU_CodigoInterno { get; set; } = string.Empty;

        [Required(ErrorMessage = "El nombre del producto es obligatorio.")]
        [MaxLength(150)]
        public string NombreProducto { get; set; } = string.Empty;

        [MaxLength(255)]
        public string? Descripcion { get; set; }

        [Required(ErrorMessage = "El precio es obligatorio.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El precio debe ser mayor a 0.")]
        public decimal PrecioVenta { get; set; }

        [MaxLength(500)]
        public string? ImagenUrl { get; set; }

        [Required(ErrorMessage = "La categoría es obligatoria.")]
        [Range(1, int.MaxValue, ErrorMessage = "Seleccione una categoría válida.")]
        public int ID_Categoria { get; set; }

        [Required(ErrorMessage = "La ubicación en bodega es obligatoria.")]
        [Range(1, int.MaxValue, ErrorMessage = "Seleccione una ubicación válida.")]
        public int ID_Ubicacion { get; set; }
    }

    public class ProductoUpdateDto
    {
        [Required(ErrorMessage = "El nombre del producto es obligatorio.")]
        [MaxLength(150)]
        public string NombreProducto { get; set; } = string.Empty;

        [MaxLength(255)]
        public string? Descripcion { get; set; }

        [Required(ErrorMessage = "El precio es obligatorio.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El precio debe ser mayor a 0.")]
        public decimal PrecioVenta { get; set; }

        [MaxLength(500)]
        public string? ImagenUrl { get; set; }

        [Required(ErrorMessage = "La categoría es obligatoria.")]
        [Range(1, int.MaxValue, ErrorMessage = "Seleccione una categoría válida.")]
        public int ID_Categoria { get; set; }

        [Required(ErrorMessage = "La ubicación es obligatoria.")]
        [Range(1, int.MaxValue, ErrorMessage = "Seleccione una ubicación válida.")]
        public int ID_Ubicacion { get; set; }
    }

    public class ProductoResponseDto
    {
        public int     ID_Producto          { get; set; }
        public string  SKU_CodigoInterno    { get; set; } = string.Empty;
        public string  NombreProducto       { get; set; } = string.Empty;
        public string? Descripcion          { get; set; }
        public decimal PrecioVenta          { get; set; }
        public int     Stock_Bodega_Total   { get; set; }
        public int     Stock_Estante_Total  { get; set; }
        public int     Stock_Reservado_Total { get; set; }
        public string? ImagenUrl            { get; set; }
        public string  NombreCategoria      { get; set; } = string.Empty;
        public int     ID_Categoria         { get; set; }
        public string  NombreUbicacion      { get; set; } = string.Empty;
        public int     ID_Ubicacion         { get; set; }
    }
}
