using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiTuCajaGeek.Models
{
    [Table("Users")]
    public class User
    {
        [Key]
        [Column("User_Id")]
        public Guid UserId { get; set; }

        [Column("Email_user")]
        [Required, StringLength(100)]
        public string EmailUser { get; set; } = string.Empty;

        [Column("Name_user")]
        [Required, StringLength(100)]
        public string NameUser { get; set; } = string.Empty;

        [Column("LastName_user")]
        [Required, StringLength(300)]
        public string LastNameUser { get; set; } = string.Empty;

        [Column("Cell_number_user")]
        public long? CellNumberUser { get; set; }

        [Column("Password_hash")]
        [Required]
        public string PasswordHash { get; set; } = string.Empty;

        [Column("User_state")]
        public bool UserState { get; set; }

        [Column("Rol_id")]
        public Guid RolId { get; set; }
    }
}
