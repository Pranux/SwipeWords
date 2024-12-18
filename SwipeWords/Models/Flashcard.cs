using System.Collections;
using SwipeWords.Data;
using SwipeWords.Extensions;
using SwipeWords.Services;

namespace SwipeWords.Models
{
    public class Flashcard : IEnumerable<string>
    {
        private Words _words = new Words(new List<string>(), new List<string>()); // Initialize to prevent null issues

        public Guid Id { get; } = Guid.NewGuid();

        public List<string> CorrectWords => _words.CorrectWords;
        public List<string> IncorrectWords => _words.IncorrectWords;
        
        public async Task InitializeAsync(FlashcardGameDatabaseContext databaseContext, ExternalApiService apiService, int wordCount = 5, bool useScalingMode = false, WordSource.Difficulties difficulty = WordSource.Difficulties.Hard)
        {
            var random = new Random();
            var numCorrectWords = random.Next((int)(wordCount * 0.25), (int)(wordCount * 0.75));

            var correctWords = await apiService.GetCorrectWordsAsync(numCorrectWords, useScalingMode, difficulty);
            var incorrectWords = await apiService.GetIncorrectWordsAsync(wordCount - numCorrectWords, useScalingMode, difficulty);

            _words = new Words(correctWords, incorrectWords);

            Save(databaseContext);
        }

        public IEnumerator<string> GetEnumerator()
        {
            return _words.CorrectWords.Concat(_words.IncorrectWords).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public List<string> GetMixedWords()
        {
            var random = new Random();
            return _words.CorrectWords.Concat(_words.IncorrectWords).OrderBy(_ => random.Next()).ToList();
        }
        
        // For testing purposes
        public List<string> GetMixedWords(Random random = null)
        {
            random ??= new Random();
            return _words.CorrectWords.Concat(_words.IncorrectWords).OrderBy(_ => random.Next()).ToList();
        }

        public void SetWords(List<string> correctWords, List<string> incorrectWords)
        {
            _words = new Words(correctWords, incorrectWords);
        }
        
        public static (int score, List<string> correctWords, List<string> incorrectWords) CalculateScore(
            List<string> userCorrect,
            List<string> userIncorrect,
            Guid flashcardId,
            FlashcardGameDatabaseContext databaseContext)
            {
                var correctWords = databaseContext.GetCorrectWordsById(flashcardId);
                var incorrectWords = databaseContext.GetIncorrectWordsById(flashcardId);

                var correctMatches = correctWords.Intersect(userCorrect).Count();
                var incorrectMatches = incorrectWords.Intersect(userIncorrect).Count();
                var score = correctMatches + incorrectMatches;

                var flashcardToDelete = databaseContext.Flashcards.Find(flashcardId);
                if (flashcardToDelete != null)
                {
                    databaseContext.Flashcards.Remove(flashcardToDelete);
                    databaseContext.SaveChanges();
                }

                return (score, correctWords, incorrectWords);
            }

        private void Save(FlashcardGameDatabaseContext databaseContext)
        {
            var flashcardEntity = new FlashcardEntity
            {
                Id = Id,
                CorrectWords = string.Join(",", _words.CorrectWords),
                IncorrectWords = string.Join(",", _words.IncorrectWords)
            };
            databaseContext.Add(flashcardEntity);
            databaseContext.SaveChanges();
        }

    }
}