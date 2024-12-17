using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using SwipeWords.Data;
using SwipeWords.Migrations.FlashcardGameDatabase;

namespace SwipeWords.IntegrationTests.Migrations
{
    [TestFixture]
    [TestOf(typeof(InitialRemote))]
    public class InitialRemoteTest
    {
        private DbContextOptions<UsersDatabaseContext> _options;

        [SetUp]
        public void Setup()
        {
            // Configure an in-memory database for testing migrations
            _options = new DbContextOptionsBuilder<UsersDatabaseContext>()
                .UseInMemoryDatabase(databaseName: "UserTestDatabase")
                .Options;
        }

        [Test]
        public void Migration_CreatesRequiredTables()
        {
            // Arrange & Act: Create the database context which will apply migrations
            using (var context = new UsersDatabaseContext(_options))
            {
                // Ensure the database is created and all migrations are applied
                context.Database.EnsureCreated();

                // Assert: Verify Users table exists and has expected properties
                var usersTableExists = context.Model.FindEntityType(typeof(User)) != null;
                Assert.That(usersTableExists, Is.True, "Users table should exist after migration");

                var userProperties = context.Model.FindEntityType(typeof(User))?.GetProperties();
                Assert.That(userProperties, Is.Not.Null);
                Assert.That(userProperties.Any(p => p.Name == "UserId"), Is.True, "UserId property should exist");
                Assert.That(userProperties.Any(p => p.Name == "Name"), Is.True, "Name property should exist");
                Assert.That(userProperties.Any(p => p.Name == "PasswordHash"), Is.True, "PasswordHash property should exist");
                Assert.That(userProperties.Any(p => p.Name == "PasswordSalt"), Is.True, "PasswordSalt property should exist");

                // Assert: Verify Leaderboards table exists and has expected properties
                var leaderboardsTableExists = context.Model.FindEntityType(typeof(Leaderboard)) != null;
                Assert.That(leaderboardsTableExists, Is.True, "Leaderboards table should exist after migration");

                var leaderboardProperties = context.Model.FindEntityType(typeof(Leaderboard))?.GetProperties();
                Assert.That(leaderboardProperties, Is.Not.Null);
                Assert.That(leaderboardProperties.Any(p => p.Name == "UserName"), Is.True, "UserName property should exist");
                Assert.That(leaderboardProperties.Any(p => p.Name == "MaxScore"), Is.True, "MaxScore property should exist");
                Assert.That(leaderboardProperties.Any(p => p.Name == "UserId"), Is.True, "UserId property should exist");
            }
        }

        [Test]
        public void Migration_PrimaryKeysAreCorrectlyConfigured()
        {
            // Arrange & Act
            using (var context = new UsersDatabaseContext(_options))
            {
                context.Database.EnsureCreated();

                // Assert: Verify primary keys
                var userPrimaryKey = context.Model.FindEntityType(typeof(User))?.FindPrimaryKey();
                Assert.That(userPrimaryKey, Is.Not.Null);
                Assert.That(userPrimaryKey.Properties.First().Name, Is.EqualTo("UserId"));

                var leaderboardPrimaryKey = context.Model.FindEntityType(typeof(Leaderboard))?.FindPrimaryKey();
                Assert.That(leaderboardPrimaryKey, Is.Not.Null);
                Assert.That(leaderboardPrimaryKey.Properties.First().Name, Is.EqualTo("UserName"));
            }
        }

        [Test]
        public void CanAddAndRetrieveUsers()
        {
            // Arrange
            using (var context = new UsersDatabaseContext(_options))
            {
                context.Database.EnsureCreated();

                // Act
                var testUser = new User
                {
                    UserId = Guid.NewGuid(),
                    Name = "TestUser",
                    PasswordHash = "TestHash",
                    PasswordSalt = "TestSalt"
                };

                context.Users.Add(testUser);
                context.SaveChanges();

                // Assert
                var retrievedUser = context.Users.FirstOrDefault(u => u.Name == "TestUser");
                Assert.That(retrievedUser, Is.Not.Null);
                Assert.That(retrievedUser.Name, Is.EqualTo("TestUser"));
                Assert.That(retrievedUser.PasswordHash, Is.EqualTo("TestHash"));
            }
        }

        [Test]
        public void CanAddAndRetrieveLeaderboards()
        {
            // Arrange
            using (var context = new UsersDatabaseContext(_options))
            {
                context.Database.EnsureCreated();

                // Act
                var testLeaderboard = new Leaderboard
                {
                    UserName = "TestUser",
                    MaxScore = 100,
                    UserId = Guid.NewGuid()
                };

                context.Leaderboards.Add(testLeaderboard);
                context.SaveChanges();

                // Assert
                var retrievedLeaderboard = context.Leaderboards.FirstOrDefault(l => l.UserName == "TestUser");
                Assert.That(retrievedLeaderboard, Is.Not.Null);
                Assert.That(retrievedLeaderboard.MaxScore, Is.EqualTo(100));
            }
        }

        [TearDown]
        public void TearDown()
        {
            // Cleanup: Ensure the in-memory database is deleted after each test
            using (var context = new UsersDatabaseContext(_options))
            {
                context.Database.EnsureDeleted();
            }
        }
    }
}