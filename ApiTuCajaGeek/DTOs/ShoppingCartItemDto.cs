namespace ApiTuCajaGeek.DTOs
{
    public class ShoppingCartItemDto
    {
        public Guid User_Id { get; set; }
        public long Product_Id { get; set; }
        public int Amount { get; set; }
    }
}
