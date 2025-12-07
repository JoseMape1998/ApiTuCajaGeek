public class ShoppingCartResponseDto
{
    public long Shopping_cart_Id { get; set; }
    public Guid User_Id { get; set; }
    public long Product_Id { get; set; }
    public string Product_name { get; set; } = string.Empty;
    public string Category_name { get; set; } = string.Empty;
    public decimal Unit_value { get; set; }
    public int Amount { get; set; }
    public decimal Subtotal { get; set; }
    public List<string> Product_Urls { get; set; } = new();
}