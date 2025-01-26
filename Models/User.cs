namespace UserManager.Models
{
    public class User
    {
        public int Id { get; set; }
        public required string Username { get; set; }
        public string? Email { get; set; }
        public required string PasswordHash { get; set; }
        public int RoleId { get; set; }
        public required Role Role { get; set; }
    }
}
