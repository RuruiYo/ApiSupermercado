using Microsoft.EntityFrameworkCore;
using SupermercadoAPI.Data;
using SupermercadoAPI.DTOs.Movimientos;
using SupermercadoAPI.Models;

namespace SupermercadoAPI.Services
{
    public class MovimientoService
    {
        private readonly AppDbContext _context;

        public MovimientoService(AppDbContext context)
        {
            _context = context;
        }

        public List<MovimientoResponseDto> GetTodos()
        {
            return _context.Historial_Movimientos
                .Include(m => m.Producto)
                .Include(m => m.LoteAfectado)
                .Include(m => m.UsuarioResponsable)
                .OrderByDescending(m => m.FechaHora)
                .Select(m => new MovimientoResponseDto
                {
                    ID_Movimiento = m.ID_Movimiento,
                    FechaHora = m.FechaHora,
                    TipoMovimiento = m.TipoMovimiento,
                    CantidadMovida = m.CantidadMovida,
                    Observaciones = m.Observaciones,
                    NombreProducto = m.Producto!.NombreProducto,
                    CodigoLote = m.LoteAfectado != null ? m.LoteAfectado.CodigoLoteFisico : "N/A",
                    NombreUsuario = m.UsuarioResponsable!.NombreCompleto
                }).ToList();
        }

        // ════════════════════════════════════════════════════════════════════════════════
        // NUEVO: Registrar Entrada de Productos
        // ════════════════════════════════════════════════════════════════════════════════
        public (bool Exito, string Mensaje, int ID) RegistrarEntrada(EntradaDto dto, int idUsuario)
        {
            var lote = _context.Inventario_Lotes
                .Include(l => l.Producto)
                .FirstOrDefault(l => l.ID_Lote == dto.ID_Lote);

            if (lote == null)
                return (false, $"No se encontró el lote con ID {dto.ID_Lote}.", 0);

            // Validar que la cantidad registrada no supere la original del lote
            int totalActual = lote.UnidadesEnBodega + lote.UnidadesEnEstante + lote.UnidadesVendidas;
            if (totalActual + dto.Cantidad > lote.CantidadOriginal)
                return (false, $"La cantidad supera el total original del lote ({lote.CantidadOriginal}). Disponible: {lote.CantidadOriginal - totalActual} unidades.", 0);

            // Actualizar inventario del lote
            lote.UnidadesEnBodega += dto.Cantidad;

            // Actualizar stock general del producto
            lote.Producto!.Stock_Bodega_Total += dto.Cantidad;

            // Registrar el movimiento en el historial
            var movimiento = new HistorialMovimiento
            {
                TipoMovimiento = "ENTRADA_LOTE",
                CantidadMovida = dto.Cantidad,
                Observaciones = dto.Observaciones ?? "Registro de entrada de lote",
                ID_Producto = lote.ID_Producto,
                ID_Lote_Afectado = lote.ID_Lote,
                ID_Usuario_Responsable = idUsuario,
                FechaHora = DateTime.Now
            };

            _context.Historial_Movimientos.Add(movimiento);
            _context.SaveChanges();

            return (true, $"Entrada registrada: {dto.Cantidad} unidades agregadas a bodega.", movimiento.ID_Movimiento);
        }

        // ════════════════════════════════════════════════════════════════════════════════
        // Trasladar Unidades de Bodega a Estante
        // ════════════════════════════════════════════════════════════════════════════════
        public (bool Exito, string Mensaje) Trasladar(TrasladoDto dto, int idUsuario)
        {
            var lote = _context.Inventario_Lotes
                .Include(l => l.Producto)
                .FirstOrDefault(l => l.ID_Lote == dto.ID_Lote);

            if (lote == null)
                return (false, $"No se encontró el lote con ID {dto.ID_Lote}.");

            if (lote.UnidadesEnBodega <= 0)
                return (false, "Este lote no tiene unidades disponibles en bodega.");

            if (lote.UnidadesEnBodega < dto.Cantidad)
                return (false, $"Stock insuficiente en bodega. Disponible: {lote.UnidadesEnBodega} unidades.");

            lote.UnidadesEnBodega -= dto.Cantidad;
            lote.UnidadesEnEstante += dto.Cantidad;
            lote.Producto!.Stock_Bodega_Total -= dto.Cantidad;
            lote.Producto.Stock_Estante_Total += dto.Cantidad;

            _context.Historial_Movimientos.Add(new HistorialMovimiento
            {
                TipoMovimiento = "TRASLADO_ESTANTE",
                CantidadMovida = dto.Cantidad,
                Observaciones = dto.Observaciones,
                ID_Producto = lote.ID_Producto,
                ID_Lote_Afectado = lote.ID_Lote,
                ID_Usuario_Responsable = idUsuario,
                FechaHora = DateTime.Now
            });

            _context.SaveChanges();
            return (true, $"Traslado de {dto.Cantidad} unidades realizado correctamente.");
        }

        // ════════════════════════════════════════════════════════════════════════════════
        // Descartar Unidades
        // ════════════════════════════════════════════════════════════════════════════════
        public (bool Exito, string Mensaje) Descartar(DescartarDto dto, int idUsuario)
        {
            var lote = _context.Inventario_Lotes
                .Include(l => l.Producto)
                .FirstOrDefault(l => l.ID_Lote == dto.ID_Lote);

            if (lote == null)
                return (false, $"No se encontro el lote con ID {dto.ID_Lote}.");

            int totalDisponible = lote.UnidadesEnBodega + lote.UnidadesEnEstante;
            if (totalDisponible <= 0)
                return (false, "Este lote no tiene unidades disponibles para descartar.");

            if (totalDisponible < dto.Cantidad)
                return (false, $"No hay suficientes unidades para descartar. Disponible: {totalDisponible} unidades.");

            int restante = dto.Cantidad;

            // Descontar primero de bodega, luego de estante
            if (lote.UnidadesEnBodega >= restante)
            {
                lote.UnidadesEnBodega             -= restante;
                lote.Producto!.Stock_Bodega_Total  -= restante;
            }
            else
            {
                int deBodega  = lote.UnidadesEnBodega;
                int deEstante = restante - deBodega;

                lote.Producto!.Stock_Bodega_Total  -= deBodega;
                lote.UnidadesEnBodega               = 0;

                lote.UnidadesEnEstante             -= deEstante;
                lote.Producto.Stock_Estante_Total  -= deEstante;
            }

            // Acumular en UnidadesDescartadas (lote) y Stock_Descartado_Total (producto)
            lote.UnidadesDescartadas              += dto.Cantidad;
            lote.Producto!.Stock_Descartado_Total += dto.Cantidad;

            _context.Historial_Movimientos.Add(new HistorialMovimiento
            {
                TipoMovimiento         = "DESCARTE",
                CantidadMovida         = dto.Cantidad,
                Observaciones          = dto.Observaciones,
                ID_Producto            = lote.ID_Producto,
                ID_Lote_Afectado       = lote.ID_Lote,
                ID_Usuario_Responsable = idUsuario,
                FechaHora              = DateTime.Now
            });

            _context.SaveChanges();
            return (true, $"Descarte de {dto.Cantidad} unidades registrado correctamente.");
        }
    }
}