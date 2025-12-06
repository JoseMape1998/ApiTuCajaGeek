using System.Collections.Generic;

namespace ApiTuCajaGeek.Models
{
    public class Category
    {
        public long Category_Id { get; set; }
        public string Category_name { get; set; } = string.Empty;
        public string? Category_description { get; set; }
        public ICollection<Images_Category>? ImagesCategory { get; set; }
        public ICollection<Products>? Products { get; set; }
    }
}
