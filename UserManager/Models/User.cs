using System.ComponentModel.DataAnnotations;
using Swashbuckle.AspNetCore.Annotations;

namespace UserManager.Models
{
    public class User
    {
        [SwaggerSchema(ReadOnly = true, Description = "Unique identifier for the User.")]
        public int Id { get; set; }
        
        [Required(ErrorMessage = "The username is required.")]
        [StringLength(30, MinimumLength = 5, ErrorMessage = "The username must be between {2} and {1} characters.")]
        [SwaggerSchema(Description = "The unique username of the user.")]
        public required string Username { get; set; }

        [EmailAddress(ErrorMessage = "The email is not valid.")]
        [SwaggerSchema(Description = "The email address of the user.")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "The password is required.")]
        [SwaggerSchema(Description = "The password hash of the user.")]
        public required string PasswordHash { get; set; }

        [Required]
        [SwaggerSchema(Description = "The role ID associated with the user.")]
        public required int RoleId { get; set; }

        [SwaggerSchema(Description = "The role associated with the user.")]
        public required Role Role { get; set; }
    }
}
