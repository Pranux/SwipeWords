using Microsoft.AspNetCore.Mvc;
using SwipeWords.Services;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace SwipeWords.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LeaderboardController : ControllerBase
    {
        private readonly ILeaderboardService _leaderboardService;
        private readonly ILogger<LeaderboardController> _logger;

        public LeaderboardController(ILeaderboardService leaderboardService, ILogger<LeaderboardController> logger)
        {
            _leaderboardService = leaderboardService;
            _logger = logger;
        }

        [HttpPost("AddOrUpdateScore")]
        public async Task<IActionResult> AddOrUpdateScore(string userName, int score)
        {
            var leaderboardEntry = await _leaderboardService.AddOrUpdateScoreAsync(userName, score);

            if (leaderboardEntry == null)
            {
                return NotFound(new { message = "User not found." });
            }

            return Ok(new { message = "Score added or updated successfully." });
        }

        [HttpGet("GetLeaderboard")]
        public async Task<IActionResult> GetLeaderboard(int top = 10)
        {
            var leaderboard = (await _leaderboardService.GetLeaderboardAsync(top))
                .Select(lb => new
                {
                    lb.UserName,
                    lb.MaxScore
                })
                .ToList();

            return Ok(leaderboard);
        }
    }
}