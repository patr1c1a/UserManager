using Microsoft.AspNetCore.Mvc;
using UserManager.Data;
using UserManager.Models;

namespace UserManager.Controllers
{
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
            var users = _userRepository.GetAllUsers();
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

            return Ok(user);
        }

        [HttpPost]
        public IActionResult CreateUser([FromBody] User userDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = new User
            {
                Username = userDto.Username,
                Email = userDto.Email,
                RoleId = userDto.RoleId,
                Password = userDto.Password
            };

            _userRepository.AddUser(userDto);

            return CreatedAtAction(nameof(GetUser), new { id = userDto.Id }, userDto);
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
