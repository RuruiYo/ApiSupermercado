using Microsoft.EntityFrameworkCore;
using SupermercadoAPI.Data;
using SupermercadoAPI.DTOs.Pedidos;
using SupermercadoAPI.Models;

namespace SupermercadoAPI.Services
{
    public class PedidoService
    {
        private readonly AppDbContext _context;

        public PedidoService(AppDbContext context)
        {
            _context = context;
        }

        public List<PedidoResponseDto> GetTodos()
        {
            return _context.Pedidos_Web_Pickup
                .Include(p => p.Cliente)
                .Include(p => p.Detalles).ThenInclude(d => d.Producto)
                .OrderByDescending(p => p.FechaHoraPedido)
                .Select(p => MapToDto(p)).ToList();
        }

        public List<PedidoResponseDto> GetMisPedidos(int idCliente)
        {
            return _context.Pedidos_Web_Pickup
                .Include(p => p.Cliente)
                .Include(p => p.Detalles).ThenInclude(d => d.Producto)
                .Where(p => p.ID_Usuario_Cliente == idCliente)
                .OrderByDescending(p => p.FechaHoraPedido)
                .Select(p => MapToDto(p)).ToList();
        }

        public (bool Exito, string Mensaje, object? Datos) Crear(PedidoCreateDto dto, int idCliente)
        {
            var ids = dto.Detalles.Select(d => d.ID_Producto).ToList();
            if (ids.Distinct().Count() != ids.Count)
                return (false, "Hay productos duplicados en el pedido. Combine las cantidades.", null);

            decimal total = 0;
            var detalles = new List<DetallePedidoWeb>();

            foreach (var item in dto.Detalles)
            {
                var producto = _context.Productos.Find(item.ID_Producto);
                if (producto == null)
                    return (false, $"No se encontró el producto con ID {item.ID_Producto}.", null);

                if (producto.Stock_Estante_Total < item.CantidadPedida)
                    return (false, $"Stock insuficiente para '{producto.NombreProducto}'. Disponible: {producto.Stock_Estante_Total} unidades.", null);

                producto.Stock_Estante_Total -= item.CantidadPedida;
                producto.Stock_Reservado_Total += item.CantidadPedida;

                total += producto.PrecioVenta * item.CantidadPedida;
                detalles.Add(new DetallePedidoWeb
                {
                    ID_Producto = item.ID_Producto,
                    CantidadPedida = item.CantidadPedida,
                    PrecioAlMomento = producto.PrecioVenta
                });
            }

            var pedido = new PedidoWebPickup
            {
                TotalPedido = total,
                EstadoPedido = "PENDIENTE",
                ID_Usuario_Cliente = idCliente,
                Detalles = detalles
            };

            _context.Pedidos_Web_Pickup.Add(pedido);
            _context.SaveChanges();

            return (true, "Pedido creado correctamente. Puede retirarlo en tienda.",
                new { pedido.ID_PedidoWeb, total });
        }

        public (bool Exito, string Mensaje) CambiarEstado(int id, string nuevoEstado, int idCajero)
        {
            var pedido = _context.Pedidos_Web_Pickup
                .Include(p => p.Detalles)
                .FirstOrDefault(p => p.ID_PedidoWeb == id);

            if (pedido == null)
                return (false, $"No se encontró el pedido con ID {id}.");

            if (pedido.EstadoPedido == "ENTREGADO" || pedido.EstadoPedido == "CANCELADO")
                return (false, $"No se puede modificar un pedido en estado '{pedido.EstadoPedido}'.");

            if (nuevoEstado == "ENTREGADO")
            {
                foreach (var detalle in pedido.Detalles)
                {
                    var producto = _context.Productos.Find(detalle.ID_Producto);
                    if (producto != null)
                        producto.Stock_Reservado_Total -= detalle.CantidadPedida;
                }
            }

            if (nuevoEstado == "CANCELADO")
            {
                foreach (var detalle in pedido.Detalles)
                {
                    var producto = _context.Productos.Find(detalle.ID_Producto);
                    if (producto != null)
                    {
                        producto.Stock_Reservado_Total -= detalle.CantidadPedida;
                        producto.Stock_Estante_Total += detalle.CantidadPedida;
                    }
                }
            }

            pedido.EstadoPedido = nuevoEstado;
            pedido.ID_Usuario_Cajero_Atendio = idCajero;
            _context.SaveChanges();

            return (true, $"Pedido actualizado a estado '{nuevoEstado}' correctamente.");
        }

        private static PedidoResponseDto MapToDto(PedidoWebPickup p) => new PedidoResponseDto
        {
            ID_PedidoWeb = p.ID_PedidoWeb,
            FechaHoraPedido = p.FechaHoraPedido,
            TotalPedido = p.TotalPedido,
            EstadoPedido = p.EstadoPedido,
            NombreCliente = p.Cliente!.NombreCompleto,
            Detalles = p.Detalles.Select(d => new DetallePedidoResponseDto
            {
                NombreProducto = d.Producto!.NombreProducto,
                CantidadPedida = d.CantidadPedida,
                PrecioAlMomento = d.PrecioAlMomento,
                Subtotal = d.CantidadPedida * d.PrecioAlMomento
            }).ToList()
        };


    }
}
