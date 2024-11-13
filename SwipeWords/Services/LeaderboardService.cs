using SwipeWords.Data;
using SwipeWords.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace SwipeWords.Services
{
    public class LeaderboardService
    {
        private readonly UsersDatabaseContext _context;
        private readonly ILogger<LeaderboardService> _logger;

        public LeaderboardService(UsersDatabaseContext context, ILogger<LeaderboardService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Leaderboard> AddOrUpdateScoreAsync(string userName, int score)
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
                    return null;
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

            return leaderboardEntry;
        }

        public async Task<IQueryable<Leaderboard>> GetLeaderboardAsync(int top)
        {
            return _context.Leaderboards
                .OrderByDescending(lb => lb.MaxScore)
                .Take(top);
        }
    }
}