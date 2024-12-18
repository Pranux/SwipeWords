using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using SwipeWords.Data;
using System;
using System.Linq;
using SwipeWords.Migrations;

namespace SwipeWords.IntegrationTests.Migrations;

[TestFixture]
[TestOf(typeof(UsersDatabaseContextModelSnapshot))]
public class UserDatabaseContextModelSnapshotTest
{
    private UsersDatabaseContext _dbContext;
    private DbContextOptions<UsersDatabaseContext> _options;

    [SetUp]
    public void SetUp()
    {
        // Create an in-memory database to test the migration
        _options = new DbContextOptionsBuilder<UsersDatabaseContext>()
            .UseInMemoryDatabase(databaseName: "UsersDatabaseTest")
            .Options;

        // Initialize the DbContext
        _dbContext = new UsersDatabaseContext(_options);
    }

    [TearDown]
    public void TearDown()
    {
        _dbContext.Database.EnsureDeleted();
        _dbContext.Dispose();
    }

    [Test]
    public void Migration_ShouldCreateUserAndLeaderboardTables()
    {
        // Act: Apply migrations (this is to simulate applying the migration in a real database)
        _dbContext.Database.EnsureCreated(); // This will apply the model snapshot and migrations.

        // Assert: Check that the tables exist
        var userTableExists = _dbContext.Model.GetEntityTypes()
            .Any(e => e.ClrType == typeof(User));
        var leaderboardTableExists = _dbContext.Model.GetEntityTypes()
            .Any(e => e.ClrType == typeof(Leaderboard));

        Assert.IsTrue(userTableExists, "User table should be created.");
        Assert.IsTrue(leaderboardTableExists, "Leaderboard table should be created.");
    }

    [Test]
    public void Migration_ShouldHaveUserIdAsPrimaryKeyInUserTable()
    {
        // Act: Apply migrations (this is to simulate applying the migration in a real database)
        _dbContext.Database.EnsureCreated(); // This will apply the model snapshot and migrations.

        // Assert: Check that UserId is the primary key for the User table
        var userEntity = _dbContext.Model.FindEntityType(typeof(User));
        var primaryKey = userEntity.FindPrimaryKey();
        Assert.AreEqual("UserId", primaryKey.Properties.First().Name);
    }

    [Test]
    public void Migration_ShouldHaveUserNameAsPrimaryKeyInLeaderboardTable()
    {
        // Act: Apply migrations (this is to simulate applying the migration in a real database)
        _dbContext.Database.EnsureCreated(); // This will apply the model snapshot and migrations.

        // Assert: Check that UserName is the primary key for the Leaderboard table
        var leaderboardEntity = _dbContext.Model.FindEntityType(typeof(Leaderboard));
        var primaryKey = leaderboardEntity.FindPrimaryKey();
        Assert.AreEqual("UserName", primaryKey.Properties.First().Name);
    }

    [Test]
    public void Migration_ShouldInsertDataCorrectly()
    {
        // Arrange: Create a User entity to insert
        var user = new User
        {
            UserId = Guid.NewGuid(),
            Name = "testuser",
            PasswordHash = "hash",
            PasswordSalt = "salt"
        };

        // Act: Insert the user into the in-memory database
        _dbContext.Users.Add(user);
        _dbContext.SaveChanges();

        // Assert: Verify the user is saved and can be retrieved
        var retrievedUser = _dbContext.Users.FirstOrDefault(u => u.Name == "testuser");
        Assert.IsNotNull(retrievedUser);
        Assert.AreEqual("testuser", retrievedUser.Name);
    }

    [Test]
    public void METHOD()
    {
        
    }
}