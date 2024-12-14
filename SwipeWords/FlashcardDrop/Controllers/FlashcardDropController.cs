using Microsoft.AspNetCore.Mvc;
using SwipeWords.FlashcardDrop.Services;
using SwipeWords.Models;
using SwipeWords.Services;

namespace SwipeWords.FlashcardDrop.Controllers;

[ApiController]
[Route("api/flashcardDrop")]

public class FlashcardDropController : ControllerBase
{
    private readonly FlashcardDropService _flashcardDropService;
    private readonly FlashcardService _flashcardService;

    public FlashcardDropController(FlashcardService flashcardService, FlashcardDropService flashcardDropService)
    {
        _flashcardDropService = flashcardDropService;
        _flashcardService = flashcardService;
        
    }

    [HttpGet("GetFlashcardForDrop")]
    public async Task<IActionResult> GetFlashcardForDrop()
    {
        try
        {
            var flashcard = await _flashcardService.GetFlashcardsAsync(5, false, "difficult");
            var mixedWords = flashcard.GetMixedWords();
            var word = _flashcardDropService.SelectWord(mixedWords);
            var isCorrect = _flashcardDropService.CheckIfCorrect(word, flashcard.Id);
            var id = flashcard.Id;
            return Ok(new { word, isCorrect, id });
        }
        catch (Exception ex)
        {
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpPost("CalculateScore")]
    public IActionResult CalculateScore([FromBody] ScoreRequest2 request)
    {
        try
        {
            var score = _flashcardDropService.CalculateScore(
                request.elapsedTime,
                request.difficulty);

            return Ok(new { score });
        }
        catch (Exception ex)
        {
            return StatusCode(500, "Internal server error");
        }
    }
    public class ScoreRequest2
    {
        public int elapsedTime { get; set; }
        public string difficulty { get; set; }
    }
}