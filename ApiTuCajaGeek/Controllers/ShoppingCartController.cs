using ApiTuCajaGeek.AppData;
using ApiTuCajaGeek.DTOs;
using ApiTuCajaGeek.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace ApiTuCajaGeek.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ShoppingCartController : ControllerBase
    {
        private readonly ShoppingCartService _service;
        private readonly ILogger<ShoppingCartController> _logger;

        public ShoppingCartController(ShoppingCartService service, ILogger<ShoppingCartController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddToCart([FromBody] ShoppingCartItemDto dto)
        {
            _logger.LogInformation("Agregando producto al carrito: {ProductId} para el usuario: {UserId}", dto.Product_Id, dto.User_Id);
            try
            {
                var item = await _service.AddToCartAsync(dto);
                return Ok(new { Message = "Producto agregado al carrito", Shopping_cart_Id = item.Shopping_cart_Id });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error agregando producto al carrito: {ProductId}", dto.Product_Id);
                return StatusCode(500, new ErrorResponse { Message = "Error agregando al carrito", Detail = ex.Message });
            }
        }

        [HttpPut("update/{id:long}")]
        public async Task<IActionResult> UpdateCartItem(long id, [FromBody] int newAmount)
        {
            _logger.LogInformation("Actualizando cantidad del carrito: {CartId}", id);
            try
            {
                var ok = await _service.UpdateCartItemAsync(id, newAmount);
                if (!ok) return NotFound(new ErrorResponse { Message = "Item no encontrado" });
                return Ok(new { Message = "Cantidad actualizada" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error actualizando carrito: {CartId}", id);
                return StatusCode(500, new ErrorResponse { Message = "Error actualizando carrito", Detail = ex.Message });
            }
        }

        [HttpDelete("remove/{id:long}")]
        public async Task<IActionResult> RemoveFromCart(long id)
        {
            _logger.LogInformation("Eliminando item del carrito: {CartId}", id);
            try
            {
                var ok = await _service.RemoveFromCartAsync(id);
                if (!ok) return NotFound(new ErrorResponse { Message = "Item no encontrado" });
                return Ok(new { Message = "Item eliminado" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error eliminando item del carrito: {CartId}", id);
                return StatusCode(500, new ErrorResponse { Message = "Error eliminando item del carrito", Detail = ex.Message });
            }
        }

        [HttpGet("{userId:guid}")]
        public async Task<IActionResult> GetUserCart(Guid userId)
        {
            _logger.LogInformation("Obteniendo carrito del usuario: {UserId}", userId);
            try
            {
                var cart = await _service.GetUserCartAsync(userId);
                return Ok(cart);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo carrito del usuario: {UserId}", userId);
                return StatusCode(500, new ErrorResponse { Message = "Error obteniendo carrito", Detail = ex.Message });
            }
        }
    }
}
