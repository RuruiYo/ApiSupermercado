using Microsoft.EntityFrameworkCore;
using SupermercadoAPI.Data;
using SupermercadoAPI.DTOs.Productos;
using SupermercadoAPI.Models;

namespace SupermercadoAPI.Services
{
    public class ProductoService
    {
        private readonly AppDbContext _context;
        public ProductoService(AppDbContext context) { _context = context; }

        public List<ProductoResponseDto> GetTodos()
        {
            return _context.Productos
                .Include(p => p.Categoria)
                .Include(p => p.Ubicacion)
                .Select(p => ToDto(p))
                .ToList();
        }

        public List<ProductoResponseDto> GetCatalogo(string? nombre, int? categoriaId,
            decimal? precioMin, decimal? precioMax)
        {
            var q = _context.Productos
                .Include(p => p.Categoria)
                .Include(p => p.Ubicacion)
                .Where(p => p.Stock_Estante_Total > 0)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(nombre))
                q = q.Where(p => p.NombreProducto.Contains(nombre));
            if (categoriaId.HasValue)
                q = q.Where(p => p.ID_Categoria == categoriaId.Value);
            if (precioMin.HasValue)
                q = q.Where(p => p.PrecioVenta >= precioMin.Value);
            if (precioMax.HasValue)
                q = q.Where(p => p.PrecioVenta <= precioMax.Value);

            return q.Select(p => ToDto(p)).ToList();
        }

        public (bool Exito, string Mensaje, ProductoResponseDto? Datos) GetPorId(int id)
        {
            var p = _context.Productos
                .Include(p => p.Categoria)
                .Include(p => p.Ubicacion)
                .FirstOrDefault(p => p.ID_Producto == id);
            if (p == null) return (false, $"No se encontró el producto con ID {id}.", null);
            return (true, "OK", ToDto(p));
        }

        public (bool Exito, string Mensaje, int ID) Crear(ProductoCreateDto dto)
        {
            if (_context.Productos.Any(p => p.SKU_CodigoInterno == dto.SKU_CodigoInterno))
                return (false, $"Ya existe un producto con el SKU '{dto.SKU_CodigoInterno}'.", 0);

            if (!_context.Categorias.Any(c => c.ID_Categoria == dto.ID_Categoria))
                return (false, "La categoría seleccionada no existe.", 0);

            if (!_context.Ubicaciones_Bodega.Any(u => u.ID_Ubicacion == dto.ID_Ubicacion))
                return (false, "La ubicación de bodega seleccionada no existe.", 0);

            var producto = new Producto
            {
                SKU_CodigoInterno = dto.SKU_CodigoInterno,
                NombreProducto    = dto.NombreProducto,
                Descripcion       = dto.Descripcion,
                PrecioVenta       = dto.PrecioVenta,
                ImagenUrl         = dto.ImagenUrl,
                ID_Categoria      = dto.ID_Categoria,
                ID_Ubicacion      = dto.ID_Ubicacion
            };
            _context.Productos.Add(producto);
            _context.SaveChanges();
            return (true, "Producto creado correctamente.", producto.ID_Producto);
        }

        public (bool Exito, string Mensaje) Actualizar(int id, ProductoUpdateDto dto)
        {
            var producto = _context.Productos.Find(id);
            if (producto == null) return (false, $"No se encontró el producto con ID {id}.");

            if (!_context.Categorias.Any(c => c.ID_Categoria == dto.ID_Categoria))
                return (false, "La categoría seleccionada no existe.");

            if (!_context.Ubicaciones_Bodega.Any(u => u.ID_Ubicacion == dto.ID_Ubicacion))
                return (false, "La ubicación de bodega seleccionada no existe.");

            producto.NombreProducto = dto.NombreProducto;
            producto.Descripcion    = dto.Descripcion;
            producto.PrecioVenta    = dto.PrecioVenta;
            producto.ImagenUrl      = dto.ImagenUrl;
            producto.ID_Categoria   = dto.ID_Categoria;
            producto.ID_Ubicacion   = dto.ID_Ubicacion;

            _context.SaveChanges();
            return (true, "Producto actualizado correctamente.");
        }

        private static ProductoResponseDto ToDto(Producto p) => new()
        {
            ID_Producto           = p.ID_Producto,
            SKU_CodigoInterno     = p.SKU_CodigoInterno,
            NombreProducto        = p.NombreProducto,
            Descripcion           = p.Descripcion,
            PrecioVenta           = p.PrecioVenta,
            Stock_Bodega_Total    = p.Stock_Bodega_Total,
            Stock_Estante_Total   = p.Stock_Estante_Total,
            Stock_Reservado_Total = p.Stock_Reservado_Total,
            ImagenUrl             = p.ImagenUrl,
            NombreCategoria       = p.Categoria!.NombreCategoria,
            ID_Categoria          = p.ID_Categoria,
            NombreUbicacion       = p.Ubicacion!.NombreUbicacion,
            ID_Ubicacion          = p.ID_Ubicacion
        };
    }
}
