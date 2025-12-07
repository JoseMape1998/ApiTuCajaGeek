namespace ApiTuCajaGeek.DTOs
{
    public class UserDetailDto
    {
        public Guid UserId { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public long? CellNumber { get; set; }
        public bool UserState { get; set; }

        public RoleDto? Rol { get; set; }

        public PurchaseProductDto? PurchaseData { get; set; }
        public List<ShoppingCartItemDto>? CartItems { get; set; }
    }
}
