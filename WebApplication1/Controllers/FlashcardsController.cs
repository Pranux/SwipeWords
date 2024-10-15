using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebApplication1.Models;
using WebApplication1.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FlashcardsController : ControllerBase
    {
        private readonly FlashcardGameDatabaseContext _context;
        private readonly ILogger<FlashcardsController> _logger;

        public FlashcardsController(FlashcardGameDatabaseContext context, ILogger<FlashcardsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: api/Flashcards/GetFlashcards?wordCount=5
        [HttpGet("GetFlashcards")]
        public IActionResult GetFlashcards(int wordCount = 5)
        {
            try
            {
                var flashcard = new Flashcard(_context, wordCount);
                var mixedWords = flashcard.GetMixedWords();
                return Ok(new { flashcardId = flashcard.Id, mixedWords });
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
                var userCorrectList = request.UserCorrect.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                var userIncorrectList = request.UserIncorrect.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList();

                var (score, correctWords, incorrectWords) = Flashcard.CalculateScore(
                    userCorrectList,
                    userIncorrectList,
                    request.FlashcardId,
                    _context);

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
}