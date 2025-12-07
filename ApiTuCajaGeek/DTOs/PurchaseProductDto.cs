namespace ApiTuCajaGeek.DTOs
{
    public class PurchaseProductDto
    {
        public long Product_Id { get; set; }
        public string Product_name { get; set; } = string.Empty;
        public string Category_name { get; set; } = string.Empty;
        public decimal Unit_value { get; set; }
        public List<string> Images { get; set; } = new();
    }
}
