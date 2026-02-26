using System.ComponentModel.DataAnnotations;

namespace SupermercadoAPI.DTOs.Pedidos
{
    public class DetallePedidoCreateDto
    {
        [Required(ErrorMessage = "El producto es obligatorio.")]
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un producto válido.")]
        public int ID_Producto { get; set; }

        [Required(ErrorMessage = "La cantidad es obligatoria.")]
        [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser mayor a 0.")]
        public int CantidadPedida { get; set; }
    }

    public class PedidoCreateDto
    {
        [Required(ErrorMessage = "El pedido debe tener al menos un producto.")]
        [MinLength(1, ErrorMessage = "El pedido debe tener al menos un producto.")]
        public List<DetallePedidoCreateDto> Detalles { get; set; } = new();
    }

    public class PedidoEstadoDto
    {
        [Required(ErrorMessage = "El estado es obligatorio.")]
        [RegularExpression("^(PENDIENTE|LISTO|ENTREGADO|CANCELADO)$",
            ErrorMessage = "Estado no válido. Use: PENDIENTE, LISTO, ENTREGADO o CANCELADO.")]
        public string EstadoPedido { get; set; } = string.Empty;
    }

    public class DetallePedidoResponseDto
    {
        public string NombreProducto { get; set; } = string.Empty;
        public int CantidadPedida { get; set; }
        public decimal PrecioAlMomento { get; set; }
        public decimal Subtotal { get; set; }
    }

    public class PedidoResponseDto
    {
        public int ID_PedidoWeb { get; set; }
        public DateTime FechaHoraPedido { get; set; }
        public decimal TotalPedido { get; set; }
        public string EstadoPedido { get; set; } = string.Empty;
        public string NombreCliente { get; set; } = string.Empty;
        public List<DetallePedidoResponseDto> Detalles { get; set; } = new();
    }
}
