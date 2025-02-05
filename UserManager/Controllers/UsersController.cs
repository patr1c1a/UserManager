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
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        public UsersController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpGet]
        public IActionResult GetUsers()
        {
            var users = _userRepository.GetAllUsers()
            .Select(u => new UserDTO
            {
                Id = u.Id,
                Username = u.Username,
                Email = u.Email ?? "",
                RoleId = u.RoleId,
                RoleName = u.Role?.Name ?? "Unknown"
            }).ToList();

            return Ok(users);
        }

        [HttpGet("{id}")]
        public IActionResult GetUser(int id)
        {
            var user = _userRepository.GetUserById(id);

            if (user == null)
            {
                return NotFound(new { Message = "User not found." });
            }

            var userDto = new UserDTO
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email ?? "",
                RoleId = user.RoleId,
                RoleName = user.Role?.Name ?? "Unknown"
            };

            return Ok(userDto);
        }

        [HttpPost]
        public IActionResult CreateUser([FromBody] User newUser)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = new User
            {
                Username = newUser.Username,
                Email = newUser.Email,
                RoleId = newUser.RoleId,
                Password = newUser.Password
            };

            _userRepository.AddUser(user);

            return CreatedAtAction(nameof(GetUser), new { id = user.Id }, newUser);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateUser(int id, [FromBody] User updatedUser)
        {
            var user = _userRepository.GetUserById(id);

            if (user == null)
            {
                return NotFound(new { Message = "User not found." });
            }

            user.Username = updatedUser.Username;
            user.Password = updatedUser.Password;
            user.RoleId = updatedUser.RoleId;

            _userRepository.UpdateUser(user);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteUser(int id)
        {
            var user = _userRepository.GetUserById(id);

            if (user == null)
            {
                return NotFound(new { Message = "User not found." });
            }

            _userRepository.DeleteUser(id);

            return NoContent();
        }
    }
}
