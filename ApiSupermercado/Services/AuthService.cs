using Microsoft.IdentityModel.Tokens;
using SupermercadoAPI.Data;
using SupermercadoAPI.DTOs.Auth;
using SupermercadoAPI.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SupermercadoAPI.Services
{
    public class AuthService
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _config;

        public AuthService(AppDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        public (bool Exito, string Mensaje, object? Datos) Login(LoginDto dto)
        {
            var usuario = _context.Usuarios
                .FirstOrDefault(u => u.Correo_Usuario == dto.Correo_Usuario
                                  && u.Contrasena == dto.Contrasena);

            if (usuario == null)
                return (false, "Correo o contraseña incorrectos.", null);

            if (!usuario.EstadoActivo)
                return (false, "El usuario está inactivo. Contacte al administrador.", null);

            var rol = _context.Roles.Find(usuario.ID_Rol);
            if (rol == null)
                return (false, "Error al obtener el rol del usuario.", null);

            var token = GenerarToken(usuario.ID_Usuario, usuario.Correo_Usuario, rol.NombreRol);

            return (true, "Login exitoso.", new
            {
                token,
                usuario.ID_Usuario,
                usuario.NombreCompleto,
                usuario.Correo_Usuario,
                Rol = rol.NombreRol
            });
        }

        public (bool Exito, string Mensaje, object? Datos) Registro(RegistroDto dto)
        {
            if (_context.Usuarios.Any(u => u.Correo_Usuario == dto.Correo_Usuario))
                return (false, "Ya existe una cuenta con ese correo.", null);

            var rolCliente = _context.Roles.FirstOrDefault(r => r.NombreRol == "Cliente");
            if (rolCliente == null)
                return (false, "Error de configuración: el rol Cliente no existe.", null);

            var usuario = new Usuario
            {
                NombreCompleto = dto.NombreCompleto,
                Correo_Usuario = dto.Correo_Usuario,
                Contrasena = dto.Contrasena,
                EstadoActivo = true,
                ID_Rol = rolCliente.ID_Rol
            };

            _context.Usuarios.Add(usuario);
            _context.SaveChanges();

            var token = GenerarToken(usuario.ID_Usuario, usuario.Correo_Usuario, "Cliente");

            return (true, "Cuenta creada correctamente.", new
            {
                token,
                usuario.ID_Usuario,
                usuario.NombreCompleto
            });
        }

        private string GenerarToken(int idUsuario, string correo, string rol)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, idUsuario.ToString()),
                new Claim(ClaimTypes.Email, correo),
                new Claim(ClaimTypes.Role, rol)
            };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(8),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
