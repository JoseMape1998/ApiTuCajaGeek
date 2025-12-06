using System;

namespace ApiTuCajaGeek.Models
{
    public class Shopping_cart
    {
        public long Shopping_cart_Id { get; set; }
        public Guid User_Id { get; set; }
        public long Product_Id { get; set; }
        public decimal Unit_value { get; set; }
        public int Amount { get; set; }
        public DateTime Date_added { get; set; }
        public decimal Subtotal { get; private set; }
        public Users? User { get; set; }
        public Products? Product { get; set; }
    }
}
