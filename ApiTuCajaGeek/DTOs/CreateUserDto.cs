namespace ApiTuCajaGeek.DTOs
{
    public class CreateUserDto
    {
        public string Name_user { get; set; } = null!;
        public string LastName_user { get; set; } = null!;
        public string Email_user { get; set; } = null!;
        public long Cell_number_user { get; set; }     // bigint

        public Guid Rol_Id { get; set; }               // GUID del rol
        public string Password { get; set; } = null!;
    }
}
