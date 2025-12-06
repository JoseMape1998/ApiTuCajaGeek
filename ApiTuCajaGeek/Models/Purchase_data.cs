using System;

namespace ApiTuCajaGeek.Models
{
    public class Purchase_data
    {
        public Guid User_Id { get; set; }
        public string Purchase_address { get; set; } = string.Empty;
        public long Purchase_type_Id { get; set; }
        public Users? User { get; set; }
        public Purchase_type? PurchaseType { get; set; }
    }
}
