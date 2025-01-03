using Microsoft.AspNetCore.Mvc;
using SwipeWords.Services;
using SwipeWords.Models;
using System;
using System.Threading.Tasks;

namespace SwipeWords.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FlashcardsController : ControllerBase
    {
        private readonly IFlashcardService _flashcardService;
        private readonly ILogger<FlashcardsController> _logger;

        public FlashcardsController(IFlashcardService flashcardService, ILogger<FlashcardsController> logger)
        {
            _flashcardService = flashcardService;
            _logger = logger;
        }

        [HttpGet("GetFlashcards")]
        public async Task<IActionResult> GetFlashcards(int wordCount = 5, bool useScalingMode = false, WordSource.Difficulties difficulty = WordSource.Difficulties.Hard)
        {
            try
            {
                var flashcard = await _flashcardService.GetFlashcardsAsync(wordCount, useScalingMode, difficulty);
                var mixedWords = flashcard.GetMixedWords();
                return Ok(new { flashcardId = flashcard.Id, mixedWords });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting flashcards");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("CalculateScore")]
        public IActionResult CalculateScore([FromBody] ScoreRequest request)
        {
            try
            {
                var (score, correctWords, incorrectWords) = _flashcardService.CalculateScore(
                    request.UserCorrect,
                    request.UserIncorrect,
                    request.FlashcardId);

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
            public List<string> UserCorrect { get; set; }
            public List<string> UserIncorrect { get; set; }
            public Guid FlashcardId { get; set; }
        }
}