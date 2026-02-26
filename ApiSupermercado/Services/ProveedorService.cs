using SupermercadoAPI.Data;
using SupermercadoAPI.DTOs.Proveedores;

namespace SupermercadoAPI.Services
{
    public class ProveedorService
    {
        private readonly AppDbContext _context;

        public ProveedorService(AppDbContext context)
        {
            _context = context;
        }

        public List<ProveedorResponseDto> GetTodos()
        {
            return _context.Proveedores
                .Select(p => new ProveedorResponseDto
                {
                    ID_Proveedor = p.ID_Proveedor,
                    NombreEmpresa = p.NombreEmpresa,
                    ContactoAsignado = p.ContactoAsignado,
                    Telefono = p.Telefono,
                    EstadoActivo = p.EstadoActivo
                }).ToList();
        }

        public (bool Exito, string Mensaje, ProveedorResponseDto? Datos) GetPorId(int id)
        {
            var p = _context.Proveedores.Find(id);
            if (p == null)
                return (false, $"No se encontró el proveedor con ID {id}.", null);

            return (true, "OK", new ProveedorResponseDto
            {
                ID_Proveedor = p.ID_Proveedor,
                NombreEmpresa = p.NombreEmpresa,
                ContactoAsignado = p.ContactoAsignado,
                Telefono = p.Telefono,
                EstadoActivo = p.EstadoActivo
            });
        }

        public (bool Exito, string Mensaje) CambiarEstado(int id, bool estado)
        {
            var proveedor = _context.Proveedores.Find(id);
            if (proveedor == null)
                return (false, $"No se encontró el proveedor con ID {id}.");

            proveedor.EstadoActivo = estado;
            _context.SaveChanges();

            string texto = estado ? "activado" : "desactivado";
            return (true, $"Proveedor {texto} correctamente.");
        }
    }
}
