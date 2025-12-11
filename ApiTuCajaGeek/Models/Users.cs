using System;
using System.Collections.Generic;

namespace ApiTuCajaGeek.Models
{
    public class Users
    {
        public Guid User_Id { get; set; }
        public string Email_user { get; set; } = string.Empty;
        public string Name_user { get; set; } = string.Empty;
        public string LastName_user { get; set; } = string.Empty;
        public long? Cell_number_user { get; set; }
        public string Password_hash { get; set; } = string.Empty;
        public bool User_state { get; set; }
        public Guid Rol_id { get; set; }
        public Rol? Rol { get; set; }
        public ICollection<User_rating>? User_ratings { get; set; }
        public ICollection<Shopping_cart>? ShoppingCarts { get; set; }
        public Purchase_data? PurchaseData { get; set; }

    }
}
