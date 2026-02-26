using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SupermercadoAPI.DTOs.Ventas;
using SupermercadoAPI.Services;
using System.Security.Claims;

namespace SupermercadoAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Cajero")]
    public class VentasFisicasController : ControllerBase
    {
        private readonly VentaService _service;

        public VentasFisicasController(VentaService service)
        {
            _service = service;
        }

        // GET api/ventasfisicas
        [HttpGet]
        public IActionResult GetVentas() => Ok(_service.GetTodas());

        // POST api/ventasfisicas
        [HttpPost]
        public IActionResult RegistrarVenta(VentaCreateDto dto)
        {
            var idCajero = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var (exito, mensaje, datos) = _service.Registrar(dto, idCajero);
            if (!exito)
            {
                if (mensaje.Contains("No se encontró")) return NotFound(new { mensaje });
                return BadRequest(new { mensaje });
            }
            return CreatedAtAction(null, null, new { mensaje, datos });
        }
    }
}
