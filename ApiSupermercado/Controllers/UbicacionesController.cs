using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SupermercadoAPI.DTOs.Ubicaciones;
using SupermercadoAPI.Services;

namespace SupermercadoAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UbicacionesController : ControllerBase
    {
        private readonly UbicacionService _service;
        public UbicacionesController(UbicacionService service) { _service = service; }

        // GET api/ubicaciones  → accesible por Admin y Bodeguero
        [HttpGet]
        [Authorize(Roles = "Administrador,Bodeguero")]
        public IActionResult GetUbicaciones() => Ok(_service.GetTodas());

        // GET api/ubicaciones/5
        [HttpGet("{id}")]
        [Authorize(Roles = "Administrador,Bodeguero")]
        public IActionResult GetUbicacion(int id)
        {
            var (exito, mensaje, datos) = _service.GetPorId(id);
            if (!exito) return NotFound(new { mensaje });
            return Ok(datos);
        }

        // POST api/ubicaciones  → solo Admin
        [HttpPost]
        [Authorize(Roles = "Administrador")]
        public IActionResult CreateUbicacion([FromBody] UbicacionCreateDto dto)
        {
            var (exito, mensaje, id) = _service.Crear(dto);
            if (!exito) return Conflict(new { mensaje });
            return CreatedAtAction(nameof(GetUbicacion), new { id }, new { mensaje, id });
        }

        // DELETE api/ubicaciones/5  → solo Admin
        [HttpDelete("{id}")]
        [Authorize(Roles = "Administrador")]
        public IActionResult DeleteUbicacion(int id)
        {
            var (exito, mensaje) = _service.Eliminar(id);
            if (!exito)
            {
                if (mensaje.Contains("encontró")) return NotFound(new { mensaje });
                return BadRequest(new { mensaje });
            }
            return Ok(new { mensaje });
        }
    }
}
