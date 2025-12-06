using ApiTuCajaGeek.AppData;
using ApiTuCajaGeek.Data;
using ApiTuCajaGeek.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace ApiTuCajaGeek.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(AuthService authService, ILogger<AuthController> logger)
        {
            _authService = authService;
            _logger = logger;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(AuthRequest request)
        {
            try
            {
                var token = await _authService.LoginAsync(request.Email, request.Password);

                if (token == null)
                {
                    _logger.LogWarning("Intento de login fallido para el usuario {Email}", request.Email);

                    return Unauthorized(new ErrorResponse
                    {
                        Message = "Credenciales inválidas.",
                        Detail = "Usuario o contraseña incorrecta."
                    });
                }

                return Ok(new { token });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "Error inesperado durante el login del usuario {Email}. Mensaje: {Message}",
                    request.Email, ex.Message);

                return StatusCode(500, new ErrorResponse
                {
                    Message = "Ocurrió un error interno en el servidor.",
                    Detail = "Error inesperado en el login."
                });
            }
        }
    }

}
