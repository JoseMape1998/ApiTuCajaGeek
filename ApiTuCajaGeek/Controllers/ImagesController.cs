using Microsoft.AspNetCore.Mvc;
using ApiTuCajaGeek.Data;
using ApiTuCajaGeek.Models;
using System.IO;
using System.Linq;

namespace ApiTuCajaGeek.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ImagesController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public ImagesController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        [HttpPost("upload/{productId}")]
        public IActionResult UploadImage(long productId, [FromForm] IFormFile imageFile)
        {
            if (imageFile == null || imageFile.Length == 0)
                return BadRequest(new { message = "Debe subir una imagen válida." });

            string uploadsFolder = Path.Combine(_env.WebRootPath, "images", "productos");
            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            string uniqueFileName = $"{Guid.NewGuid()}_{imageFile.FileName}";
            string filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                imageFile.CopyTo(stream);
            }

            var image = new ImageProduct
            {
                ProductId = productId,
                ImageUrl = $"/images/productos/{uniqueFileName}"
            };

            _context.ImagesProduct.Add(image);
            _context.SaveChanges();

            return Ok(new { message = "Imagen subida correctamente.", image });
        }

        [HttpGet]
        public IActionResult GetAllImages()
        {
            var images = _context.ImagesProduct.ToList();
            return Ok(images);
        }

        [HttpGet("{id}")]
        public IActionResult GetImage(long id)
        {
            var image = _context.ImagesProduct.FirstOrDefault(i => i.ImageProductId == id);
            if (image == null)
                return NotFound(new { message = "Imagen no encontrada." });

            return Ok(image);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateImage(long id, [FromBody] ImageProduct updatedImage)
        {
            var existing = _context.ImagesProduct.FirstOrDefault(i => i.ImageProductId == id);
            if (existing == null)
                return NotFound(new { message = "Imagen no encontrada." });

            existing.ProductId = updatedImage.ProductId;
            existing.ImageUrl = updatedImage.ImageUrl;
            _context.SaveChanges();

            return Ok(new { message = "Imagen actualizada correctamente.", existing });
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteImage(long id)
        {
            var image = _context.ImagesProduct.FirstOrDefault(i => i.ImageProductId == id);
            if (image == null)
                return NotFound(new { message = "Imagen no encontrada." });

            // Eliminar archivo físico si existe
            string filePath = Path.Combine(_env.WebRootPath, image.ImageUrl.TrimStart('/'));
            if (System.IO.File.Exists(filePath))
                System.IO.File.Delete(filePath);

            _context.ImagesProduct.Remove(image);
            _context.SaveChanges();

            return Ok(new { message = "Imagen eliminada correctamente." });
        }
    }
}
