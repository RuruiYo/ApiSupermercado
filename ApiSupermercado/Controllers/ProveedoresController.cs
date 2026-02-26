using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SupermercadoAPI.DTOs.Proveedores;
using SupermercadoAPI.Services;

namespace SupermercadoAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Administrador")]
    public class ProveedoresController : ControllerBase
    {
        private readonly ProveedorService _service;

        public ProveedoresController(ProveedorService service)
        {
            _service = service;
        }

        // GET api/proveedores
        [HttpGet]
        public IActionResult GetProveedores() => Ok(_service.GetTodos());

        // GET api/proveedores/5
        [HttpGet("{id}")]
        public IActionResult GetProveedor(int id)
        {
            var (exito, mensaje, datos) = _service.GetPorId(id);
            if (!exito) return NotFound(new { mensaje });
            return Ok(datos);
        }

        // PATCH api/proveedores/5/estado
        [HttpPatch("{id}/estado")]
        public IActionResult CambiarEstado(int id, ProveedorEstadoDto dto)
        {
            var (exito, mensaje) = _service.CambiarEstado(id, dto.EstadoActivo);
            if (!exito) return NotFound(new { mensaje });
            return Ok(new { mensaje });
        }
    }
}
