using Microsoft.AspNetCore.Mvc;
using SwipeWords.Data;

namespace SwipeWords.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LeaderboardController : ControllerBase
{
    private readonly UsersDatabaseContext _context;

    public LeaderboardController(UsersDatabaseContext context)
    {
        _context = context;
    }
}