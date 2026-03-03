using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SupermercadoAPI.DTOs.Ventas;
using SupermercadoAPI.Services;
using System.Security.Claims;

namespace SupermercadoAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VentasController : ControllerBase
    {
        private readonly VentaService _service;
        public VentasController(VentaService service) { _service = service; }

        // GET api/ventas → Admin ve todas
        [HttpGet]
        [Authorize(Roles = "Administrador")]
        public IActionResult GetVentas() => Ok(_service.GetTodas());

        // GET api/ventas/mis-ventas → Cajero ve solo las suyas
        [HttpGet("mis-ventas")]
        [Authorize(Roles = "Cajero")]
        public IActionResult GetMisVentas()
        {
            int idCajero = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            return Ok(_service.GetPorCajero(idCajero));
        }

        // POST api/ventas → Cajero registra una venta
        [HttpPost]
        [Authorize(Roles = "Cajero")]
        public IActionResult CreateVenta([FromBody] VentaCreateDto dto)
        {
            int idCajero = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var (exito, mensaje, datos) = _service.Registrar(dto, idCajero);
            if (!exito) return BadRequest(new { mensaje });
            return Ok(new { mensaje, venta = datos });
        }
    }
}
