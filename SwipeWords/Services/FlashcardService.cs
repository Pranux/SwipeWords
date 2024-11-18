using SwipeWords.Data;
using SwipeWords.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace SwipeWords.Services
{
    public interface IFlashcardService
    {
        Task<Flashcard> GetFlashcardsAsync(int wordCount, bool useScalingMode, string difficulty);
        (int score, List<string> correctWords, List<string> incorrectWords) CalculateScore(List<string> userCorrect, List<string> userIncorrect, Guid flashcardId);
    }
    
    public class FlashcardService : IFlashcardService
    {
        private readonly IFlashcardGameDatabaseContext _context;
        private readonly ILogger<FlashcardService> _logger;

        public FlashcardService(IFlashcardGameDatabaseContext context, ILogger<FlashcardService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Flashcard> GetFlashcardsAsync(int wordCount, bool useScalingMode, string difficulty)
        {
            var apiService = new ExternalApiService();
            var flashcard = new Flashcard();
            await flashcard.InitializeAsync(_context as FlashcardGameDatabaseContext, apiService, wordCount, useScalingMode, difficulty);
            return flashcard;
        }

        public (int score, List<string> correctWords, List<string> incorrectWords) CalculateScore(List<string> userCorrect, List<string> userIncorrect, Guid flashcardId)
        {
            return Flashcard.CalculateScore(userCorrect, userIncorrect, flashcardId, _context as FlashcardGameDatabaseContext);
        }
    }
}