using System.Collections.Generic;

namespace ApiTuCajaGeek.Models
{
    public class Purchase_type
    {
        public long Purchase_type_Id { get; set; }
        public string Name_purchase_type { get; set; } = string.Empty;
        public bool Requires_card_information { get; set; }
        public ICollection<Purchase_data>? PurchaseDatas { get; set; }
    }
}
