using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using SwipeWords.Controllers;
using SwipeWords.Data;
using SwipeWords.Services;
using System.Text.Json;

namespace SwipeWords.UnitTests.Controllers;

[TestFixture]
[TestOf(typeof(LeaderboardController))]
public class LeaderboardControllerTests
{
    private Mock<ILeaderboardService> _mockLeaderboardService;
    private Mock<ILogger<LeaderboardController>> _mockLogger;
    private LeaderboardController _controller;

    [SetUp]
    public void SetUp()
    {
        _mockLeaderboardService = new Mock<ILeaderboardService>();
        _mockLogger = new Mock<ILogger<LeaderboardController>>();
        _controller = new LeaderboardController(_mockLeaderboardService.Object, _mockLogger.Object);
    }

    [Test]
    public async Task AddOrUpdateScore_ReturnsOkResult_WhenScoreUpdated()
    {
        var userName = "testUser";
        var score = 100;
        var leaderboardEntry = new Leaderboard
        {
            UserName = userName,
            MaxScore = score,
            UserId = Guid.NewGuid()
        };

        _mockLeaderboardService
            .Setup(service => service.AddOrUpdateScoreAsync(userName, score))
            .ReturnsAsync(leaderboardEntry);
        
        var result = await _controller.AddOrUpdateScore(userName, score);
        
        Assert.That(result, Is.TypeOf<OkObjectResult>());
        var okResult = result as OkObjectResult;
        var resultDict = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(
            JsonSerializer.Serialize(okResult.Value)
        );
        
        Assert.That(resultDict["message"].GetString(), Is.EqualTo("Score added or updated successfully."));
    }

    [Test]
    public async Task AddOrUpdateScore_ReturnsNotFound_WhenUserDoesNotExist()
    {
        var userName = "nonexistentUser";
        var score = 100;

        _mockLeaderboardService
            .Setup(service => service.AddOrUpdateScoreAsync(userName, score))
            .ReturnsAsync((Leaderboard)null);
        
        var result = await _controller.AddOrUpdateScore(userName, score);
        
        Assert.That(result, Is.TypeOf<NotFoundObjectResult>());
        var notFoundResult = result as NotFoundObjectResult;
        var resultDict = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(
            JsonSerializer.Serialize(notFoundResult.Value)
        );
        
        Assert.That(resultDict["message"].GetString(), Is.EqualTo("User not found."));
    }

    [Test]
    public async Task GetLeaderboard_ReturnsOkResult_WithLeaderboardEntries()
    {
        var top = 3;
        var userId1 = Guid.NewGuid();
        var userId2 = Guid.NewGuid();
        var userId3 = Guid.NewGuid();
        
        var leaderboardEntries = new List<Leaderboard>
        {
            new() { UserName = "user1", MaxScore = 100, UserId = userId1 },
            new() { UserName = "user2", MaxScore = 90, UserId = userId2 },
            new() { UserName = "user3", MaxScore = 80, UserId = userId3 }
        }.AsQueryable();

        _mockLeaderboardService
            .Setup(service => service.GetLeaderboardAsync(top))
            .ReturnsAsync(leaderboardEntries);
        
        var result = await _controller.GetLeaderboard(top);
        
        Assert.That(result, Is.TypeOf<OkObjectResult>());
        var okResult = result as OkObjectResult;
        var leaderboard = JsonSerializer.Deserialize<List<Dictionary<string, JsonElement>>>(
            JsonSerializer.Serialize(okResult.Value)
        );
        
        Assert.That(leaderboard.Count, Is.EqualTo(3));
        
        var firstEntry = leaderboard[0];
        Assert.That(firstEntry["UserName"].GetString(), Is.EqualTo("user1"));
        Assert.That(firstEntry["MaxScore"].GetInt32(), Is.EqualTo(100));
    }

    [Test]
    public async Task GetLeaderboard_ReturnsOkResult_WithDefaultTopValue()
    {
        var defaultTop = 10;
        var userId1 = Guid.NewGuid();
        var userId2 = Guid.NewGuid();
        
        var leaderboardEntries = new List<Leaderboard>
        {
            new() { UserName = "user1", MaxScore = 100, UserId = userId1 },
            new() { UserName = "user2", MaxScore = 90, UserId = userId2 }
        }.AsQueryable();

        _mockLeaderboardService
            .Setup(service => service.GetLeaderboardAsync(defaultTop))
            .ReturnsAsync(leaderboardEntries);
        
        var result = await _controller.GetLeaderboard();
        
        Assert.That(result, Is.TypeOf<OkObjectResult>());
        var okResult = result as OkObjectResult;
        var leaderboard = JsonSerializer.Deserialize<List<Dictionary<string, JsonElement>>>(
            JsonSerializer.Serialize(okResult.Value)
        );
        
        Assert.That(leaderboard.Count, Is.EqualTo(2));
        
        var firstEntry = leaderboard[0];
        Assert.That(firstEntry["UserName"].GetString(), Is.EqualTo("user1"));
        Assert.That(firstEntry["MaxScore"].GetInt32(), Is.EqualTo(100));
    }
}