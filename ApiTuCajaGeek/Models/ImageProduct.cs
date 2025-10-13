using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiTuCajaGeek.Models
{
    [Table("Images_Product")]
    public class ImageProduct
    {
        [Key]
        [Column("Image_Product_Id")]
        public long ImageProductId { get; set; }

        [Required]
        [Column("Product_Id")]
        public long ProductId { get; set; }

        [Required]
        [StringLength(500)]
        [Column("Image_Url")]
        public string ImageUrl { get; set; } = string.Empty;
    }
}
