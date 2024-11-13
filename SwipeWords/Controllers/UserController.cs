using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SwipeWords.Services;
using SwipeWords.Models;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SwipeWords.Data;

namespace SwipeWords.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;
        private readonly TokenProvider _tokenProvider;
        private readonly ILogger<UserController> _logger;

        public UserController(UserService userService, TokenProvider tokenProvider, ILogger<UserController> logger)
        {
            _userService = userService;
            _tokenProvider = tokenProvider;
            _logger = logger;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> PostUser([FromBody] UserDto userDto)
        {
            if (userDto == null)
            {
                return BadRequest();
            }

            if (await _userService.IsUsernameTakenAsync(userDto.Name))
            {
                return Conflict(new { message = "Username is taken" });
            }

            var salt = PasswordHasher.GenerateSalt();
            var user = new User
            {
                UserId = Guid.NewGuid(),
                Name = userDto.Name,
                PasswordSalt = salt,
                PasswordHash = PasswordHasher.HashPassword(userDto.Password, salt)
            };

            // Call the TokenProvider to create a token
            var token = _tokenProvider.Create(user);

            await _userService.AddUserAsync(user);

            return CreatedAtAction(nameof(PostUser), new { token });
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] UserDto userDto)
        {
            if (userDto == null)
            {
                return BadRequest();
            }

            var user = await _userService.GetUserByNameAsync(userDto.Name);
            if (user == null)
            {
                return NotFound();
            }

            var storedSalt = user.PasswordSalt;
            var storedHash = user.PasswordHash;
            if (!PasswordHasher.VerifyPassword(userDto.Password, storedHash, storedSalt))
            {
                return Unauthorized();
            }

            // Call the TokenProvider to create a token
            var token = _tokenProvider.Create(user);

            return Ok(new { token });
        }

        [HttpGet("CheckUsernameAvailability")]
        public async Task<IActionResult> CheckUsernameAvailability([FromQuery] string username)
        {
            var isTaken = await _userService.IsUsernameTakenAsync(username);
            return Ok(new { isAvailable = !isTaken });
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetUserById(Guid id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound(new { message = "User not found" });
            }

            var userDto = new ReturnUserDto
            {
                UserId = user.UserId,
                Name = user.Name
            };

            return Ok(userDto);
        }
    }
}
public class UserDto
{
    public string Name { get; set; }
    public string Password { get; set; }
}

public class ReturnUserDto
{
    public Guid UserId { get; set; }
    public string Name { get; set; }
}