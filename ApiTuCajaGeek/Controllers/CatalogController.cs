using System;
using System.Threading.Tasks;
using ApiTuCajaGeek.AppData;
using ApiTuCajaGeek.DTOs;
using ApiTuCajaGeek.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiTuCajaGeek.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CatalogController : ControllerBase
    {
        private readonly CatalogService _service;
        private readonly ILogger<CatalogController> _logger;

        public CatalogController(CatalogService service, ILogger<CatalogController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpPost("categories")]
        public async Task<IActionResult> CreateCategory([FromBody] CategoryDto dto)
        {
            _logger.LogInformation("Creando categoría: {Name}", dto.Category_name);
            try
            {
                var category = await _service.CreateCategoryAsync(dto);
                return Ok(new { Message = "Categoría creada", CategoryId = category.Category_Id });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear categoría: {Name}", dto.Category_name);
                return StatusCode(500, new ErrorResponse { Message = "Error al crear categoría", Detail = ex.Message });
            }
        }

        [HttpPut("categories/{id:long}")]
        public async Task<IActionResult> EditCategory(long id, [FromBody] CategoryDto dto)
        {
            _logger.LogInformation("Editando categoría: {Id}", id);
            try
            {
                var updated = await _service.UpdateCategoryAsync(id, dto);
                if (updated == null) return NotFound(new ErrorResponse { Message = "Categoría no encontrada" });
                return Ok(new { Message = "Categoría actualizada" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al editar categoría: {Id}", id);
                return StatusCode(500, new ErrorResponse { Message = "Error al editar categoría", Detail = ex.Message });
            }
        }

        [HttpDelete("categories/{id:long}")]
        public async Task<IActionResult> DeleteCategory(long id)
        {
            _logger.LogInformation("Eliminando categoría: {Id}", id);
            try
            {
                var ok = await _service.DeleteCategoryAsync(id);
                if (!ok) return NotFound(new ErrorResponse { Message = "Categoría no encontrada" });
                return Ok(new { Message = "Categoría eliminada" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar categoría: {Id}", id);
                return StatusCode(500, new ErrorResponse { Message = "Error al eliminar categoría", Detail = ex.Message });
            }
        }

        [HttpPost("categories/{id:long}/images")]
        public async Task<IActionResult> CreateCategoryImage(long id, IFormFile file)
        {
            _logger.LogInformation("Creando imagen categoría: {Id}", id);
            try
            {
                if (file == null) return BadRequest(new ErrorResponse { Message = "Archivo no enviado" });
                var img = await _service.CreateCategoryImageAsync(id, file);
                return Ok(new { Message = "Imagen creada", ImageId = img.Image_category_Id, Url = img.Image_Url });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear imagen de categoría: {Id}", id);
                return StatusCode(500, new ErrorResponse { Message = "Error al crear imagen", Detail = ex.Message });
            }
        }

        [HttpDelete("categories/images/{imageId:long}")]
        public async Task<IActionResult> DeleteCategoryImage(long imageId)
        {
            _logger.LogInformation("Eliminando imagen categoría: {ImageId}", imageId);
            try
            {
                var ok = await _service.DeleteCategoryImageAsync(imageId);
                if (!ok) return NotFound(new ErrorResponse { Message = "Imagen no encontrada" });
                return Ok(new { Message = "Imagen eliminada" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar imagen categoría: {ImageId}", imageId);
                return StatusCode(500, new ErrorResponse { Message = "Error al eliminar imagen", Detail = ex.Message });
            }
        }

        [HttpPost("products")]
        public async Task<IActionResult> CreateProduct([FromForm] ProductDto dto)
        {
            _logger.LogInformation("Creando producto: {Name}", dto.Product_name);
            try
            {
                var product = await _service.CreateProductAsync(dto);
                return Ok(new { Message = "Producto creado", ProductId = product.Product_Id });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear producto: {Name}", dto.Product_name);
                return StatusCode(500, new ErrorResponse { Message = "Error al crear producto", Detail = ex.Message });
            }
        }

        [HttpPut("products/{id:long}")]
        public async Task<IActionResult> EditProduct(long id, [FromForm] ProductDto dto)
        {
            _logger.LogInformation("Editando producto: {Id}", id);
            try
            {
                var updated = await _service.UpdateProductAsync(id, dto);
                if (updated == null) return NotFound(new ErrorResponse { Message = "Producto no encontrado" });
                return Ok(new { Message = "Producto actualizado" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al editar producto: {Id}", id);
                return StatusCode(500, new ErrorResponse { Message = "Error al editar producto", Detail = ex.Message });
            }
        }

        [HttpPut("products/{id:long}/disable")]
        public async Task<IActionResult> DisableProduct(long id)
        {
            _logger.LogInformation("Inhabilitando producto: {Id}", id);
            try
            {
                var ok = await _service.DisableProductAsync(id);
                if (!ok) return NotFound(new ErrorResponse { Message = "Producto no encontrado" });
                return Ok(new { Message = "Producto inhabilitado" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al inhabilitar producto: {Id}", id);
                return StatusCode(500, new ErrorResponse { Message = "Error al inhabilitar producto", Detail = ex.Message });
            }
        }

        [HttpPost("products/{id:long}/images")]
        public async Task<IActionResult> CreateProductImage(long id, IFormFile file)
        {
            _logger.LogInformation("Creando imagen producto: {Id}", id);
            try
            {
                if (file == null) return BadRequest(new ErrorResponse { Message = "Archivo no enviado" });
                var img = await _service.CreateProductImageAsync(id, file);
                return Ok(new { Message = "Imagen creada", ImageId = img.Image_product_Id, Url = img.Image_Url });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear imagen de producto: {Id}", id);
                return StatusCode(500, new ErrorResponse { Message = "Error al crear imagen", Detail = ex.Message });
            }
        }

        [HttpDelete("products/images/{imageId:long}")]
        public async Task<IActionResult> DeleteProductImage(long imageId)
        {
            _logger.LogInformation("Eliminando imagen producto: {ImageId}", imageId);
            try
            {
                var ok = await _service.DeleteProductImageAsync(imageId);
                if (!ok) return NotFound(new ErrorResponse { Message = "Imagen no encontrada" });
                return Ok(new { Message = "Imagen eliminada" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar imagen producto: {ImageId}", imageId);
                return StatusCode(500, new ErrorResponse { Message = "Error al eliminar imagen", Detail = ex.Message });
            }
        }

        [HttpGet("categories")]
        public async Task<IActionResult> GetCategories()
        {
            try
            {
                var categories = await _service.GetCategoriesAsync();
                var result = categories.Select(c => new
                {
                    c.Category_Id,
                    c.Category_name,
                    c.Category_description,
                    Images = c.ImagesCategory?.Select(i => i.Image_Url)
                });
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener categorías");
                return StatusCode(500, new ErrorResponse { Message = "Error al obtener categorías", Detail = ex.Message });
            }
        }

        [HttpGet("categories/{id:long}")]
        public async Task<IActionResult> GetCategory(long id)
        {
            try
            {
                var category = await _service.GetCategoryByIdAsync(id);
                if (category == null) return NotFound(new ErrorResponse { Message = "Categoría no encontrada" });

                var result = new
                {
                    category.Category_Id,
                    category.Category_name,
                    category.Category_description,
                    Images = category.ImagesCategory?.Select(i => i.Image_Url)
                };
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener categoría: {Id}", id);
                return StatusCode(500, new ErrorResponse { Message = "Error al obtener categoría", Detail = ex.Message });
            }
        }

        [HttpGet("products")]
        public async Task<IActionResult> GetProducts()
        {
            try
            {
                var products = await _service.GetProductsAsync();
                var result = products.Select(p => new
                {
                    p.Product_Id,
                    p.Product_name,
                    p.Product_description,
                    p.Product_value_before_discount,
                    p.Product_value_after_discount,
                    p.Number_existences,
                    p.Stock_State,
                    p.Weight,
                    p.Height,
                    p.Width,
                    p.Depth,
                    p.Product_State,
                    Category = new
                    {
                        p.Category?.Category_Id,
                        p.Category?.Category_name
                    },
                    Images = p.Images?.Select(i => i.Image_Url)
                });
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener productos");
                return StatusCode(500, new ErrorResponse { Message = "Error al obtener productos", Detail = ex.Message });
            }
        }

        [HttpGet("products/{id:long}")]
        public async Task<IActionResult> GetProduct(long id)
        {
            try
            {
                var product = await _service.GetProductByIdAsync(id);
                if (product == null) return NotFound(new ErrorResponse { Message = "Producto no encontrado" });

                var result = new
                {
                    product.Product_Id,
                    product.Product_name,
                    product.Product_description,
                    product.Product_value_before_discount,
                    product.Product_value_after_discount,
                    product.Number_existences,
                    product.Stock_State,
                    product.Weight,
                    product.Height,
                    product.Width,
                    product.Depth,
                    product.Product_State,
                    Category = new
                    {
                        product.Category?.Category_Id,
                        product.Category?.Category_name
                    },
                    Images = product.Images?.Select(i => i.Image_Url)
                };
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener producto: {Id}", id);
                return StatusCode(500, new ErrorResponse { Message = "Error al obtener producto", Detail = ex.Message });
            }
        }
    }
}
