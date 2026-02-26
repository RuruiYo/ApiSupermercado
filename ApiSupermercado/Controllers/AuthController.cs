using Microsoft.AspNetCore.Mvc;
using SupermercadoAPI.DTOs.Auth;
using SupermercadoAPI.Services;

namespace SupermercadoAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _service;

        public AuthController(AuthService service)
        {
            _service = service;
        }

        // POST api/auth/login
        [HttpPost("login")]
        public IActionResult Login(LoginDto dto)
        {
            var (exito, mensaje, datos) = _service.Login(dto);
            if (!exito) return Unauthorized(new { mensaje });
            return Ok(datos);
        }

        // POST api/auth/registro
        [HttpPost("registro")]
        public IActionResult Registro(RegistroDto dto)
        {
            var (exito, mensaje, datos) = _service.Registro(dto);
            if (!exito) return Conflict(new { mensaje });
            return CreatedAtAction(null, null, datos);
        }
    }
}
