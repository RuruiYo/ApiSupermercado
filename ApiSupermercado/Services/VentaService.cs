// ARCHIVO CORREGIDO - reemplaza ApiSupermercado/Services/VentaService.cs
using Microsoft.EntityFrameworkCore;
using SupermercadoAPI.Data;
using SupermercadoAPI.DTOs.Ventas;
using SupermercadoAPI.Models;

namespace SupermercadoAPI.Services
{
    public class VentaService
    {
        private readonly AppDbContext _context;
        public VentaService(AppDbContext context) { _context = context; }

        public List<VentaResponseDto> GetTodas()
        {
            return _context.Ventas_Fisicas
                .Include(v => v.Cajero)
                .Include(v => v.Caja)
                .Include(v => v.Detalles).ThenInclude(d => d.Producto)
                .OrderByDescending(v => v.FechaHora)
                .Select(v => ToDto(v))
                .ToList();
        }

        public List<VentaResponseDto> GetPorCajero(int idCajero)
        {
            return _context.Ventas_Fisicas
                .Include(v => v.Cajero)
                .Include(v => v.Caja)
                .Include(v => v.Detalles).ThenInclude(d => d.Producto)
                .Where(v => v.ID_Usuario_Cajero == idCajero)
                .OrderByDescending(v => v.FechaHora)
                .Select(v => ToDto(v))
                .ToList();
        }

        public (bool Exito, string Mensaje, VentaResponseDto? Datos) Registrar(
            VentaCreateDto dto, int idCajero)
        {
            // Verificar caja activa asignada al cajero
            var caja = _context.Cajas.Find(dto.ID_Caja);
            if (caja == null)
                return (false, "No se encontró la caja especificada.", null);
            if (!caja.EstadoActiva)
                return (false, "La caja no está activa.", null);
            if (caja.ID_Usuario_Cajero != idCajero)
                return (false, "Esta caja no le está asignada.", null);

            // Calcular total y verificar stock en estante
            decimal total = 0;
            var lineas = new List<(Producto prod, int cantidad)>();

            foreach (var det in dto.Detalles)
            {
                var prod = _context.Productos.Find(det.ID_Producto);
                if (prod == null)
                    return (false, $"No se encontró el producto con ID {det.ID_Producto}.", null);
                if (prod.Stock_Estante_Total < det.Cantidad)
                    return (false,
                        $"Stock insuficiente en estante para '{prod.NombreProducto}'. " +
                        $"Disponible: {prod.Stock_Estante_Total}.", null);

                total += prod.PrecioVenta * det.Cantidad;
                lineas.Add((prod, det.Cantidad));
            }

            // Validar monto recibido (solo efectivo)
            decimal cambio = 0;
            if (dto.TipoPago == "Efectivo")
            {
                if (dto.MontoRecibido < total)
                    return (false,
                        $"Monto recibido (${dto.MontoRecibido:F2}) menor al total (${total:F2}).", null);
                cambio = dto.MontoRecibido - total;
            }

            // Crear venta
            var venta = new VentaFisica
            {
                FechaHora = DateTime.Now,
                TotalVenta = total,
                MontoRecibido = dto.TipoPago == "Efectivo" ? dto.MontoRecibido : total,
                Cambio = cambio,
                TipoPago = dto.TipoPago,
                ID_Usuario_Cajero = idCajero,
                ID_Caja = dto.ID_Caja
            };

            foreach (var (prod, cantidad) in lineas)
            {
                venta.Detalles.Add(new DetalleVentaFisica
                {
                    ID_Producto = prod.ID_Producto,
                    CantidadComprada = cantidad,
                    PrecioAlMomento = prod.PrecioVenta
                });

                // ── Descontar del Stock general del producto ──────────────────
                prod.Stock_Estante_Total -= cantidad;

                // ── Descontar de lotes usando FIFO (primero en vencer = primero en salir) ──
                int restante = cantidad;

                var lotesConEstante = _context.Inventario_Lotes
                    .Where(l => l.ID_Producto == prod.ID_Producto && l.UnidadesEnEstante > 0)
                    .OrderBy(l => l.FechaVencimiento)   // FIFO por fecha de vencimiento
                    .ToList();

                foreach (var lote in lotesConEstante)
                {
                    if (restante <= 0) break;

                    int descontar = Math.Min(restante, lote.UnidadesEnEstante);
                    lote.UnidadesEnEstante -= descontar;
                    lote.UnidadesVendidas += descontar;
                    restante -= descontar;

                    // Registrar movimiento en historial por cada lote afectado
                    _context.Historial_Movimientos.Add(new HistorialMovimiento
                    {
                        TipoMovimiento = "VENTA",
                        CantidadMovida = descontar,
                        Observaciones = $"Venta física - {prod.NombreProducto} ({descontar} unid. del lote {lote.CodigoLoteFisico})",
                        ID_Producto = prod.ID_Producto,
                        ID_Lote_Afectado = lote.ID_Lote,
                        ID_Usuario_Responsable = idCajero,
                        FechaHora = DateTime.Now
                    });
                }

                // Si aún queda restante > 0 es un caso de inconsistencia de datos
                // (Stock_Estante_Total decía que había pero los lotes no tienen)
                // Lo registramos sin lote para no perder la venta
                if (restante > 0)
                {
                    _context.Historial_Movimientos.Add(new HistorialMovimiento
                    {
                        TipoMovimiento = "VENTA",
                        CantidadMovida = restante,
                        Observaciones = $"Venta física - {prod.NombreProducto} ({restante} unid. sin lote asignado - revisar inventario)",
                        ID_Producto = prod.ID_Producto,
                        ID_Lote_Afectado = null,
                        ID_Usuario_Responsable = idCajero,
                        FechaHora = DateTime.Now
                    });
                }
            }

            _context.Ventas_Fisicas.Add(venta);
            _context.SaveChanges();

            // Recargar con includes para el DTO
            var ventaCompleta = _context.Ventas_Fisicas
                .Include(v => v.Cajero)
                .Include(v => v.Caja)
                .Include(v => v.Detalles).ThenInclude(d => d.Producto)
                .First(v => v.ID_Venta == venta.ID_Venta);

            return (true, "Venta registrada correctamente.", ToDto(ventaCompleta));
        }

        private static VentaResponseDto ToDto(VentaFisica v) => new()
        {
            ID_Venta = v.ID_Venta,
            FechaHora = v.FechaHora,
            TotalVenta = v.TotalVenta,
            MontoRecibido = v.MontoRecibido,
            Cambio = v.Cambio,
            TipoPago = v.TipoPago,
            NombreCajero = v.Cajero?.NombreCompleto ?? "",
            NombreCaja = v.Caja?.NombreCaja ?? "",
            Detalles = v.Detalles?.Select(d => new DetalleVentaResponseDto
            {
                NombreProducto = d.Producto?.NombreProducto ?? "",
                Cantidad = d.CantidadComprada,
                PrecioUnitario = d.PrecioAlMomento,
                Subtotal = d.PrecioAlMomento * d.CantidadComprada
            }).ToList() ?? new()
        };
    }
}