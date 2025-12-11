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
    public class ShoppingCartService
    {
        private readonly ApiTuCajaGeekContext _context;

        public ShoppingCartService(ApiTuCajaGeekContext context)
        {
            _context = context;
        }

        public async Task<Shopping_cart> AddToCartAsync(ShoppingCartItemDto dto, string userId)
        {
            var product = await _context.Products.FindAsync(dto.Product_Id);
            if (product == null) throw new Exception("Producto no encontrado");

            var existingItem = await _context.Shopping_cart
                .FirstOrDefaultAsync(c => c.User_Id == Guid.Parse(userId) && c.Product_Id == dto.Product_Id);

            if (existingItem != null)
            {
                existingItem.Amount += dto.Amount;
                await _context.SaveChangesAsync();
                return existingItem;
            }

            var cartItem = new Shopping_cart
            {
                User_Id = Guid.Parse(userId),
                Product_Id = dto.Product_Id,
                Unit_value = product.Product_value_after_discount,
                Amount = dto.Amount
            };

            _context.Shopping_cart.Add(cartItem);
            await _context.SaveChangesAsync();
            return cartItem;
        }

        public async Task<bool> RemoveFromCartAsync(long shoppingCartId)
        {
            var item = await _context.Shopping_cart.FindAsync(shoppingCartId);
            if (item == null) return false;

            _context.Shopping_cart.Remove(item);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateCartItemAsync(long shoppingCartId, int newAmount)
        {
            var item = await _context.Shopping_cart.FindAsync(shoppingCartId);
            if (item == null) return false;

            if (newAmount <= 0)
            {
                _context.Shopping_cart.Remove(item);
            }
            else
            {
                item.Amount = newAmount;
            }

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<ShoppingCartResponseDto>> GetUserCartAsync(string userId)
        {
            return await _context.Shopping_cart
                .Where(c => c.User_Id == Guid.Parse(userId) && c.Product.Product_State)
                .Select(c => new ShoppingCartResponseDto
                {
                    Shopping_cart_Id = c.Shopping_cart_Id,
                    User_Id = c.User_Id,
                    Product_Id = c.Product_Id,
                    Product_name = c.Product.Product_name,
                    Category_name = c.Product.Category.Category_name,
                    Unit_value = c.Unit_value,
                    Amount = c.Amount,
                    Subtotal = c.Subtotal,
                    Product_Urls = c.Product.Images
                                        .Select(img => img.Image_Url)
                                        .ToList()
                })
                .ToListAsync();
        }


    }
}
