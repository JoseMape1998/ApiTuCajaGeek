namespace ApiTuCajaGeek.Models
{
    public class Images_Product
    {
        public long Image_product_Id { get; set; }
        public long Product_Id { get; set; }
        public string Image_Url { get; set; } = string.Empty;
        public Products? Product { get; set; }
    }
}
