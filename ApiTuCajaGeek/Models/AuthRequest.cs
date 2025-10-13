namespace ApiTuCajaGeek.Models
{
    public class AuthRequest
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string? Name { get; set; }
        public string? LastName { get; set; }
        public long? CellNumber { get; set; }
        public Guid? RolId { get; set; }
    }
}
