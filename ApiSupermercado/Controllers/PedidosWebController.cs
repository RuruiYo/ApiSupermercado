using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SupermercadoAPI.DTOs.Pedidos;
using SupermercadoAPI.Services;
using System.Security.Claims;

namespace SupermercadoAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PedidosWebController : ControllerBase
    {
        private readonly PedidoService _service;

        public PedidosWebController(PedidoService service)
        {
            _service = service;
        }

        // GET api/pedidosweb
        [HttpGet]
        [Authorize(Roles = "Cajero")]
        public IActionResult GetPedidos() => Ok(_service.GetTodos());

        // GET api/pedidosweb/mispedidos
        [HttpGet("mispedidos")]
        [Authorize(Roles = "Cliente")]
        public IActionResult GetMisPedidos()
        {
            var idCliente = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            return Ok(_service.GetMisPedidos(idCliente));
        }

        // POST api/pedidosweb
        [HttpPost]
        [Authorize(Roles = "Cliente")]
        public IActionResult CrearPedido(PedidoCreateDto dto)
        {
            var idCliente = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var (exito, mensaje, datos) = _service.Crear(dto, idCliente);
            if (!exito)
            {
                if (mensaje.Contains("No se encontró")) return NotFound(new { mensaje });
                return BadRequest(new { mensaje });
            }
            return CreatedAtAction(nameof(GetMisPedidos), null, new { mensaje, datos });
        }

        // PATCH api/pedidosweb/5/estado
        [HttpPatch("{id}/estado")]
        [Authorize(Roles = "Cajero")]
        public IActionResult CambiarEstado(int id, PedidoEstadoDto dto)
        {
            var idCajero = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var (exito, mensaje) = _service.CambiarEstado(id, dto.EstadoPedido, idCajero);
            if (!exito)
            {
                if (mensaje.Contains("No se encontró")) return NotFound(new { mensaje });
                return BadRequest(new { mensaje });
            }
            return Ok(new { mensaje });
        }


    }
}
