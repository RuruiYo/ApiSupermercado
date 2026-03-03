using System.ComponentModel.DataAnnotations;

namespace SupermercadoAPI.DTOs.Ventas
{
    public class VentaCreateDto
    {
        [Required(ErrorMessage = "Debe especificar la caja.")]
        [Range(1, int.MaxValue, ErrorMessage = "Seleccione una caja válida.")]
        public int ID_Caja { get; set; }

        [Required(ErrorMessage = "El tipo de pago es obligatorio.")]
        [RegularExpression("Efectivo|Tarjeta", ErrorMessage = "TipoPago debe ser 'Efectivo' o 'Tarjeta'.")]
        public string TipoPago { get; set; } = "Efectivo";

        [Range(0, double.MaxValue, ErrorMessage = "El monto recibido no puede ser negativo.")]
        public decimal MontoRecibido { get; set; } = 0;

        [Required]
        [MinLength(1, ErrorMessage = "La venta debe tener al menos un producto.")]
        public List<DetalleVentaDto> Detalles { get; set; } = new();
    }

    public class DetalleVentaDto
    {
        [Range(1, int.MaxValue)]
        public int ID_Producto { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser mayor a 0.")]
        public int Cantidad { get; set; }
    }

    public class VentaResponseDto
    {
        public int      ID_Venta       { get; set; }
        public DateTime FechaHora     { get; set; }
        public decimal  TotalVenta     { get; set; }
        public decimal  MontoRecibido  { get; set; }
        public decimal  Cambio         { get; set; }
        public string   TipoPago       { get; set; } = string.Empty;
        public string   NombreCajero   { get; set; } = string.Empty;
        public string   NombreCaja     { get; set; } = string.Empty;
        public List<DetalleVentaResponseDto> Detalles { get; set; } = new();
    }

    public class DetalleVentaResponseDto
    {
        public string  NombreProducto { get; set; } = string.Empty;
        public int     Cantidad       { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal Subtotal       { get; set; }
    }
}
