using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SupermercadoAPI.DTOs.Productos;
using SupermercadoAPI.Services;

namespace SupermercadoAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductosController : ControllerBase
    {
        private readonly ProductoService _service;

        public ProductosController(ProductoService service)
        {
            _service = service;
        }

        // GET api/productos
        [HttpGet]
        [Authorize(Roles = "Administrador,Bodeguero,Cajero")]
        public IActionResult GetProductos() => Ok(_service.GetTodos());

        // GET api/productos/catalogo
        [HttpGet("catalogo")]
        [Authorize(Roles = "Cliente")]
        public IActionResult GetCatalogo(
            [FromQuery] string? nombre,
            [FromQuery] int? categoriaId,
            [FromQuery] decimal? precioMin,
            [FromQuery] decimal? precioMax)
        {
            if (precioMin.HasValue && precioMax.HasValue && precioMin > precioMax)
                return BadRequest(new { mensaje = "El precio mínimo no puede ser mayor al precio máximo." });

            return Ok(_service.GetCatalogo(nombre, categoriaId, precioMin, precioMax));
        }

        // GET api/productos/5
        [HttpGet("{id}")]
        [Authorize(Roles = "Administrador,Bodeguero,Cajero,Cliente")]
        public IActionResult GetProducto(int id)
        {
            var (exito, mensaje, datos) = _service.GetPorId(id);
            if (!exito) return NotFound(new { mensaje });
            return Ok(datos);
        }

        // POST api/productos
        [HttpPost]
        [Authorize(Roles = "Administrador")]
        public IActionResult CreateProducto(ProductoCreateDto dto)
        {
            var (exito, mensaje, id) = _service.Crear(dto);
            if (!exito)
            {
                if (mensaje.Contains("no existe")) return NotFound(new { mensaje });
                if (mensaje.Contains("SKU")) return Conflict(new { mensaje });
                return BadRequest(new { mensaje });
            }
            return CreatedAtAction(nameof(GetProducto), new { id }, new { mensaje, id });
        }

        // PUT api/productos/5
        [HttpPut("{id}")]
        [Authorize(Roles = "Administrador")]
        public IActionResult UpdateProducto(int id, ProductoUpdateDto dto)
        {
            var (exito, mensaje) = _service.Actualizar(id, dto);
            if (!exito)
            {
                if (mensaje.Contains("No se encontró")) return NotFound(new { mensaje });
                return BadRequest(new { mensaje });
            }
            return Ok(new { mensaje });
        }
    }
}
