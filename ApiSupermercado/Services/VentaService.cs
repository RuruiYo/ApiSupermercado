using Microsoft.EntityFrameworkCore;
using SupermercadoAPI.Data;
using SupermercadoAPI.DTOs.Ventas;
using SupermercadoAPI.Models;

namespace SupermercadoAPI.Services
{
    public class VentaService
    {
        private readonly AppDbContext _context;

        public VentaService(AppDbContext context)
        {
            _context = context;
        }

        public List<VentaResponseDto> GetTodas()
        {
            return _context.Ventas_Fisicas
                .Include(v => v.Cajero)
                .Include(v => v.Detalles).ThenInclude(d => d.Producto)
                .OrderByDescending(v => v.FechaHora)
                .Select(v => new VentaResponseDto
                {
                    ID_Venta = v.ID_Venta,
                    FechaHora = v.FechaHora,
                    TotalVenta = v.TotalVenta,
                    EstadoVenta = v.EstadoVenta,
                    NombreCajero = v.Cajero!.NombreCompleto,
                    Detalles = v.Detalles.Select(d => new DetalleVentaResponseDto
                    {
                        NombreProducto = d.Producto!.NombreProducto,
                        CantidadComprada = d.CantidadComprada,
                        PrecioAlMomento = d.PrecioAlMomento,
                        Subtotal = d.CantidadComprada * d.PrecioAlMomento
                    }).ToList()
                }).ToList();
        }

        public (bool Exito, string Mensaje, object? Datos) Registrar(VentaCreateDto dto, int idCajero)
        {
            var ids = dto.Detalles.Select(d => d.ID_Producto).ToList();
            if (ids.Distinct().Count() != ids.Count)
                return (false, "Hay productos duplicados en la venta. Combine las cantidades.", null);

            decimal total = 0;
            var detalles = new List<DetalleVentaFisica>();

            foreach (var item in dto.Detalles)
            {
                var producto = _context.Productos.Find(item.ID_Producto);
                if (producto == null)
                    return (false, $"No se encontró el producto con ID {item.ID_Producto}.", null);

                if (producto.Stock_Estante_Total < item.CantidadComprada)
                    return (false, $"Stock insuficiente para '{producto.NombreProducto}'. Disponible: {producto.Stock_Estante_Total} unidades.", null);

                producto.Stock_Estante_Total -= item.CantidadComprada;

                // FIFO: descontar de lotes más próximos a vencer primero
                int restante = item.CantidadComprada;
                var lotes = _context.Inventario_Lotes
                    .Where(l => l.ID_Producto == item.ID_Producto && l.UnidadesEnEstante > 0)
                    .OrderBy(l => l.FechaVencimiento)
                    .ToList();

                foreach (var lote in lotes)
                {
                    if (restante <= 0) break;
                    int tomado = Math.Min(lote.UnidadesEnEstante, restante);
                    lote.UnidadesEnEstante -= tomado;
                    lote.UnidadesVendidas += tomado;
                    restante -= tomado;
                }

                total += producto.PrecioVenta * item.CantidadComprada;
                detalles.Add(new DetalleVentaFisica
                {
                    ID_Producto = item.ID_Producto,
                    CantidadComprada = item.CantidadComprada,
                    PrecioAlMomento = producto.PrecioVenta
                });
            }

            var venta = new VentaFisica
            {
                TotalVenta = total,
                EstadoVenta = "APROBADA",
                ID_Usuario_Cajero = idCajero,
                Detalles = detalles
            };

            _context.Ventas_Fisicas.Add(venta);
            _context.SaveChanges();

            return (true, "Venta registrada correctamente.", new { venta.ID_Venta, total });
        }
    }
}
