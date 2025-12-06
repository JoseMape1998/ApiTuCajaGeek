using System;

namespace ApiTuCajaGeek.Models
{
    public class User_rating
    {
        public long Rating_Id { get; set; }
        public long Product_Id { get; set; }
        public Guid User_Id { get; set; }
        public int Rating { get; set; }
        public string? Purchase_comment { get; set; }
        public Users? User { get; set; }
        public Products? Product { get; set; }
        public ICollection<Images_rating>? Images { get; set; }
    }
}
