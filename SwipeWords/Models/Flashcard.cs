using System.Collections;
using SwipeWords.Data;

namespace SwipeWords.Models;

public class Flashcard 
{
    public Flashcard(FlashcardGameDatabaseContext databaseContext, int wordCount = 5)
    {
        Id = Guid.NewGuid();
        var random = new Random();

        var numCorrectWords = random.Next((int)(wordCount * 0.25), (int)(wordCount * 0.75));

        CorrectWords = GetSelectedCorrectWords(databaseContext, numCorrectWords);
        IncorrectWords = GetSelectedIncorrectWords(databaseContext, wordCount - numCorrectWords);

        Save(databaseContext);
    }

    public Guid Id { get; }
    public List<string> CorrectWords { get; }
    public List<string> IncorrectWords { get; }

    public List<string> GetMixedWords()
    {
        var random = new Random();
        return CorrectWords.Concat(IncorrectWords).OrderBy(x => random.Next()).ToList();
    }

    public static (int score, List<string> correctWords, List<string> incorrectWords) CalculateScore(
        List<string> userCorrect,
        List<string> userIncorrect,
        Guid flashcardId,
        FlashcardGameDatabaseContext databaseContext)
    {
        var correctWords = GetCorrectWordsById(databaseContext, flashcardId);
        var incorrectWords = GetIncorrectWordsById(databaseContext, flashcardId);

        var correctMatches = correctWords.Intersect(userCorrect).Count();
        var incorrectMatches = incorrectWords.Intersect(userIncorrect).Count();
        var score = correctMatches + incorrectMatches;

        return (score, correctWords, incorrectWords);
    }

    private void Save(FlashcardGameDatabaseContext databaseContext)
    {
        var flashcardEntity = new FlashcardEntity
        {
            Id = Id,
            CorrectWords = string.Join(",", CorrectWords),
            IncorrectWords = string.Join(",", IncorrectWords)
        };
        databaseContext.Add(flashcardEntity);
        databaseContext.SaveChanges();
    }
    
    public static List<string> GetSelectedCorrectWords( FlashcardGameDatabaseContext databaseContext, int count)
    {
        return databaseContext.CorrectWords
            .OrderByDescending(w => w.Frequency)
            .Skip((int)(databaseContext.CorrectWords.Count() * 0.25))
            .OrderBy(w => Guid.NewGuid())
            .Take(count)
            .Select(w => w.Word)
            .ToList();
    }

    public static List<string> GetSelectedIncorrectWords( FlashcardGameDatabaseContext databaseContext, int count)
    {
        return databaseContext.IncorrectWords
            .OrderByDescending(w => w.Frequency)
            .Skip((int)(databaseContext.IncorrectWords.Count() * 0.25))
            .OrderBy(w => Guid.NewGuid())
            .Take(count)
            .Select(w => w.Word)
            .ToList();
    }

    
    public static List<string> GetCorrectWordsById( FlashcardGameDatabaseContext databaseContext, Guid id)
    {
        var flashcardEntity = databaseContext.Flashcards.Find(id);
        if (flashcardEntity == null) throw new Exception("Flashcard not found");
        return flashcardEntity.CorrectWords.Split(",").ToList();
    }

    public static List<string> GetIncorrectWordsById(FlashcardGameDatabaseContext databaseContext, Guid id)
    {
        var flashcardEntity = databaseContext.Flashcards.Find(id);
        if (flashcardEntity == null) throw new Exception("Flashcard not found");
        return flashcardEntity.IncorrectWords.Split(",").ToList();
    }
}