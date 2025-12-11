using ApiTuCajaGeek.AppData;
using ApiTuCajaGeek.DTOs;
using ApiTuCajaGeek.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ApiTuCajaGeek.Controllers
{

    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class PurchaseController : ControllerBase
    {
        private readonly PurchaseService _service;
        private readonly ILogger<PurchaseController> _logger;

        public PurchaseController(PurchaseService service, ILogger<PurchaseController> logger)
        {
            _service = service;
            _logger = logger;
        }

        // 1. OBTENER TODAS LAS COMPRAS DEL USUARIO
        [HttpGet("user")]
        public async Task<IActionResult> GetUserPurchases()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            _logger.LogInformation("Obteniendo compras del usuario: {UserId}", userId);

            try
            {
                var purchases = await _service.GetUserPurchasesAsync(userId);
                return Ok(purchases);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo compras del usuario: {UserId}", userId);
                return StatusCode(500, new ErrorResponse { Message = "Error obteniendo compras", Detail = ex.Message });
            }
        }

        // 2. OBTENER UNA COMPRA POR PRODUCTO
        [HttpGet("user/product/{productId:long}")]
        public async Task<IActionResult> GetPurchaseByProduct(long productId)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            _logger.LogInformation("Obteniendo compra del usuario: {UserId} para el producto: {ProductId}", userId, productId);

            try
            {
                var purchase = await _service.GetPurchaseByProductAsync(userId, productId);

                if (purchase == null)
                    return NotFound(new ErrorResponse { Message = "Compra no encontrada" });

                return Ok(purchase);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo compra: User={UserId}, Product={ProductId}", userId, productId);
                return StatusCode(500, new ErrorResponse { Message = "Error obteniendo compra", Detail = ex.Message });
            }
        }

        // 3. PROCESAR COMPRA DESDE EL CARRITO
        [HttpPost("process")]
        public async Task<IActionResult> ProcessPurchase([FromBody] ProcessPurchaseRequestDto request)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            _logger.LogInformation("Procesando compra del usuario: {UserId}", userId);

            try
            {
                var result = await _service.ProcessPurchaseFromCartAsync(request, userId);
                return Ok(new { Message = "Compra procesada correctamente", Purchases = result });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error procesando compra del usuario: {UserId}", userId);
                return StatusCode(500, new ErrorResponse { Message = "Error procesando compra", Detail = ex.Message });
            }
        }

        // 4. DESHABILITAR UNA COMPRA
        [HttpPut("disable/product/{purchaseId:long}")]
        public async Task<IActionResult> DisablePurchase(long purchaseId)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            _logger.LogInformation("Deshabilitando compra del usuario: {userId} para el producto: {purchaseId}", userId, purchaseId);

            try
            {
                var ok = await _service.DisablePurchaseByProductAsync(userId, purchaseId);

                if (!ok)
                    return NotFound(new ErrorResponse { Message = "Compra no encontrada" });

                return Ok(new { Message = "Compra deshabilitada correctamente" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deshabilitando compra: User={userId}, Product={purchaseId}", userId, purchaseId);
                return StatusCode(500, new ErrorResponse { Message = "Error deshabilitando compra", Detail = ex.Message });
            }
        }
    }
}
