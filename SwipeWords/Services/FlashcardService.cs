using SwipeWords.Data;
using SwipeWords.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace SwipeWords.Services
{
    public class FlashcardService
    {
        private readonly FlashcardGameDatabaseContext _context;
        private readonly ILogger<FlashcardService> _logger;

        public FlashcardService(FlashcardGameDatabaseContext context, ILogger<FlashcardService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Flashcard> GetFlashcardsAsync(int wordCount, bool useScalingMode, string difficulty)
        {
            var apiService = new ExternalApiService();
            var flashcard = new Flashcard();
            await flashcard.InitializeAsync(_context, apiService, wordCount, useScalingMode, difficulty);
            return flashcard;
        }

        public (int score, List<string> correctWords, List<string> incorrectWords) CalculateScore(List<string> userCorrect, List<string> userIncorrect, Guid flashcardId)
        {
            return Flashcard.CalculateScore(userCorrect, userIncorrect, flashcardId, _context);
        }
    }
}