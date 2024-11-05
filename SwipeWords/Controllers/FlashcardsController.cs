using Microsoft.AspNetCore.Mvc;
using SwipeWords.Data;
using SwipeWords.Models;
using SwipeWords.Services;

namespace SwipeWords.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FlashcardsController : ControllerBase
{
    private readonly FlashcardService _flashcardService;
    private readonly FlashcardGameDatabaseContext _dbContext;
    private readonly ILogger<FlashcardsController> _logger;

    public FlashcardsController(FlashcardService flashcardService, FlashcardGameDatabaseContext dbContext, ILogger<FlashcardsController> logger)
    {
        _dbContext = dbContext;
        _flashcardService = flashcardService;
        _logger = logger;
    }

    // GET: api/Flashcards/GetFlashcards?wordCount=5
    [HttpGet("GetFlashcards")]
    public async Task<IActionResult> GetFlashcards(int wordCount = 5)
    {
        try
        {
            await _flashcardService.InitializeAsync(wordCount);
            var mixedWords = _flashcardService.GetMixedWords();
            return Ok(new { flashcardId = _flashcardService.Id, mixedWords });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting flashcards");
            return StatusCode(500, "Internal server error");
        }
    }

    // POST: api/Flashcards/CalculateScore
    [HttpPost("CalculateScore")]
    public IActionResult CalculateScore([FromBody] ScoreRequest request)
    {
        try
        {
            var userCorrectList = request.UserCorrect.Split(',').ToList();
            var userIncorrectList = request.UserIncorrect.Split(',').ToList();

            var (score, correctWords, incorrectWords) = FlashcardService.CalculateScore(
                userCorrectList,
                userIncorrectList,
                request.FlashcardId,
                _dbContext);

            return Ok(new { score, correctWords, incorrectWords });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error calculating score");
            return StatusCode(500, "Internal server error");
        }
    }
}

public class ScoreRequest
{
    public string UserCorrect { get; set; } = string.Empty;
    public string UserIncorrect { get; set; } = string.Empty;
    public Guid FlashcardId { get; set; }
}