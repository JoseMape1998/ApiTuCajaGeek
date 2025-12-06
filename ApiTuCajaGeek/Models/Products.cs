using System.Collections.Generic;

namespace ApiTuCajaGeek.Models
{
    public class Products
    {
        public long Product_Id { get; set; }
        public System.Guid SKU { get; set; }
        public string Product_name { get; set; } = string.Empty;
        public string? Product_description { get; set; }
        public decimal Product_value_before_discount { get; set; }
        public decimal Product_value_after_discount { get; set; }
        public int Number_existences { get; set; }
        public bool Stock_State { get; set; }
        public decimal? Weight { get; set; }
        public decimal? Height { get; set; }
        public decimal? Width { get; set; }
        public decimal? Depth { get; set; }
        public bool Product_State { get; set; }
        public long Category_Id { get; set; }
        public Category? Category { get; set; }
        public ICollection<Images_Product>? Images { get; set; }
        public ICollection<User_rating>? User_ratings { get; set; }
        public ICollection<Shopping_cart>? ShoppingCarts { get; set; }
    }
}
