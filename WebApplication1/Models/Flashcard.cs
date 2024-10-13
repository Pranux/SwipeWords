using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using WebApplication1.Data;

namespace WebApplication1.Models
{
    public class Flashcard : IEnumerable<string>
    {
        public enum FlashcardDifficulty
        {
            Easy,
            Medium,
            Hard
        }

        public Guid Id { get; }
        public FlashcardDifficulty Difficulty { get; }
        private Words _words;

        public Flashcard(FlashcardGameDatabaseContext databaseContext, int wordCount = 5, FlashcardDifficulty difficulty = FlashcardDifficulty.Medium)
        {
            Id = Guid.NewGuid();
            Difficulty = difficulty;
            var random = new Random();

            var numCorrectWords = random.Next((int)(wordCount * 0.25), (int)(wordCount * 0.75));

            var correctWords = databaseContext.GetSelectedCorrectWords(numCorrectWords);
            var incorrectWords = databaseContext.GetSelectedIncorrectWords(wordCount - numCorrectWords);

            _words = new Words(correctWords, incorrectWords);

            Save(databaseContext);
        }

        public List<string> GetMixedWords()
        {
            var random = new Random();
            return _words.CorrectWords.Concat(_words.IncorrectWords).OrderBy(x => random.Next()).ToList();
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

            return (score, correctWords, incorrectWords);
        }

        private void Save(FlashcardGameDatabaseContext databaseContext)
        {
            var flashcardEntity = new FlashcardEntity
            {
                Id = this.Id,
                CorrectWords = string.Join(",", _words.CorrectWords),
                IncorrectWords = string.Join(",", _words.IncorrectWords)
            };
            databaseContext.Add(flashcardEntity);
            databaseContext.SaveChanges();
        }

        // Implementation of IEnumerable<string>
        public IEnumerator<string> GetEnumerator()
        {
            return _words.CorrectWords.Concat(_words.IncorrectWords).GetEnumerator();
        }

        // Implementation of non-generic IEnumerable
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}