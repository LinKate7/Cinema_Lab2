using Microsoft.AspNetCore.Mvc;
using Lab2OOP.models;
using Lab2OOP.Services;
using Lab2OOP.DTO;

namespace Lab2OOP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UserService _userService;

        public UsersController(TheatreContext context, UserService userService)
        {
            _userService = userService;
        }

        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetUsers()
        {
            var users = await _userService.GetUsersAsync();
            return Ok(users);
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserDto>> GetUser(Guid id)
        {
            var user = await _userService.GetUserAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        // PUT: api/Users/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(Guid id, UserDto userDto)
        {
            var success = await _userService.UpdateUserAsync(id, userDto);

            if (!success)
            {
                return NotFound();
            }

            return NoContent();
        }

        // POST: api/Users
        [HttpPost]
        public async Task<ActionResult<UserDto>> PostUser(UserDto userDto)
        {
            try
            {
                var createdUser = await _userService.CreateUserAsync(userDto);
                var createdUserDto = _userService.MapToUserDto(createdUser);
                return CreatedAtAction(nameof(GetUser), new { id = createdUser.Id }, createdUserDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }

        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            var success = await _userService.DeleteUserAsync(id);
            if (!success)
            {
                return NotFound();
            }
            return NoContent();
        }

    }
}
