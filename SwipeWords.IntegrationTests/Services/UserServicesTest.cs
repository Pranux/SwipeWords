using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SwipeWords.Data;
using SwipeWords.Models;
using SwipeWords.Services;
using System;
using System.Threading.Tasks;

namespace SwipeWords.IntegrationTests.Services
{
    [TestFixture]
    public class UserServiceIntegrationTest
    {
        private UsersDatabaseContext _dbContext;
        private Mock<ILogger<UserService>> _mockLogger;
        private UserService _userService;

        [SetUp]
        public void SetUp()
        {
            // Set up an in-memory database
            var options = new DbContextOptionsBuilder<UsersDatabaseContext>()
                .UseInMemoryDatabase(databaseName: "UserServiceTestDb")
                .Options;

            _dbContext = new UsersDatabaseContext(options);
            _dbContext.Database.EnsureCreated();

            // Mock the logger
            _mockLogger = new Mock<ILogger<UserService>>();

            // Initialize the UserService
            _userService = new UserService(_dbContext, _mockLogger.Object);
        }

        [TearDown]
        public void TearDown()
        {
            _dbContext.Database.EnsureDeleted();
            _dbContext.Dispose();
        }

        [Test]
        public async Task AddUserAsync_ShouldAddUserToDatabase()
        {
            // Arrange
            var user = new User
            {
                UserId = Guid.NewGuid(),
                Name = "test_user",
                PasswordHash = "dummyHash",
                PasswordSalt = "dummySalt"
            };

            // Act
            await _userService.AddUserAsync(user);

            // Assert
            var savedUser = await _dbContext.Users.FindAsync(user.UserId);
            Assert.IsNotNull(savedUser);
            Assert.AreEqual(user.Name, savedUser.Name);
        }

        [Test]
        public async Task IsUsernameTakenAsync_UsernameNotTaken_ShouldReturnFalse()
        {
            // Arrange
            var username = "new_user";

            // Act
            var isTaken = await _userService.IsUsernameTakenAsync(username);

            // Assert
            Assert.IsFalse(isTaken);
        }

        [Test]
        public async Task IsUsernameTakenAsync_UsernameTaken_ShouldReturnTrue()
        {
            // Arrange
            var user = new User
            {
                UserId = Guid.NewGuid(),
                Name = "existing_user",
                PasswordHash = "dummyHash",
                PasswordSalt = "dummySalt"
            };
            await _userService.AddUserAsync(user);

            // Act
            var isTaken = await _userService.IsUsernameTakenAsync(user.Name);

            // Assert
            Assert.IsTrue(isTaken);
        }

        [Test]
        public async Task GetUserByNameAsync_UserExists_ShouldReturnUser()
        {
            // Arrange
            var user = new User
            {
                UserId = Guid.NewGuid(),
                Name = "existing_user",
                PasswordHash = "dummyHash",
                PasswordSalt = "dummySalt"
            };
            await _userService.AddUserAsync(user);

            // Act
            var foundUser = await _userService.GetUserByNameAsync(user.Name);

            // Assert
            Assert.IsNotNull(foundUser);
            Assert.AreEqual(user.Name, foundUser.Name);
        }

        [Test]
        public async Task GetUserByNameAsync_UserDoesNotExist_ShouldReturnNull()
        {
            // Act
            var foundUser = await _userService.GetUserByNameAsync("non_existent_user");

            // Assert
            Assert.IsNull(foundUser);
        }

        [Test]
        public async Task GetUserByIdAsync_UserExists_ShouldReturnUser()
        {
            // Arrange
            var user = new User
            {
                UserId = Guid.NewGuid(),
                Name = "existing_user",
                PasswordHash = "dummyHash",
                PasswordSalt = "dummySalt"
            };
            await _userService.AddUserAsync(user);

            // Act
            var foundUser = await _userService.GetUserByIdAsync(user.UserId);

            // Assert
            Assert.IsNotNull(foundUser);
            Assert.AreEqual(user.UserId, foundUser.UserId);
        }

        [Test]
        public async Task GetUserByIdAsync_UserDoesNotExist_ShouldReturnNull()
        {
            // Act
            var foundUser = await _userService.GetUserByIdAsync(Guid.NewGuid());

            // Assert
            Assert.IsNull(foundUser);
        }
    }
}
