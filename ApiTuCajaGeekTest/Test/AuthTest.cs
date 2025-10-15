using Xunit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ApiTuCajaGeek.Controllers;
using ApiTuCajaGeek.Data;
using ApiTuCajaGeek.Models;
using System;
using System.Security.Cryptography;
using System.Text;

namespace ApiTuCajaGeekTest.Test
{
    public class AuthTest
    {
        private AppDbContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()) 
                .Options;

            return new AppDbContext(options);
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

        [Fact]
        public void Login_DeberiaRetornarOk_CuandoCredencialesSonCorrectas()
        {
            // Arrange
            var context = GetInMemoryDbContext();

            var password = "123456";
            var passwordHash = GetSha256(password);

            var user = new User
            {
                UserId = Guid.NewGuid(),
                EmailUser = "test@correo.com",
                NameUser = "Juan",
                LastNameUser = "Pérez",
                PasswordHash = passwordHash,
                UserState = true,
                RolId = Guid.NewGuid()
            };

            context.Users.Add(user);
            context.SaveChanges();

            var controller = new AuthController(context);
            var request = new AuthRequest
            {
                Email = "test@correo.com",
                Password = "123456"
            };

            // Act
            var result = controller.Login(request) as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.Contains("Autenticación satisfactoria", result.Value.ToString());
        }

        [Fact]
        public void Login_DeberiaRetornarUnauthorized_CuandoCredencialesSonIncorrectas()
        {
            // Arrange
            var context = GetInMemoryDbContext();

            var user = new User
            {
                UserId = Guid.NewGuid(),
                EmailUser = "test@correo.com",
                NameUser = "Juan",
                LastNameUser = "Pérez",
                PasswordHash = GetSha256("123456"),
                UserState = true,
                RolId = Guid.NewGuid()
            };

            context.Users.Add(user);
            context.SaveChanges();

            var controller = new AuthController(context);
            var request = new AuthRequest
            {
                Email = "test@correo.com",
                Password = "incorrecta"
            };

            // Act
            var result = controller.Login(request) as UnauthorizedObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(401, result.StatusCode);
            Assert.Contains("Credenciales incorrectas", result.Value.ToString());
        }
    }
}
