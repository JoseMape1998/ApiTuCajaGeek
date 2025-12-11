namespace ApiTuCajaGeek.DTOs
{
    public class ProcessPurchaseRequestDto
    {
        public long Purchase_type_Id { get; set; }
        public string Purchase_address { get; set; } = string.Empty;
    }
}
