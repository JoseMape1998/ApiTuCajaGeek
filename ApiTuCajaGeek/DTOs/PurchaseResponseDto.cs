namespace ApiTuCajaGeek.DTOs
{
    public class PurchaseResponseDto
    {
        public long Purchase_Id { get; set; }
        public Guid User_Id { get; set; }
        public string Purchase_type { get; set; } = string.Empty;
        public string Purchase_address { get; set; } = string.Empty;
        public bool Purchase_state { get; set; }
        public PurchaseProductDto Product { get; set; } = new();
    }
}


