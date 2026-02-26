using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SupermercadoAPI.DTOs.Usuarios;
using SupermercadoAPI.Services;

namespace SupermercadoAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Administrador")]
    public class UsuariosController : ControllerBase
    {
        private readonly UsuarioService _service;

        public UsuariosController(UsuarioService service)
        {
            _service = service;
        }

        // GET api/usuarios
        [HttpGet]
        public IActionResult GetUsuarios() => Ok(_service.GetTodos());

        // GET api/usuarios/5
        [HttpGet("{id}")]
        public IActionResult GetUsuario(int id)
        {
            var (exito, mensaje, datos) = _service.GetPorId(id);
            if (!exito) return NotFound(new { mensaje });
            return Ok(datos);
        }

        // POST api/usuarios
        [HttpPost]
        public IActionResult CreateUsuario(UsuarioCreateDto dto)
        {
            var (exito, mensaje, id) = _service.Crear(dto);
            if (!exito)
            {
                if (mensaje.Contains("no existe")) return NotFound(new { mensaje });
                if (mensaje.Contains("Solo se pueden")) return BadRequest(new { mensaje });
                return Conflict(new { mensaje });
            }
            return CreatedAtAction(nameof(GetUsuario), new { id }, new { mensaje, id });
        }

        // PATCH api/usuarios/5/estado
        [HttpPatch("{id}/estado")]
        public IActionResult CambiarEstado(int id, UsuarioEstadoDto dto)
        {
            var (exito, mensaje) = _service.CambiarEstado(id, dto.EstadoActivo);
            if (!exito) return NotFound(new { mensaje });
            return Ok(new { mensaje });
        }
    }
}
