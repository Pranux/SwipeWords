using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using SwipeWords.Controllers;
using SwipeWords.Data;
using SwipeWords.Services;
using SwipeWords.Models;
using System.Text.Json;

namespace SwipeWords.UnitTests.Controllers;

[TestFixture]
[TestOf(typeof(UserController))]
public class UserControllerTests
{
    private Mock<IUserService> _mockUserService;
    private Mock<ITokenProvider> _mockTokenProvider;
    private Mock<ILogger<UserController>> _mockLogger;
    private UserController _controller;
    private readonly string _testToken = "test.jwt.token";

    [SetUp]
    public void SetUp()
    {
        _mockUserService = new Mock<IUserService>();
        _mockTokenProvider = new Mock<ITokenProvider>();
        _mockLogger = new Mock<ILogger<UserController>>();
        _controller = new UserController(
            _mockUserService.Object,
            _mockTokenProvider.Object,
            _mockLogger.Object
        );
    }

    [Test]
    public async Task Register_ReturnsCreatedResult_WhenUserRegistrationSucceeds()
    {
        var userDto = new UserDto
        {
            Name = "testUser",
            Password = "testPassword"
        };

        _mockUserService
            .Setup(service => service.IsUsernameTakenAsync(userDto.Name))
            .ReturnsAsync(false);

        User createdUser = null;
        _mockUserService
            .Setup(service => service.AddUserAsync(It.IsAny<User>()))
            .Callback<User>(user => createdUser = user);

        _mockTokenProvider
            .Setup(provider => provider.Create(It.IsAny<User>()))
            .Returns(_testToken);
        
        var result = await _controller.PostUser(userDto);
        
        Assert.That(result, Is.TypeOf<CreatedAtActionResult>());
        var createdResult = result as CreatedAtActionResult;
        var resultDict = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(
            JsonSerializer.Serialize(createdResult.Value)
        );
        
        Assert.That(resultDict["token"].GetString(), Is.EqualTo(_testToken));
        Assert.That(createdUser.Name, Is.EqualTo(userDto.Name));
        Assert.That(createdUser.UserId, Is.Not.EqualTo(Guid.Empty));
    }

    [Test]
    public async Task Register_ReturnsConflict_WhenUsernameIsTaken()
    {
        var userDto = new UserDto
        {
            Name = "existingUser",
            Password = "testPassword"
        };

        _mockUserService
            .Setup(service => service.IsUsernameTakenAsync(userDto.Name))
            .ReturnsAsync(true);
        
        var result = await _controller.PostUser(userDto);
        
        Assert.That(result, Is.TypeOf<ConflictObjectResult>());
        var conflictResult = result as ConflictObjectResult;
        var resultDict = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(
            JsonSerializer.Serialize(conflictResult.Value)
        );
        
        Assert.That(resultDict["message"].GetString(), Is.EqualTo("Username is taken"));
    }

    [Test]
    public async Task Login_ReturnsOkResult_WhenCredentialsAreValid()
    {
        var userDto = new UserDto
        {
            Name = "testUser",
            Password = "testPassword"
        };

        var storedUser = new User
        {
            UserId = Guid.NewGuid(),
            Name = userDto.Name,
            PasswordSalt = "testSalt",
            PasswordHash = PasswordHasher.HashPassword(userDto.Password, "testSalt")
        };

        _mockUserService
            .Setup(service => service.GetUserByNameAsync(userDto.Name))
            .ReturnsAsync(storedUser);

        _mockTokenProvider
            .Setup(provider => provider.Create(It.IsAny<User>()))
            .Returns(_testToken);
        
        var result = await _controller.Login(userDto);
        
        Assert.That(result, Is.TypeOf<OkObjectResult>());
        var okResult = result as OkObjectResult;
        var resultDict = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(
            JsonSerializer.Serialize(okResult.Value)
        );
        
        Assert.That(resultDict["token"].GetString(), Is.EqualTo(_testToken));
    }

    [Test]
    public async Task Login_ReturnsNotFound_WhenUserDoesNotExist()
    {
        var userDto = new UserDto
        {
            Name = "nonexistentUser",
            Password = "testPassword"
        };

        _mockUserService
            .Setup(service => service.GetUserByNameAsync(userDto.Name))
            .ReturnsAsync((User)null);
        
        var result = await _controller.Login(userDto);
        
        Assert.That(result, Is.TypeOf<NotFoundResult>());
    }

    [Test]
    public async Task CheckUsernameAvailability_ReturnsCorrectAvailability()
    {
        var username = "testUser";
        _mockUserService
            .Setup(service => service.IsUsernameTakenAsync(username))
            .ReturnsAsync(false);
        
        var result = await _controller.CheckUsernameAvailability(username);
        
        Assert.That(result, Is.TypeOf<OkObjectResult>());
        var okResult = result as OkObjectResult;
        var resultDict = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(
            JsonSerializer.Serialize(okResult.Value)
        );
        
        Assert.That(resultDict["isAvailable"].GetBoolean(), Is.True);
    }

    [Test]
    public async Task GetUserById_ReturnsOkResult_WhenUserExists()
    {
        var userId = Guid.NewGuid();
        var user = new User
        {
            UserId = userId,
            Name = "testUser"
        };

        _mockUserService
            .Setup(service => service.GetUserByIdAsync(userId))
            .ReturnsAsync(user);
        
        var result = await _controller.GetUserById(userId);
        
        Assert.That(result, Is.TypeOf<OkObjectResult>());
        var okResult = result as OkObjectResult;
        var returnedUser = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(
            JsonSerializer.Serialize(okResult.Value)
        );
        
        Assert.That(returnedUser["UserId"].GetString(), Is.EqualTo(userId.ToString()));
        Assert.That(returnedUser["Name"].GetString(), Is.EqualTo(user.Name));
    }

    [Test]
    public async Task GetUserById_ReturnsNotFound_WhenUserDoesNotExist()
    {
        var userId = Guid.NewGuid();
        _mockUserService
            .Setup(service => service.GetUserByIdAsync(userId))
            .ReturnsAsync((User)null);
        
        var result = await _controller.GetUserById(userId);
        
        Assert.That(result, Is.TypeOf<NotFoundObjectResult>());
        var notFoundResult = result as NotFoundObjectResult;
        var resultDict = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(
            JsonSerializer.Serialize(notFoundResult.Value)
        );
        
        Assert.That(resultDict["message"].GetString(), Is.EqualTo("User not found"));
    }
}