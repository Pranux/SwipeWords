using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SwipeWords.Data;
using SwipeWords.Models;

namespace SwipeWords.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly UsersDatabaseContext _context;
    private readonly TokenProvider _tokenProvider;

    public UserController(UsersDatabaseContext context, TokenProvider tokenProvider)
    {
        _context = context;
        _tokenProvider = tokenProvider;
    }

    [HttpPost("Register")]
    public async Task<IActionResult> PostUser([FromBody] UserDto userDto)
    {
        if (userDto == null)
        {
            return BadRequest();
        }

        if (!await IsUsernameTaken(userDto.Name))
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

        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        

        return CreatedAtAction(nameof(PostUser),  new { token });
    }

    [HttpPost("Login")]
    public async Task<IActionResult> Login([FromBody] UserDto userDto)
    {
        if (userDto == null)
        {
            return BadRequest();
        }

        var user = await _context.Users.FirstOrDefaultAsync(u => u.Name == userDto.Name);
        if (user == null)
        {
            return NotFound();
        }

        var storedSalt = await GetStoredSalt(userDto.Name);
        var storedHash = await GetStoredHash(userDto.Name);
        if (!PasswordHasher.VerifyPassword(userDto.Password, storedHash, storedSalt))
        {
            return Unauthorized();
        }

        // Call the TokenProvider to create a token
        var token = _tokenProvider.Create(user);

        return Ok(new {token});
    }

    [HttpGet("CheckUsernameAvailability")]
    [Authorize]
    public async Task<IActionResult> CheckUsernameAvailability([FromQuery] string username)
    {
        var isTaken = await _context.Users.AnyAsync(u => u.Name == username);
        return Ok(new { isAvailable = !isTaken });
    }

    [HttpGet("{id}")]
    [Authorize]
    public async Task<IActionResult> GetUserById(Guid id)
    {
        var user = await _context.Users.FindAsync(id);
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

    private async Task<string> GetStoredSalt(string username)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Name == username);
        return user?.PasswordSalt;
    }

    private async Task<string> GetStoredHash(string username)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Name == username);
        return user?.PasswordHash;
    }

    private async Task<bool> IsUsernameTaken(string username)
    {
        return !await _context.Users.AnyAsync(u => u.Name == username);
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