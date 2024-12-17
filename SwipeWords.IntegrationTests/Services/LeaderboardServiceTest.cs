using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SwipeWords.Data;
using SwipeWords.Models;
using SwipeWords.Services;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SwipeWords.IntegrationTests.Services
{
    [TestFixture]
    public class LeaderboardServiceIntegrationTest
    {
        private UsersDatabaseContext _dbContext;
        private Mock<ILogger<LeaderboardService>> _mockLogger;
        private LeaderboardService _leaderboardService;

        [SetUp]
        public void SetUp()
        {
            // Set up an in-memory database
            var options = new DbContextOptionsBuilder<UsersDatabaseContext>()
                .UseInMemoryDatabase(databaseName: "LeaderboardServiceTestDb")
                .Options;

            _dbContext = new UsersDatabaseContext(options);
            _dbContext.Database.EnsureCreated();

            // Mock the logger
            _mockLogger = new Mock<ILogger<LeaderboardService>>();

            // Initialize the LeaderboardService
            _leaderboardService = new LeaderboardService(_dbContext, _mockLogger.Object);
        }

        [TearDown]
        public void TearDown()
        {
            _dbContext.Database.EnsureDeleted();
            _dbContext.Dispose();
        }

        [Test]
        public async Task AddOrUpdateScoreAsync_NewUser_ShouldAddToLeaderboard()
        {
            // Arrange
            var user = new User
            {
                UserId = Guid.NewGuid(),
                Name = "test_user",
                PasswordHash = "dummyHash",
                PasswordSalt = "dummySalt"
            };
            _dbContext.Users.Add(user);
            await _dbContext.SaveChangesAsync();

            // Act
            var leaderboardEntry = await _leaderboardService.AddOrUpdateScoreAsync(user.Name, 50);

            // Assert
            Assert.IsNotNull(leaderboardEntry);
            Assert.AreEqual(user.UserId, leaderboardEntry.UserId);
            Assert.AreEqual(50, leaderboardEntry.MaxScore);

            var savedEntry = await _dbContext.Leaderboards.FirstOrDefaultAsync(lb => lb.UserName == user.Name);
            Assert.IsNotNull(savedEntry);
            Assert.AreEqual(50, savedEntry.MaxScore);
        }

        [Test]
        public async Task AddOrUpdateScoreAsync_ExistingUserWithHigherScore_ShouldUpdateScore()
        {
            // Arrange
            var user = new User
            {
                UserId = Guid.NewGuid(),
                Name = "test_user",
                PasswordHash = "dummyHash",
                PasswordSalt = "dummySalt"
            };
            _dbContext.Users.Add(user);
            _dbContext.Leaderboards.Add(new Leaderboard
            {
                UserName = user.Name,
                UserId = user.UserId,
                MaxScore = 30
            });
            await _dbContext.SaveChangesAsync();

            // Act
            var leaderboardEntry = await _leaderboardService.AddOrUpdateScoreAsync(user.Name, 50);

            // Assert
            Assert.IsNotNull(leaderboardEntry);
            Assert.AreEqual(50, leaderboardEntry.MaxScore);

            var updatedEntry = await _dbContext.Leaderboards.FirstOrDefaultAsync(lb => lb.UserName == user.Name);
            Assert.AreEqual(50, updatedEntry.MaxScore);
        }

        [Test]
        public async Task AddOrUpdateScoreAsync_ExistingUserWithLowerScore_ShouldNotUpdateScore()
        {
            // Arrange
            var user = new User
            {
                UserId = Guid.NewGuid(),
                Name = "test_user",
                PasswordHash = "dummyHash",
                PasswordSalt = "dummySalt"
            };
            _dbContext.Users.Add(user);
            _dbContext.Leaderboards.Add(new Leaderboard
            {
                UserName = user.Name,
                UserId = user.UserId,
                MaxScore = 50
            });
            await _dbContext.SaveChangesAsync();

            // Act
            var leaderboardEntry = await _leaderboardService.AddOrUpdateScoreAsync(user.Name, 30);

            // Assert
            Assert.IsNotNull(leaderboardEntry);
            Assert.AreEqual(50, leaderboardEntry.MaxScore);

            var unchangedEntry = await _dbContext.Leaderboards.FirstOrDefaultAsync(lb => lb.UserName == user.Name);
            Assert.AreEqual(50, unchangedEntry.MaxScore);
        }

        [Test]
        public async Task GetLeaderboardAsync_ShouldReturnTopScores()
        {
            // Arrange
            var users = new[]
            {
                new User { UserId = Guid.NewGuid(), Name = "user1", PasswordHash = "hash1", PasswordSalt = "salt1" },
                new User { UserId = Guid.NewGuid(), Name = "user2", PasswordHash = "hash2", PasswordSalt = "salt2" },
                new User { UserId = Guid.NewGuid(), Name = "user3", PasswordHash = "hash3", PasswordSalt = "salt3" }
            };

            var leaderboardEntries = new[]
            {
                new Leaderboard { UserId = users[0].UserId, UserName = users[0].Name, MaxScore = 40 },
                new Leaderboard { UserId = users[1].UserId, UserName = users[1].Name, MaxScore = 60 },
                new Leaderboard { UserId = users[2].UserId, UserName = users[2].Name, MaxScore = 50 }
            };

            _dbContext.Users.AddRange(users);
            _dbContext.Leaderboards.AddRange(leaderboardEntries);
            await _dbContext.SaveChangesAsync();

            // Act
            var topScores = await _leaderboardService.GetLeaderboardAsync(2);

            // Assert
            Assert.AreEqual(2, topScores.Count());

            var topScoresList = topScores.ToList();
            Assert.AreEqual("user2", topScoresList[0].UserName);
            Assert.AreEqual(60, topScoresList[0].MaxScore);
            Assert.AreEqual("user3", topScoresList[1].UserName);
            Assert.AreEqual(50, topScoresList[1].MaxScore);
        }
    }
}
