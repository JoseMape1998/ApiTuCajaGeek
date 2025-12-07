using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiTuCajaGeek.Models
{
    [Table("Purchase_data")]
    public class Purchase_data
    {
        [Column("User_Id")]
        public Guid UserId { get; set; }

        [Column("Purchase_address")]
        public string PurchaseAddress { get; set; } = null!;

        [Column("Purchase_type_Id")]
        public long PurchaseTypeId { get; set; }

        [Column("Unit_value")]
        public decimal? UnitValue { get; set; }

        [Column("Product_Id")]
        public long? ProductId { get; set; }

        [Column("Purchase_State")]
        public bool? PurchaseState { get; set; }

        public virtual Products? Product { get; set; }

        public virtual Purchase_type PurchaseType { get; set; } = null!;

        public virtual Users User { get; set; } = null!;
    }
}
