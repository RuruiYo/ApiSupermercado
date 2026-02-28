using SupermercadoAPI.Data;
using SupermercadoAPI.DTOs.Ubicaciones;
using SupermercadoAPI.Models;

namespace SupermercadoAPI.Services
{
    public class UbicacionService
    {
        private readonly AppDbContext _context;
        public UbicacionService(AppDbContext context) { _context = context; }

        public List<UbicacionResponseDto> GetTodas()
        {
            return _context.Ubicaciones_Bodega
                .OrderBy(u => u.NombreUbicacion)
                .Select(u => new UbicacionResponseDto
                {
                    ID_Ubicacion    = u.ID_Ubicacion,
                    NombreUbicacion = u.NombreUbicacion,
                    TotalProductos  = _context.Productos.Count(p => p.ID_Ubicacion == u.ID_Ubicacion)
                })
                .ToList();
        }

        public (bool Exito, string Mensaje, UbicacionResponseDto? Datos) GetPorId(int id)
        {
            var u = _context.Ubicaciones_Bodega.Find(id);
            if (u == null) return (false, $"No se encontró la ubicación con ID {id}.", null);
            return (true, "OK", new UbicacionResponseDto
            {
                ID_Ubicacion    = u.ID_Ubicacion,
                NombreUbicacion = u.NombreUbicacion,
                TotalProductos  = _context.Productos.Count(p => p.ID_Ubicacion == id)
            });
        }

        public (bool Exito, string Mensaje, int ID) Crear(UbicacionCreateDto dto)
        {
            string nombre = dto.NombreUbicacion.Trim();
            if (_context.Ubicaciones_Bodega.Any(u => u.NombreUbicacion.ToLower() == nombre.ToLower()))
                return (false, $"Ya existe una ubicación llamada '{nombre}'.", 0);

            var ub = new UbicacionBodega { NombreUbicacion = nombre };
            _context.Ubicaciones_Bodega.Add(ub);
            _context.SaveChanges();
            return (true, "Ubicación creada correctamente.", ub.ID_Ubicacion);
        }

        public (bool Exito, string Mensaje) Eliminar(int id)
        {
            var ub = _context.Ubicaciones_Bodega.Find(id);
            if (ub == null) return (false, $"No se encontró la ubicación con ID {id}.");
            if (_context.Productos.Any(p => p.ID_Ubicacion == id))
                return (false, "No se puede eliminar: hay productos asignados a esta ubicación.");
            _context.Ubicaciones_Bodega.Remove(ub);
            _context.SaveChanges();
            return (true, "Ubicación eliminada correctamente.");
        }
    }
}
