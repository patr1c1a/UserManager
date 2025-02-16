using System.ComponentModel.DataAnnotations;
using Swashbuckle.AspNetCore.Annotations;

namespace UserManager.Models
{
    public class Role
    {
        [SwaggerSchema(ReadOnly = true, Description = "Unique identifier for the Role.")]
        public int Id { get; set; }

        [Required(ErrorMessage = "The role name is required.")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "The role name must be between {2} and {1} characters.")]
        [SwaggerSchema(Description = "The name of the role. Must be between 3 and 50 characters.")]
        public required string Name { get; set; }

        [Required(ErrorMessage = "The users list required (it can be empty).")]
        [SwaggerSchema(ReadOnly = true, Description = "The list of users associated with this role.")]
        public required ICollection<User> Users { get; set; }
    }
}
