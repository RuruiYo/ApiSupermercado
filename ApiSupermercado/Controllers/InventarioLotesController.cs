using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SupermercadoAPI.DTOs.Lotes;
using SupermercadoAPI.Services;
using System.Security.Claims;

namespace SupermercadoAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Bodeguero,Administrador")]
    public class InventarioLotesController : ControllerBase
    {
        private readonly LoteService _service;

        public InventarioLotesController(LoteService service)
        {
            _service = service;
        }

        // GET api/inventariolotes
        [HttpGet]
        public IActionResult GetLotes() => Ok(_service.GetTodos());

        // GET api/inventariolotes/producto/5
        [HttpGet("producto/{idProducto}")]
        public IActionResult GetLotesPorProducto(int idProducto)
        {
            var (exito, mensaje, datos) = _service.GetPorProducto(idProducto);
            if (!exito) return NotFound(new { mensaje });
            return Ok(datos);
        }

        // POST api/inventariolotes
        [HttpPost]
        [Authorize(Roles = "Bodeguero")]
        public IActionResult RegistrarLote(LoteCreateDto dto)
        {
            var idUsuario = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var (exito, mensaje, id) = _service.Registrar(dto, idUsuario);

            if (!exito)
            {
                if (mensaje.Contains("no existe")) return NotFound(new { mensaje });
                if (mensaje.Contains("código")) return Conflict(new { mensaje });
                return BadRequest(new { mensaje });
            }

            return CreatedAtAction(nameof(GetLotesPorProducto),
                new { idProducto = dto.ID_Producto },
                new { mensaje, id });
        }
    }
}
