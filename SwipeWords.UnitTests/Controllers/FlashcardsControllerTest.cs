using Moq;
using SwipeWords.Controllers;
using SwipeWords.Services;
using SwipeWords.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace SwipeWords.UnitTests.Controllers;

[TestFixture]
[TestOf(typeof(FlashcardsController))]
public class FlashcardsControllerTests
{
    private Mock<IFlashcardService> _mockFlashcardService;
    private Mock<ILogger<FlashcardsController>> _mockLogger;
    private FlashcardsController _controller;

    [SetUp]
    public void SetUp()
    {
        _mockFlashcardService = new Mock<IFlashcardService>();
        _mockLogger = new Mock<ILogger<FlashcardsController>>();
        _controller = new FlashcardsController(_mockFlashcardService.Object, _mockLogger.Object);
    }

    [Test]
    public async Task GetFlashcards_ReturnsOkResult_WithFlashcards()
    {
        var correctWords = new List<string> { "correct1" };
        var incorrectWords = new List<string> { "incorrect1" };
        var expectedMixedWords = new List<string> { "correct1", "incorrect1" };
        
        var flashcard = new Flashcard();
        flashcard.SetWords(correctWords, incorrectWords);

        _mockFlashcardService
            .Setup(service => service.GetFlashcardsAsync(It.IsAny<int>(), It.IsAny<bool>(), It.IsAny<string>()))
            .ReturnsAsync(flashcard);
        
        var result = await _controller.GetFlashcards(5, false, "Difficult");
        
        var okResult = result as OkObjectResult;
        Assert.That(okResult, Is.Not.Null);
        
        var resultDict = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(
            JsonSerializer.Serialize(okResult.Value)
        );

        Assert.That(Guid.Parse(resultDict["flashcardId"].GetString()), Is.EqualTo(flashcard.Id));
        var returnedWords = JsonSerializer.Deserialize<List<string>>(resultDict["mixedWords"].GetRawText());
        Assert.That(returnedWords.Count, Is.EqualTo(expectedMixedWords.Count));
        Assert.That(returnedWords.Contains("correct1"), Is.True);
        Assert.That(returnedWords.Contains("incorrect1"), Is.True);
    }

    [Test]
    public async Task GetFlashcards_ReturnsInternalServerError_WhenExceptionOccurs()
    {
        _mockFlashcardService
            .Setup(service => service.GetFlashcardsAsync(It.IsAny<int>(), It.IsAny<bool>(), It.IsAny<string>()))
            .ThrowsAsync(new Exception("Test exception"));
        
        var result = await _controller.GetFlashcards(5, false, "Difficult");
        
        Assert.That(result, Is.TypeOf<ObjectResult>());
        var objectResult = result as ObjectResult;
        Assert.That(objectResult.StatusCode, Is.EqualTo(500));
        Assert.That(objectResult.Value, Is.EqualTo("Internal server error"));
    }

    [Test]
    public void CalculateScore_ReturnsOkResult_WithScore()
    {
        var scoreRequest = new ScoreRequest
        {
            UserCorrect = new List<string> { "correct1", "correct2" },
            UserIncorrect = new List<string> { "incorrect1" },
            FlashcardId = Guid.NewGuid()
        };

        var expectedScore = 3;
        var correctWords = new List<string> { "correct1", "correct2" };
        var incorrectWords = new List<string> { "incorrect1" };

        _mockFlashcardService
            .Setup(service => service.CalculateScore(
                It.IsAny<List<string>>(),
                It.IsAny<List<string>>(),
                It.IsAny<Guid>()))
            .Returns((expectedScore, correctWords, incorrectWords));
        
        var result = _controller.CalculateScore(scoreRequest);
        
        Assert.That(result, Is.TypeOf<OkObjectResult>());
        var okResult = result as OkObjectResult;
        
        var resultDict = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(
            JsonSerializer.Serialize(okResult.Value)
        );

        Assert.That(resultDict["score"].GetInt32(), Is.EqualTo(expectedScore));
        Assert.That(JsonSerializer.Deserialize<List<string>>(resultDict["correctWords"].GetRawText()), 
            Is.EqualTo(correctWords));
        Assert.That(JsonSerializer.Deserialize<List<string>>(resultDict["incorrectWords"].GetRawText()), 
            Is.EqualTo(incorrectWords));
    }

    [Test]
    public void CalculateScore_ReturnsInternalServerError_WhenExceptionOccurs()
    {
        var scoreRequest = new ScoreRequest
        {
            UserCorrect = new List<string> { "correct1", "correct2" },
            UserIncorrect = new List<string> { "incorrect1" },
            FlashcardId = Guid.NewGuid()
        };

        _mockFlashcardService
            .Setup(service => service.CalculateScore(
                It.IsAny<List<string>>(),
                It.IsAny<List<string>>(),
                It.IsAny<Guid>()))
            .Throws(new Exception("Test exception"));
        
        var result = _controller.CalculateScore(scoreRequest);
        
        Assert.That(result, Is.TypeOf<ObjectResult>());
        var objectResult = result as ObjectResult;
        Assert.That(objectResult.StatusCode, Is.EqualTo(500));
        Assert.That(objectResult.Value, Is.EqualTo("Internal server error"));
    }
}