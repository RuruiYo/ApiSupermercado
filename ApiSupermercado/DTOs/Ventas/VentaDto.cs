using System.ComponentModel.DataAnnotations;

namespace SupermercadoAPI.DTOs.Ventas
{
    public class DetalleVentaCreateDto
    {
        [Required(ErrorMessage = "El producto es obligatorio.")]
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un producto válido.")]
        public int ID_Producto { get; set; }

        [Required(ErrorMessage = "La cantidad es obligatoria.")]
        [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser mayor a 0.")]
        public int CantidadComprada { get; set; }
    }

    public class VentaCreateDto
    {
        [Required(ErrorMessage = "La venta debe tener al menos un producto.")]
        [MinLength(1, ErrorMessage = "La venta debe tener al menos un producto.")]
        public List<DetalleVentaCreateDto> Detalles { get; set; } = new();
    }

    public class DetalleVentaResponseDto
    {
        public string NombreProducto { get; set; } = string.Empty;
        public int CantidadComprada { get; set; }
        public decimal PrecioAlMomento { get; set; }
        public decimal Subtotal { get; set; }
    }

    public class VentaResponseDto
    {
        public int ID_Venta { get; set; }
        public DateTime FechaHora { get; set; }
        public decimal TotalVenta { get; set; }
        public string EstadoVenta { get; set; } = string.Empty;
        public string NombreCajero { get; set; } = string.Empty;
        public List<DetalleVentaResponseDto> Detalles { get; set; } = new();
    }
}
