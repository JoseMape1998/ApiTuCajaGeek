namespace ApiTuCajaGeek.DTOs
{
    public class UserPurchaseResponseDto
    {
        public Guid User_Id { get; set; }
        public string User_Name { get; set; } = string.Empty;
        public string User_Email { get; set; } = string.Empty;
        public string Purchase_Address { get; set; } = string.Empty;
        public string Purchase_Type { get; set; } = string.Empty;
        public List<PurchaseProductDto> Products { get; set; } = new();
    }
}
