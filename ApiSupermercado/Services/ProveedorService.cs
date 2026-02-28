using SupermercadoAPI.Data;
using SupermercadoAPI.DTOs.Proveedores;
using SupermercadoAPI.Models;

namespace SupermercadoAPI.Services
{
    public class ProveedorService
    {
        private readonly AppDbContext _context;
        public ProveedorService(AppDbContext context) { _context = context; }

        public List<ProveedorResponseDto> GetTodos()
        {
            return _context.Proveedores
                .OrderBy(p => p.NombreEmpresa)
                .Select(p => ToDto(p))
                .ToList();
        }

        public (bool Exito, string Mensaje, ProveedorResponseDto? Datos) GetPorId(int id)
        {
            var p = _context.Proveedores.Find(id);
            if (p == null) return (false, $"No se encontró el proveedor con ID {id}.", null);
            return (true, "OK", ToDto(p));
        }

        public (bool Exito, string Mensaje, int ID) Crear(ProveedorCreateDto dto)
        {
            string nombre = dto.NombreEmpresa.Trim();
            if (_context.Proveedores.Any(p => p.NombreEmpresa.ToLower() == nombre.ToLower()))
                return (false, $"Ya existe un proveedor con el nombre '{nombre}'.", 0);

            var proveedor = new Proveedor
            {
                NombreEmpresa    = nombre,
                ContactoAsignado = dto.ContactoAsignado?.Trim(),
                Telefono         = dto.Telefono?.Trim(),
                EstadoActivo     = true
            };
            _context.Proveedores.Add(proveedor);
            _context.SaveChanges();
            return (true, "Proveedor creado correctamente.", proveedor.ID_Proveedor);
        }

        public (bool Exito, string Mensaje) CambiarEstado(int id, bool estado)
        {
            var p = _context.Proveedores.Find(id);
            if (p == null) return (false, $"No se encontró el proveedor con ID {id}.");
            p.EstadoActivo = estado;
            _context.SaveChanges();
            return (true, $"Proveedor {(estado ? "activado" : "desactivado")} correctamente.");
        }

        private static ProveedorResponseDto ToDto(Proveedor p) => new()
        {
            ID_Proveedor     = p.ID_Proveedor,
            NombreEmpresa    = p.NombreEmpresa,
            ContactoAsignado = p.ContactoAsignado,
            Telefono         = p.Telefono,
            EstadoActivo     = p.EstadoActivo
        };
    }
}
