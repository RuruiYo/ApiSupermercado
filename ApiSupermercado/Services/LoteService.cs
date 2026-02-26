using Microsoft.EntityFrameworkCore;
using SupermercadoAPI.Data;
using SupermercadoAPI.DTOs.Lotes;
using SupermercadoAPI.Models;

namespace SupermercadoAPI.Services
{
    public class LoteService
    {
        private readonly AppDbContext _context;

        public LoteService(AppDbContext context)
        {
            _context = context;
        }

        public List<LoteResponseDto> GetTodos()
        {
            return _context.Inventario_Lotes
                .Include(l => l.Producto)
                .Include(l => l.Proveedor)
                .Select(l => MapToDto(l)).ToList();
        }

        public (bool Exito, string Mensaje, List<LoteResponseDto>? Datos) GetPorProducto(int idProducto)
        {
            if (!_context.Productos.Any(p => p.ID_Producto == idProducto))
                return (false, $"No se encontró el producto con ID {idProducto}.", null);

            var lotes = _context.Inventario_Lotes
                .Include(l => l.Producto)
                .Include(l => l.Proveedor)
                .Where(l => l.ID_Producto == idProducto)
                .Select(l => MapToDto(l)).ToList();

            return (true, "OK", lotes);
        }

        public (bool Exito, string Mensaje, int ID) Registrar(LoteCreateDto dto, int idUsuario)
        {
            if (dto.FechaVencimiento <= dto.FechaProduccion)
                return (false, "La fecha de vencimiento debe ser posterior a la de producción.", 0);

            if (dto.FechaVencimiento <= DateOnly.FromDateTime(DateTime.Today))
                return (false, "No se puede ingresar un lote ya vencido.", 0);

            if (!_context.Productos.Any(p => p.ID_Producto == dto.ID_Producto))
                return (false, "El producto especificado no existe.", 0);

            var proveedor = _context.Proveedores.Find(dto.ID_Proveedor);
            if (proveedor == null)
                return (false, "El proveedor especificado no existe.", 0);

            if (!proveedor.EstadoActivo)
                return (false, "No se puede registrar un lote de un proveedor inactivo.", 0);

            if (_context.Inventario_Lotes.Any(l => l.CodigoLoteFisico == dto.CodigoLoteFisico))
                return (false, "Ya existe un lote con ese código.", 0);

            var lote = new InventarioLote
            {
                CodigoLoteFisico = dto.CodigoLoteFisico,
                FechaProduccion = dto.FechaProduccion,
                FechaVencimiento = dto.FechaVencimiento,
                CantidadOriginal = dto.CantidadOriginal,
                UnidadesEnBodega = dto.CantidadOriginal,
                ID_Producto = dto.ID_Producto,
                ID_Proveedor = dto.ID_Proveedor,
                ID_Usuario_Recibio = idUsuario
            };

            _context.Inventario_Lotes.Add(lote);

            var producto = _context.Productos.Find(dto.ID_Producto)!;
            producto.Stock_Bodega_Total += dto.CantidadOriginal;

            _context.Historial_Movimientos.Add(new HistorialMovimiento
            {
                TipoMovimiento = "ENTRADA_LOTE",
                CantidadMovida = dto.CantidadOriginal,
                Observaciones = $"Ingreso de lote {dto.CodigoLoteFisico} del proveedor {proveedor.NombreEmpresa}",
                ID_Producto = dto.ID_Producto,
                ID_Usuario_Responsable = idUsuario
            });

            _context.SaveChanges();

            // Vincular movimiento al lote recién creado
            var movimiento = _context.Historial_Movimientos
                .OrderByDescending(m => m.ID_Movimiento).First();
            movimiento.ID_Lote_Afectado = lote.ID_Lote;
            _context.SaveChanges();

            return (true, "Lote registrado correctamente.", lote.ID_Lote);
        }

        private static LoteResponseDto MapToDto(InventarioLote l) => new LoteResponseDto
        {
            ID_Lote = l.ID_Lote,
            CodigoLoteFisico = l.CodigoLoteFisico,
            FechaProduccion = l.FechaProduccion,
            FechaVencimiento = l.FechaVencimiento,
            FechaIngreso = l.FechaIngreso,
            CantidadOriginal = l.CantidadOriginal,
            UnidadesEnBodega = l.UnidadesEnBodega,
            UnidadesEnEstante = l.UnidadesEnEstante,
            UnidadesVendidas = l.UnidadesVendidas,
            NombreProducto = l.Producto!.NombreProducto,
            NombreProveedor = l.Proveedor!.NombreEmpresa
        };
    }
}
