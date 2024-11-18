using Microsoft.EntityFrameworkCore;
using SwipeWords.Data;

namespace SwipeWords.IntegrationTests.Data;

[TestFixture]
[TestOf(typeof(Leaderboard))]
public class LeaderboardIntegrationTest
{
    private UsersDatabaseContext _dbContext;

    [SetUp]
    public void SetUp()
    {
        var options = new DbContextOptionsBuilder<UsersDatabaseContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;

        _dbContext = new UsersDatabaseContext(options);
        _dbContext.Database.EnsureCreated();
    }

    // Dispose the database context after each test
    [TearDown]
    public void TearDown()
    {
        _dbContext.Database.EnsureDeleted();
        _dbContext.Dispose();
    }

    [Test]
    public async Task AddLeaderboardEntry_ShouldSaveToDatabase()
    {
        var leaderboardEntry = new Leaderboard
        {
            UserName = "testUser",
            MaxScore = 100,
            UserId = Guid.NewGuid()
        };
        
        await _dbContext.AddAsync(leaderboardEntry);
        await _dbContext.SaveChangesAsync();
        
        var savedEntry = await _dbContext.Set<Leaderboard>()
            .FirstOrDefaultAsync(l => l.UserName == leaderboardEntry.UserName);
        Assert.IsNotNull(savedEntry);
        Assert.AreEqual(leaderboardEntry.MaxScore, savedEntry.MaxScore);
        Assert.AreEqual(leaderboardEntry.UserId, savedEntry.UserId);
    }

    [Test]
    public async Task GetLeaderboardEntry_ShouldRetrieveFromDatabase()
    {
        var leaderboardEntry = new Leaderboard
        {
            UserName = "testUser",
            MaxScore = 100,
            UserId = Guid.NewGuid()
        };
        await _dbContext.AddAsync(leaderboardEntry);
        await _dbContext.SaveChangesAsync();
        
        var retrievedEntry = await _dbContext.Set<Leaderboard>()
            .FirstOrDefaultAsync(l => l.UserName == leaderboardEntry.UserName);
        
        Assert.IsNotNull(retrievedEntry);
        Assert.AreEqual(leaderboardEntry.UserName, retrievedEntry.UserName);
        Assert.AreEqual(leaderboardEntry.MaxScore, retrievedEntry.MaxScore);
        Assert.AreEqual(leaderboardEntry.UserId, retrievedEntry.UserId);
    }

    [Test]
    public async Task UpdateLeaderboardEntry_ShouldPersistChanges()
    {
        var leaderboardEntry = new Leaderboard
        {
            UserName = "testUser",
            MaxScore = 100,
            UserId = Guid.NewGuid()
        };
        await _dbContext.AddAsync(leaderboardEntry);
        await _dbContext.SaveChangesAsync();
        
        leaderboardEntry.MaxScore = 200;
        _dbContext.Update(leaderboardEntry);
        await _dbContext.SaveChangesAsync();
        
        var updatedEntry = await _dbContext.Set<Leaderboard>()
            .FirstOrDefaultAsync(l => l.UserName == leaderboardEntry.UserName);
        Assert.IsNotNull(updatedEntry);
        Assert.AreEqual(200, updatedEntry.MaxScore);
    }

    [Test]
    public async Task DeleteLeaderboardEntry_ShouldRemoveFromDatabase()
    {
        var leaderboardEntry = new Leaderboard
        {
            UserName = "testUser",
            MaxScore = 100,
            UserId = Guid.NewGuid()
        };
        await _dbContext.AddAsync(leaderboardEntry);
        await _dbContext.SaveChangesAsync();
        
        _dbContext.Remove(leaderboardEntry);
        await _dbContext.SaveChangesAsync();
        
        var deletedEntry = await _dbContext.Set<Leaderboard>()
            .FirstOrDefaultAsync(l => l.UserName == leaderboardEntry.UserName);
        Assert.IsNull(deletedEntry);
    }
}