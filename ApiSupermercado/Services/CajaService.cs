using Microsoft.EntityFrameworkCore;
using SupermercadoAPI.Data;
using SupermercadoAPI.DTOs.Cajas;
using SupermercadoAPI.Models;

namespace SupermercadoAPI.Services
{
    public class CajaService
    {
        private readonly AppDbContext _context;
        public CajaService(AppDbContext context) { _context = context; }

        public List<CajaResponseDto> GetTodas()
        {
            return _context.Cajas
                .Include(c => c.Cajero)
                .Include(c => c.Ventas)
                .OrderBy(c => c.NombreCaja)
                .ToList()
                .Select(c => ToDto(c))
                .ToList();
        }

        public (bool Exito, string Mensaje, CajaResponseDto? Datos) GetPorId(int id)
        {
            var c = _context.Cajas
                .Include(c => c.Cajero)
                .Include(c => c.Ventas)
                .FirstOrDefault(c => c.ID_Caja == id);
            if (c == null) return (false, $"No se encontró la caja con ID {id}.", null);
            return (true, "OK", ToDto(c));
        }

        public (bool Exito, string Mensaje, CajaResponseDto? Datos) GetPorCajero(int idUsuario)
        {
            var c = _context.Cajas
                .Include(c => c.Cajero)
                .Include(c => c.Ventas)
                .FirstOrDefault(c => c.ID_Usuario_Cajero == idUsuario && c.EstadoActiva);
            if (c == null)
                return (false, "No tienes una caja asignada. Contacta al administrador.", null);
            return (true, "OK", ToDto(c));
        }

        public (bool Exito, string Mensaje) Asignar(int idCaja, CajaAsignarDto dto)
        {
            var caja = _context.Cajas.Find(idCaja);
            if (caja == null)
                return (false, $"No se encontró la caja con ID {idCaja}.");

            if (dto.ID_Usuario_Cajero.HasValue)
            {
                int idCajero = dto.ID_Usuario_Cajero.Value;

                var usuario = _context.Usuarios.Find(idCajero);
                if (usuario == null)
                    return (false, "El usuario especificado no existe.");
                if (usuario.ID_Rol != 3)
                    return (false, "El usuario debe tener el rol de Cajero.");

                // Verificar que este cajero no esté en OTRA caja activa
                var otraCaja = _context.Cajas
                    .FirstOrDefault(c => c.ID_Usuario_Cajero == idCajero
                                      && c.ID_Caja != idCaja
                                      && c.EstadoActiva);
                if (otraCaja != null)
                    return (false, $"Este cajero ya está asignado a '{otraCaja.NombreCaja}'. " +
                                   $"Desasígnalo primero.");

                caja.ID_Usuario_Cajero = idCajero;
                caja.SaldoInicial = dto.SaldoInicial;
            }
            else
            {
                // Desasignar: dejar la caja libre
                caja.ID_Usuario_Cajero = null;
                caja.SaldoInicial = 0;
            }

            _context.SaveChanges();

            string msg = dto.ID_Usuario_Cajero.HasValue
                ? $"Cajero asignado a {caja.NombreCaja} correctamente."
                : $"{caja.NombreCaja} desasignada correctamente.";
            return (true, msg);
        }

        private static CajaResponseDto ToDto(Caja c)
        {
            decimal entradas = c.Ventas?.Sum(v => v.MontoRecibido) ?? 0;
            decimal salidas = c.Ventas?.Sum(v => v.Cambio) ?? 0;
            return new CajaResponseDto
            {
                ID_Caja = c.ID_Caja,
                NombreCaja = c.NombreCaja,
                NombreCajero = c.Cajero?.NombreCompleto,
                ID_Cajero = c.ID_Usuario_Cajero,
                SaldoInicial = c.SaldoInicial,
                TotalEntradas = entradas,
                TotalSalidas = salidas,
                SaldoActual = c.SaldoInicial + entradas - salidas,
                EstadoActiva = c.EstadoActiva,
                Disponible = c.ID_Usuario_Cajero == null
            };
        }
    }
}