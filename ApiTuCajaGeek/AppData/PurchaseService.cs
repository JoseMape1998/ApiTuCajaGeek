using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiTuCajaGeek.Data;
using ApiTuCajaGeek.DTOs;
using ApiTuCajaGeek.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiTuCajaGeek.AppData
{
    public class PurchaseService
    {
        private readonly ApiTuCajaGeekContext _context;

        public PurchaseService(ApiTuCajaGeekContext context)
        {
            _context = context;
        }

        public async Task<List<PurchaseResponseDto>> GetUserPurchasesAsync(Guid userId)
        {
            return await _context.Purchase_data
                .Where(p => p.UserId == userId && p.PurchaseState == true)
                .Include(p => p.Product)
                    .ThenInclude(pr => pr.Images)    // Images_Product -> Image_Url
                .Include(p => p.Product)
                    .ThenInclude(pr => pr.Category)  
                .Include(p => p.PurchaseType)        
                .OrderByDescending(p => p.ProductId) 
                .Select(p => new PurchaseResponseDto
                {
                    Purchase_Id = 0, // sin PK en el modelo mostrado
                    User_Id = p.UserId,
                    Purchase_type = p.PurchaseType.Name_purchase_type,
                    Purchase_address = p.PurchaseAddress,
                    Purchase_state = p.PurchaseState ?? false,
                    Product = new PurchaseProductDto
                    {
                        Product_Id = p.ProductId ?? 0,
                        Product_name = p.Product != null ? p.Product.Product_name : string.Empty,
                        Category_name = p.Product != null && p.Product.Category != null
                        ? p.Product.Category.Category_name : string.Empty,
                        Unit_value = p.UnitValue ?? 0m,
                        Images = p.Product != null && p.Product.Images != null
                        ? p.Product.Images.Select(i => i.Image_Url).ToList() : new List<string>()
                    }
                })
                .ToListAsync();
        }

        public async Task<PurchaseResponseDto?> GetPurchaseByProductAsync(Guid userId, long productId)
        {
            var p = await _context.Purchase_data
                .Where(x => x.UserId == userId && x.ProductId == productId && x.PurchaseState == true)
                .Include(x => x.Product).ThenInclude(pr => pr.Images)
                .Include(x => x.Product).ThenInclude(pr => pr.Category)
                .Include(x => x.PurchaseType)
                .FirstOrDefaultAsync();

            if (p == null) return null;

            return new PurchaseResponseDto
            {
                Purchase_Id = 0,
                User_Id = p.UserId,
                Purchase_type = p.PurchaseType.Name_purchase_type,
                Purchase_address = p.PurchaseAddress,
                Purchase_state = p.PurchaseState ?? false,
                Product = new PurchaseProductDto
                {
                    Product_Id = p.ProductId ?? 0,
                    Product_name = p.Product?.Product_name ?? string.Empty,
                    Category_name = p.Product?.Category?.Category_name ?? string.Empty,
                    Unit_value = p.UnitValue ?? 0m,
                    Images = p.Product?.Images != null
                                ? p.Product.Images.Select(i => i.Image_Url).ToList()
                                : new List<string>()
                }
            };
        }

        public async Task<List<PurchaseResponseDto>> ProcessPurchaseFromCartAsync(ProcessPurchaseRequestDto request)
        {
            // traemos items activos del carrito (filtrando productos activos en la entidad Products)
            var cartItems = await _context.Shopping_cart
                .Where(c => c.User_Id == request.User_Id && c.Product != null && c.Product.Product_State)
                .Include(c => c.Product)
                    .ThenInclude(p => p.Images)
                .Include(c => c.Product)
                    .ThenInclude(p => p.Category)
                .ToListAsync();

            if (!cartItems.Any())
                throw new Exception("El carrito está vacío o no contiene productos activos.");

            var purchases = new List<Purchase_data>();

            foreach (var item in cartItems)
            {
                var pd = new Purchase_data
                {
                    UserId = request.User_Id,
                    ProductId = item.Product_Id,
                    UnitValue = item.Unit_value,
                    PurchaseTypeId = request.Purchase_type_Id,
                    PurchaseAddress = request.Purchase_address,
                    PurchaseState = true
                };

                purchases.Add(pd);
            }

            await _context.Purchase_data.AddRangeAsync(purchases);

            // eliminar los items procesados del carrito
            _context.Shopping_cart.RemoveRange(cartItems);

            await _context.SaveChangesAsync();

            // devolver las compras creadas (filtradas por user)
            return await GetUserPurchasesAsync(request.User_Id);
        }

        public async Task<bool> DisablePurchaseByProductAsync(Guid userId, long productId)
        {
            var purchase = await _context.Purchase_data
                .FirstOrDefaultAsync(p => p.UserId == userId && p.ProductId == productId && p.PurchaseState == true);

            if (purchase == null) return false;

            purchase.PurchaseState = false;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
