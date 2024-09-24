using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;
using WebApplication1.Data;
using System;
using System.Collections.Generic;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FlashcardsController : ControllerBase
    {
        private readonly FlashcardGameDatabaseContext _context;

        public FlashcardsController(FlashcardGameDatabaseContext context)
        {
            _context = context;
        }

        // GET: api/Flashcards/GetFlashcards?wordCount=5
        [HttpGet("GetFlashcards")]
        public IActionResult GetFlashcards(int wordCount = 5)
        {
            var flashcard = new Flashcard(_context, wordCount);
            var mixedWords = flashcard.GetMixedWords();
            return Ok(mixedWords);
        }

        // POST: api/Flashcards/CalculateScore
        [HttpPost("CalculateScore")]
        public IActionResult CalculateScore([FromBody] ScoreRequest scoreRequest)
        {
            var (score, correctWords, incorrectWords) = Flashcard.CalculateScore(
                scoreRequest.UserCorrect, 
                scoreRequest.UserIncorrect, 
                scoreRequest.FlashcardId
            );
            
            return Ok(new
            {
                Score = score,
                CorrectWords = correctWords,
                IncorrectWords = incorrectWords
            });
        }
    }

    public class ScoreRequest
    {
        public List<string> UserCorrect { get; set; } = new();
        public List<string> UserIncorrect { get; set; } = new();
        public Guid FlashcardId { get; set; }
    }
}