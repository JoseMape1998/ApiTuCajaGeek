using ApiTuCajaGeek.Data;
using ApiTuCajaGeek.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ApiTuCajaGeek.AppData
{
    public class AuthService
    {
        private readonly ApiTuCajaGeekContext _context;
        private readonly IConfiguration _config;
        private readonly PasswordHasher<Users> _passwordHasher;

        public AuthService(ApiTuCajaGeekContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
            _passwordHasher = new PasswordHasher<Users>();
        }

        public async Task<string?> LoginAsync(string email, string password)
        {
            var user = await _context.Users
                .Include(x => x.Rol)
                .FirstOrDefaultAsync(x => x.Email_user == email);

            if (user == null)
                return null;

            var result = _passwordHasher.VerifyHashedPassword(user, user.Password_hash, password);
            if (result == PasswordVerificationResult.Failed)
                return null;

            return GenerateToken(user);
        }

        private string GenerateToken(Users user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
            new Claim(ClaimTypes.NameIdentifier, user.User_Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email_user),
            new Claim(ClaimTypes.Role, user.Rol.Name_Rol),
        };

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }

}
