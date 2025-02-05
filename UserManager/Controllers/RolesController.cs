using Microsoft.AspNetCore.Mvc;
using UserManager.Data;
using UserManager.Models;
using UserManager.Models.DTO;
using Microsoft.AspNetCore.Authorization;

namespace UserManager.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class RolesController : ControllerBase
    {
        private readonly IRoleRepository _roleRepository;

        public RolesController(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }

        [HttpGet]
        public IActionResult GetRoles()
        {
            var roles = _roleRepository.GetAllRoles().
            Select(r => new RoleDTO
            {
                Id = r.Id,
                Name = r.Name
            }).ToList();

            return Ok(roles);
        }

        [HttpGet("{id}")]
        public IActionResult GetRole(int id)
        {
            var role = _roleRepository.GetRoleById(id);

            if (role == null)
            {
                return NotFound(new { Message = "Role not found." });
            }

            var roleDto = new RoleDTO
            {
                Id = role.Id,
                Name = role.Name
            };

            return Ok(roleDto);
        }

        [HttpPost]
        public IActionResult CreateRole([FromBody] Role newRole)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _roleRepository.AddRole(newRole);

            return CreatedAtAction(nameof(GetRole), new { id = newRole.Id }, newRole);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateRole(int id, [FromBody] Role updatedRole)
        {
            var role = _roleRepository.GetRoleById(id);

            if (role == null)
            {
                return NotFound(new { Message = "Role not found." });
            }

            role.Name = updatedRole.Name;

            _roleRepository.UpdateRole(updatedRole);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteRole(int id)
        {
            var role = _roleRepository.GetRoleById(id);

            if (role == null)
            {
                return NotFound(new { Message = "Role not found." });
            }

            _roleRepository.DeleteRole(id);

            return NoContent();
        }
    }
}
