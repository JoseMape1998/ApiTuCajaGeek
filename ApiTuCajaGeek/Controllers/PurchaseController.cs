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
        [HttpGet("user/{userId:guid}")]
        public async Task<IActionResult> GetUserPurchases(Guid userId)
        {
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
        [HttpGet("user/{userId:guid}/product/{productId:long}")]
        public async Task<IActionResult> GetPurchaseByProduct(Guid userId, long productId)
        {
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
            _logger.LogInformation("Procesando compra del usuario: {UserId}", request.User_Id);

            try
            {
                var result = await _service.ProcessPurchaseFromCartAsync(request);
                return Ok(new { Message = "Compra procesada correctamente", Purchases = result });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error procesando compra del usuario: {UserId}", request.User_Id);
                return StatusCode(500, new ErrorResponse { Message = "Error procesando compra", Detail = ex.Message });
            }
        }

        // 4. DESHABILITAR UNA COMPRA
        [HttpPut("disable/{userId:guid}/product/{productId:long}")]
        public async Task<IActionResult> DisablePurchase(Guid userId, long productId)
        {
            _logger.LogInformation("Deshabilitando compra del usuario: {UserId} para el producto: {ProductId}", userId, productId);

            try
            {
                var ok = await _service.DisablePurchaseByProductAsync(userId, productId);

                if (!ok)
                    return NotFound(new ErrorResponse { Message = "Compra no encontrada" });

                return Ok(new { Message = "Compra deshabilitada correctamente" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deshabilitando compra: User={UserId}, Product={ProductId}", userId, productId);
                return StatusCode(500, new ErrorResponse { Message = "Error deshabilitando compra", Detail = ex.Message });
            }
        }
    }
}
