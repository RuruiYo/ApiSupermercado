using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SupermercadoAPI.DTOs.Proveedores;
using SupermercadoAPI.Services;

namespace SupermercadoAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProveedoresController : ControllerBase
    {
        private readonly ProveedorService _service;
        public ProveedoresController(ProveedorService service) { _service = service; }

        // GET api/proveedores
        [HttpGet]
        [Authorize(Roles = "Administrador,Bodeguero")]
        public IActionResult GetProveedores() => Ok(_service.GetTodos());

        // GET api/proveedores/5
        [HttpGet("{id}")]
        [Authorize(Roles = "Administrador,Bodeguero")]
        public IActionResult GetProveedor(int id)
        {
            var (exito, mensaje, datos) = _service.GetPorId(id);
            if (!exito) return NotFound(new { mensaje });
            return Ok(datos);
        }

        // POST api/proveedores  → solo Admin
        [HttpPost]
        [Authorize(Roles = "Administrador")]
        public IActionResult CreateProveedor([FromBody] ProveedorCreateDto dto)
        {
            var (exito, mensaje, id) = _service.Crear(dto);
            if (!exito) return Conflict(new { mensaje });
            return CreatedAtAction(nameof(GetProveedor), new { id }, new { mensaje, id });
        }

        // PATCH api/proveedores/5/estado
        [HttpPatch("{id}/estado")]
        [Authorize(Roles = "Administrador")]
        public IActionResult CambiarEstado(int id, [FromBody] ProveedorEstadoDto dto)
        {
            var (exito, mensaje) = _service.CambiarEstado(id, dto.EstadoActivo);
            if (!exito) return NotFound(new { mensaje });
            return Ok(new { mensaje });
        }
    }
}
