using SwipeWords.Data;

namespace SwipeWords.Extensions;

public static class FlashcardExtensions
{
    public static List<string> GetCorrectWordsById(this FlashcardGameDatabaseContext databaseContext, Guid id)
    {
        var flashcardEntity = databaseContext.Flashcards.Find(id);
        if (flashcardEntity == null) throw new Exception("Flashcard not found");
        return flashcardEntity.CorrectWords.Split(",").ToList();
    }

    public static List<string> GetIncorrectWordsById(this FlashcardGameDatabaseContext databaseContext, Guid id)
    {
        var flashcardEntity = databaseContext.Flashcards.Find(id);
        if (flashcardEntity == null) throw new Exception("Flashcard not found");
        return flashcardEntity.IncorrectWords.Split(",").ToList();
    }
}