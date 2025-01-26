namespace UserManager.Models
{
    public class Role
    {
        public int Id { get; set; } // Primary key
        public string Name { get; set; } // Role name, e.g., Admin, User
        public ICollection<User> Users { get; set; } // Navigation property
    }
}
