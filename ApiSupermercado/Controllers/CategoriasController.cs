using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SupermercadoAPI.Data;

namespace SupermercadoAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriasController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CategoriasController(AppDbContext context)
        {
            _context = context;
        }

        // GET api/categorias  → accesible por todos los roles autenticados
        [HttpGet]
        [Authorize]
        public IActionResult GetCategorias()
        {
            var categorias = _context.Categorias
                .OrderBy(c => c.NombreCategoria)
                .Select(c => new
                {
                    iD_Categoria    = c.ID_Categoria,
                    nombreCategoria = c.NombreCategoria
                })
                .ToList();

            return Ok(categorias);
        }

        // GET api/categorias/1
        [HttpGet("{id}")]
        [Authorize]
        public IActionResult GetCategoria(int id)
        {
            var cat = _context.Categorias.Find(id);
            if (cat == null)
                return NotFound(new { mensaje = $"No se encontró la categoría con ID {id}." });

            return Ok(new
            {
                iD_Categoria    = cat.ID_Categoria,
                nombreCategoria = cat.NombreCategoria
            });
        }

        // POST api/categorias  → solo Admin
        [HttpPost]
        [Authorize(Roles = "Administrador")]
        public IActionResult CreateCategoria([FromBody] CategoriaCreateDto dto)
        {
            string nombre = dto.NombreCategoria.Trim();

            if (_context.Categorias.Any(c => c.NombreCategoria.ToLower() == nombre.ToLower()))
                return Conflict(new { mensaje = $"Ya existe una categoría llamada '{nombre}'." });

            var cat = new SupermercadoAPI.Models.Categoria { NombreCategoria = nombre };
            _context.Categorias.Add(cat);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetCategoria), new { id = cat.ID_Categoria },
                new { mensaje = "Categoría creada correctamente.", id = cat.ID_Categoria });
        }
    }

    public class CategoriaCreateDto
    {
        [System.ComponentModel.DataAnnotations.Required]
        [System.ComponentModel.DataAnnotations.MaxLength(100)]
        public string NombreCategoria { get; set; } = string.Empty;
    }
}
