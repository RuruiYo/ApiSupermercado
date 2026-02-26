using Microsoft.EntityFrameworkCore;
using SupermercadoAPI.Data;
using SupermercadoAPI.DTOs.Usuarios;
using SupermercadoAPI.Models;

namespace SupermercadoAPI.Services
{
    public class UsuarioService
    {
        private readonly AppDbContext _context;

        public UsuarioService(AppDbContext context)
        {
            _context = context;
        }

        public List<UsuarioResponseDto> GetTodos()
        {
            return _context.Usuarios
                .Include(u => u.Rol)
                .Where(u => u.Rol!.NombreRol == "Bodeguero" || u.Rol!.NombreRol == "Cajero")
                .Select(u => new UsuarioResponseDto
                {
                    ID_Usuario = u.ID_Usuario,
                    NombreCompleto = u.NombreCompleto,
                    Correo_Usuario = u.Correo_Usuario,
                    EstadoActivo = u.EstadoActivo,
                    NombreRol = u.Rol!.NombreRol
                }).ToList();
        }

        public (bool Exito, string Mensaje, UsuarioResponseDto? Datos) GetPorId(int id)
        {
            var usuario = _context.Usuarios
                .Include(u => u.Rol)
                .FirstOrDefault(u => u.ID_Usuario == id);

            if (usuario == null)
                return (false, $"No se encontró el usuario con ID {id}.", null);

            return (true, "OK", new UsuarioResponseDto
            {
                ID_Usuario = usuario.ID_Usuario,
                NombreCompleto = usuario.NombreCompleto,
                Correo_Usuario = usuario.Correo_Usuario,
                EstadoActivo = usuario.EstadoActivo,
                NombreRol = usuario.Rol!.NombreRol
            });
        }

        public (bool Exito, string Mensaje, int ID) Crear(UsuarioCreateDto dto)
        {
            var rol = _context.Roles.Find(dto.ID_Rol);
            if (rol == null)
                return (false, "El rol especificado no existe.", 0);

            if (rol.NombreRol != "Bodeguero" && rol.NombreRol != "Cajero")
                return (false, "Solo se pueden crear usuarios con rol Bodeguero o Cajero.", 0);

            if (_context.Usuarios.Any(u => u.Correo_Usuario == dto.Correo_Usuario))
                return (false, "Ya existe un usuario con ese correo.", 0);

            var usuario = new Usuario
            {
                NombreCompleto = dto.NombreCompleto,
                Correo_Usuario = dto.Correo_Usuario,
                Contrasena = dto.Contrasena,
                EstadoActivo = true,
                ID_Rol = dto.ID_Rol
            };

            _context.Usuarios.Add(usuario);
            _context.SaveChanges();

            return (true, "Usuario creado correctamente.", usuario.ID_Usuario);
        }

        public (bool Exito, string Mensaje) CambiarEstado(int id, bool estado)
        {
            var usuario = _context.Usuarios.Find(id);
            if (usuario == null)
                return (false, $"No se encontró el usuario con ID {id}.");

            usuario.EstadoActivo = estado;
            _context.SaveChanges();

            string texto = estado ? "activado" : "desactivado";
            return (true, $"Usuario {texto} correctamente.");
        }
    }
}
