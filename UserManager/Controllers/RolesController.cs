using Microsoft.AspNetCore.Mvc;
using UserManager.Data;
using UserManager.Models;

namespace UserManager.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RolesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public RolesController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetRoles()
        {
            var roles = _context.Roles.ToList();
            return Ok(roles);
        }

        [HttpGet("{id}")]
        public IActionResult GetRole(int id)
        {
            var role = _context.Roles.Find(id);

            if (role == null)
            {
                return NotFound(new { Message = "Role not found." });
            }

            return Ok(role);
        }

        [HttpPost]
        public IActionResult CreateRole([FromBody] Role newRole)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Roles.Add(newRole);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetRole), new { id = newRole.Id }, newRole);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateRole(int id, [FromBody] Role updatedRole)
        {
            var role = _context.Roles.Find(id);

            if (role == null)
            {
                return NotFound(new { Message = "Role not found." });
            }

            role.Name = updatedRole.Name;

            _context.SaveChanges();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteRole(int id)
        {
            var role = _context.Roles.Find(id);

            if (role == null)
            {
                return NotFound(new { Message = "Role not found." });
            }

            _context.Roles.Remove(role);
            _context.SaveChanges();

            return NoContent();
        }
    }
}
