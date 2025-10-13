using Microsoft.AspNetCore.Mvc;
using ApiTuCajaGeek.Data;
using ApiTuCajaGeek.Models;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace ApiTuCajaGeek.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AuthController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost("register")]
        public IActionResult Register([FromBody] AuthRequest req)
        {
            if (string.IsNullOrWhiteSpace(req.Email) || string.IsNullOrWhiteSpace(req.Password))
                return BadRequest(new { message = "Email y contraseña son requeridos." });

            if (!Regex.IsMatch(req.Email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                return BadRequest(new { message = "El formato del correo electrónico no es válido." });

            if (req.Password.Length < 6)
                return BadRequest(new { message = "La contraseña debe tener al menos 6 caracteres." });

            if (_context.Users.Any(u => u.EmailUser == req.Email))
                return Conflict(new { message = "El usuario ya existe." });

            string passwordHash = GetSha256(req.Password);

            var user = new User
            {
                UserId = Guid.NewGuid(),
                EmailUser = req.Email.Trim(),
                NameUser = req.Name?.Trim() ?? string.Empty,
                LastNameUser = req.LastName?.Trim() ?? string.Empty,
                CellNumberUser = req.CellNumber,
                PasswordHash = passwordHash,
                UserState = true,
                RolId = req.RolId ?? Guid.NewGuid()
            };

            _context.Users.Add(user);
            _context.SaveChanges();

            return Ok(new { message = "Registro exitoso." });
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] AuthRequest req)
        {
            if (string.IsNullOrWhiteSpace(req.Email) || string.IsNullOrWhiteSpace(req.Password))
                return BadRequest(new { message = "Email y contraseña son requeridos." });

            string passwordHash = GetSha256(req.Password);

            var user = _context.Users.FirstOrDefault(u =>
                u.EmailUser == req.Email && u.PasswordHash == passwordHash);

            if (user == null)
                return Unauthorized(new { message = "Credenciales incorrectas." });

            if (!user.UserState)
                return Unauthorized(new { message = "El usuario está inactivo." });

            return Ok(new
            {
                message = "Autenticación satisfactoria.",
                user.EmailUser,
                user.NameUser,
                user.LastNameUser
            });
        }

        [HttpGet("users")]
        public IActionResult GetUsers()
        {
            var users = _context.Users
                .Select(u => new
                {
                    u.UserId,
                    u.EmailUser,
                    u.NameUser,
                    u.LastNameUser,
                    u.UserState
                })
                .ToList();

            return Ok(users);
        }

        private string GetSha256(string input)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));
                StringBuilder sb = new StringBuilder();
                foreach (byte b in bytes)
                    sb.Append(b.ToString("x2"));
                return sb.ToString();
            }
        }
    }
}
