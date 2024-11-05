using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SwipeWords.Data;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SwipeWords.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LeaderboardController : ControllerBase
    {
        private readonly UsersDatabaseContext _context;

        public LeaderboardController(UsersDatabaseContext context)
        {
            _context = context;
        }
        
        [HttpPost("AddOrUpdateScore")]
        public async Task<IActionResult> AddOrUpdateScore(string userName, int score)
        {
            var leaderboardEntry = await _context.Leaderboards
                .FirstOrDefaultAsync(lb => lb.UserName == userName);

            if (leaderboardEntry != null)
            {
                if (score > leaderboardEntry.MaxScore)
                {
                    leaderboardEntry.MaxScore = score;
                    await _context.SaveChangesAsync();
                }
            }
            else
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Name == userName);
                if (user == null)
                {
                    return NotFound(new { message = "User not found." });
                }

                leaderboardEntry = new Leaderboard
                {
                    UserName = userName,
                    UserId = user.UserId,
                    MaxScore = score
                };

                _context.Leaderboards.Add(leaderboardEntry);
                await _context.SaveChangesAsync();
            }

            return Ok(new { message = "Score added or updated successfully." });
        }
        
        [HttpGet("GetLeaderboard")]
        public async Task<IActionResult> GetLeaderboard(int top = 10)
        {
            var leaderboard = await _context.Leaderboards
                .OrderByDescending(lb => lb.MaxScore)
                .Take(top)
                .Select(lb => new
                {
                    lb.UserName,
                    lb.MaxScore
                })
                .ToListAsync();

            return Ok(leaderboard);
        }
    }
}
