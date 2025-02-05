namespace UserManager.Models.DTO
{
    public class UserDTO
    {
        public int Id { get; set;}
        public required string Username { get; set; }
        public string Email { get; set; } = string.Empty;
        public required int RoleId { get; set; }
        public string? RoleName { get; set;}
    }
}
