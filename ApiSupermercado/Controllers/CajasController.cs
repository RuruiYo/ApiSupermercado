using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SupermercadoAPI.DTOs.Cajas;
using SupermercadoAPI.Services;
using System.Security.Claims;

namespace SupermercadoAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CajasController : ControllerBase
    {
        private readonly CajaService _service;
        public CajasController(CajaService service) { _service = service; }

        // GET api/cajas → Admin ve todas las cajas
        [HttpGet]
        [Authorize(Roles = "Administrador")]
        public IActionResult GetCajas() => Ok(_service.GetTodas());

        // GET api/cajas/5
        [HttpGet("{id}")]
        [Authorize(Roles = "Administrador")]
        public IActionResult GetCaja(int id)
        {
            var (exito, mensaje, datos) = _service.GetPorId(id);
            if (!exito) return NotFound(new { mensaje });
            return Ok(datos);
        }

        // GET api/cajas/mi-caja → Cajero consulta su caja asignada
        [HttpGet("mi-caja")]
        [Authorize(Roles = "Cajero")]
        public IActionResult GetMiCaja()
        {
            int idUsuario = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var (exito, mensaje, datos) = _service.GetPorCajero(idUsuario);
            if (!exito) return NotFound(new { mensaje });
            return Ok(datos);
        }

        // PATCH api/cajas/5/asignar → Admin asigna o desasigna un cajero
        [HttpPatch("{id}/asignar")]
        [Authorize(Roles = "Administrador")]
        public IActionResult AsignarCajero(int id, [FromBody] CajaAsignarDto dto)
        {
            var (exito, mensaje) = _service.Asignar(id, dto);
            if (!exito) return BadRequest(new { mensaje });
            return Ok(new { mensaje });
        }
    }
}
