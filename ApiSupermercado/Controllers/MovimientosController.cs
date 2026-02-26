using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SupermercadoAPI.DTOs.Movimientos;
using SupermercadoAPI.Services;
using System.Security.Claims;

namespace SupermercadoAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MovimientosController : ControllerBase
    {
        private readonly MovimientoService _service;

        public MovimientosController(MovimientoService service)
        {
            _service = service;
        }

        // GET api/movimientos
        [HttpGet]
        [Authorize(Roles = "Administrador")]
        public IActionResult GetMovimientos() => Ok(_service.GetTodos());

        // POST api/movimientos/traslado
        [HttpPost("traslado")]
        [Authorize(Roles = "Bodeguero")]
        public IActionResult Traslado(TrasladoDto dto)
        {
            var idUsuario = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var (exito, mensaje) = _service.Trasladar(dto, idUsuario);
            if (!exito)
            {
                if (mensaje.Contains("No se encontró")) return NotFound(new { mensaje });
                return BadRequest(new { mensaje });
            }
            return Ok(new { mensaje });
        }

        // POST api/movimientos/descartar
        [HttpPost("descartar")]
        [Authorize(Roles = "Bodeguero")]
        public IActionResult Descartar(DescartarDto dto)
        {
            var idUsuario = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var (exito, mensaje) = _service.Descartar(dto, idUsuario);
            if (!exito)
            {
                if (mensaje.Contains("No se encontró")) return NotFound(new { mensaje });
                return BadRequest(new { mensaje });
            }
            return Ok(new { mensaje });
        }
    }
}
