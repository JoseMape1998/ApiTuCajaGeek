using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ApiTuCajaGeek.Data;
using ApiTuCajaGeek.DTOs;
using ApiTuCajaGeek.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace ApiTuCajaGeek.AppData
{
    public class CatalogService
    {
        private readonly ApiTuCajaGeekContext _context;
        private readonly IWebHostEnvironment _env;

        public CatalogService(ApiTuCajaGeekContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public async Task<Category> CreateCategoryAsync(CategoryDto dto)
        {
            if (await _context.Category.AnyAsync(c => c.Category_name == dto.Category_name))
                throw new Exception("La categoría ya existe");

            var category = new Category
            {
                Category_name = dto.Category_name,
                Category_description = dto.Category_description
            };

            _context.Category.Add(category);
            await _context.SaveChangesAsync();
            return category;
        }

        public async Task<Category?> UpdateCategoryAsync(long id, CategoryDto dto)
        {
            var category = await _context.Category.FindAsync(id);
            if (category == null) return null;

            category.Category_name = dto.Category_name;
            category.Category_description = dto.Category_description;
            await _context.SaveChangesAsync();
            return category;
        }

        public async Task<bool> DeleteCategoryAsync(long id)
        {
            var category = await _context.Category.FindAsync(id);
            if (category == null) return false;

            var images = await _context.Images_Category.Where(i => i.Category_Id == id).ToListAsync();

            foreach (var img in images)
            {
                DeleteFileIfExists(img.Image_Url, "categories");
            }

            _context.Images_Category.RemoveRange(images);
            _context.Category.Remove(category);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Images_Category> CreateCategoryImageAsync(long categoryId, IFormFile file)
        {
            var categoryExists = await _context.Category.AnyAsync(c => c.Category_Id == categoryId);
            if (!categoryExists) throw new Exception("Categoría no encontrada");

            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            var relativePath = Path.Combine("images", "categories", fileName);
            var fullPath = Path.Combine(_env.WebRootPath ?? "wwwroot", relativePath);

            Directory.CreateDirectory(Path.GetDirectoryName(fullPath) ?? string.Empty);

            await using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var img = new Images_Category
            {
                Category_Id = categoryId,
                Image_Url = "/" + relativePath.Replace("\\", "/")
            };

            _context.Images_Category.Add(img);
            await _context.SaveChangesAsync();
            return img;
        }

        public async Task<bool> DeleteCategoryImageAsync(long imageId)
        {
            var img = await _context.Images_Category.FindAsync(imageId);
            if (img == null) return false;

            DeleteFileIfExists(img.Image_Url, "categories");

            _context.Images_Category.Remove(img);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Products> CreateProductAsync(ProductDto dto)
        {
            var categoryExists = await _context.Category.AnyAsync(c => c.Category_Id == dto.Category_Id);
            if (!categoryExists) throw new Exception("La categoría especificada no existe");

            var product = new Products
            {
                Product_name = dto.Product_name,
                Product_description = dto.Product_description,
                Product_value_before_discount = dto.Product_value_before_discount,
                Product_value_after_discount = dto.Product_value_after_discount,
                Number_existences = dto.Number_existences,
                Stock_State = dto.Stock_State,
                Weight = dto.Weight,
                Height = dto.Height,
                Width = dto.Width,
                Depth = dto.Depth,
                Product_State = true,
                Category_Id = dto.Category_Id
            };

            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return product;
        }

        public async Task<Products?> UpdateProductAsync(long id, ProductDto dto)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null) return null;

            var categoryExists = await _context.Category.AnyAsync(c => c.Category_Id == dto.Category_Id);
            if (!categoryExists) throw new Exception("La categoría especificada no existe");

            product.Product_name = dto.Product_name;
            product.Product_description = dto.Product_description;
            product.Product_value_before_discount = dto.Product_value_before_discount;
            product.Product_value_after_discount = dto.Product_value_after_discount;
            product.Number_existences = dto.Number_existences;
            product.Stock_State = dto.Stock_State;
            product.Weight = dto.Weight;
            product.Height = dto.Height;
            product.Width = dto.Width;
            product.Depth = dto.Depth;
            product.Category_Id = dto.Category_Id;

            await _context.SaveChangesAsync();
            return product;
        }

        public async Task<bool> DisableProductAsync(long id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null) return false;

            product.Product_State = false;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Images_Product> CreateProductImageAsync(long productId, IFormFile file)
        {
            var productExists = await _context.Products.AnyAsync(p => p.Product_Id == productId);
            if (!productExists) throw new Exception("Producto no encontrado");

            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            var relativePath = Path.Combine("images", "products", fileName);
            var fullPath = Path.Combine(_env.WebRootPath ?? "wwwroot", relativePath);

            Directory.CreateDirectory(Path.GetDirectoryName(fullPath) ?? string.Empty);

            await using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var img = new Images_Product
            {
                Product_Id = productId,
                Image_Url = "/" + relativePath.Replace("\\", "/")
            };

            _context.Images_Product.Add(img);
            await _context.SaveChangesAsync();
            return img;
        }

        public async Task<bool> DeleteProductImageAsync(long imageId)
        {
            var img = await _context.Images_Product.FindAsync(imageId);
            if (img == null) return false;

            DeleteFileIfExists(img.Image_Url, "products");

            _context.Images_Product.Remove(img);
            await _context.SaveChangesAsync();
            return true;
        }

        private void DeleteFileIfExists(string imageUrl, string folder)
        {
            if (string.IsNullOrWhiteSpace(imageUrl)) return;

            var trimmed = imageUrl.TrimStart('/');
            var fullPath = Path.Combine(_env.WebRootPath ?? "wwwroot", trimmed.Replace("/", Path.DirectorySeparatorChar.ToString()));
            if (File.Exists(fullPath)) File.Delete(fullPath);
        }

        public async Task<List<Category>> GetCategoriesAsync()
        {
            return await _context.Category
                .Include(c => c.ImagesCategory)
                .ToListAsync();
        }

        public async Task<Category?> GetCategoryByIdAsync(long id)
        {
            return await _context.Category
                .Include(c => c.ImagesCategory)
                .FirstOrDefaultAsync(c => c.Category_Id == id);
        }

        public async Task<List<Products>> GetProductsAsync()
        {
            return await _context.Products
                .Include(p => p.Images)
                .Include(p => p.Category)
                .ToListAsync();
        }

        public async Task<Products?> GetProductByIdAsync(long id)
        {
            return await _context.Products
                .Include(p => p.Images)
                .Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.Product_Id == id);
        }
    }
}
