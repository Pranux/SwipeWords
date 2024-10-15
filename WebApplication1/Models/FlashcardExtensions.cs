using System;
using System.Collections.Generic;
using System.Linq;
using WebApplication1.Data;

namespace WebApplication1.Models
{
    public static class FlashcardExtensions
    {
        public static List<string> GetSelectedCorrectWords(this FlashcardGameDatabaseContext databaseContext, int count)
        {
            return databaseContext.CorrectWords
                .OrderByDescending(w => w.Frequency)
                .Skip((int)(databaseContext.CorrectWords.Count() * 0.25))
                .OrderBy(w => Guid.NewGuid())
                .Take(count)
                .Select(w => w.Word)
                .ToList();
        }

        public static List<string> GetSelectedIncorrectWords(this FlashcardGameDatabaseContext databaseContext, int count)
        {
            return databaseContext.IncorrectWords
                .OrderByDescending(w => w.Frequency)
                .Skip((int)(databaseContext.IncorrectWords.Count() * 0.25))
                .OrderBy(w => Guid.NewGuid())
                .Take(count)
                .Select(w => w.Word)
                .ToList();
        }

        public static List<string> GetCorrectWordsById(this FlashcardGameDatabaseContext databaseContext, Guid id)
        {
            var flashcardEntity = databaseContext.Flashcards.Find(id);
            if (flashcardEntity == null)
            {
                throw new Exception("Flashcard not found");
            }
            return flashcardEntity.CorrectWords.Split(",").ToList();
        }

        public static List<string> GetIncorrectWordsById(this FlashcardGameDatabaseContext databaseContext, Guid id)
        {
            var flashcardEntity = databaseContext.Flashcards.Find(id);
            if (flashcardEntity == null)
            {
                throw new Exception("Flashcard not found");
            }
            return flashcardEntity.IncorrectWords.Split(",").ToList();
        }
    }
}