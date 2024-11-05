using SwipeWords.Data;

namespace SwipeWords.Extensions;

public static class FlashcardExtensions
{
    public static async Task<List<string>> GetSelectedCorrectWords(this FlashcardGameDatabaseContext databaseContext, int count, ExternalApiService apiService)
    {
        var correctWords = await apiService.GetCorrectWordsAsync(count);

        foreach (var word in correctWords)
        {
            databaseContext.CorrectWords.Add(new CorrectWord { Word = word });
        }
        await databaseContext.SaveChangesAsync();

        return correctWords;
    }

    public static async Task<List<string>> GetSelectedIncorrectWords(this FlashcardGameDatabaseContext databaseContext, int count, ExternalApiService apiService)
    {
        var incorrectWords = await apiService.GetIncorrectWordsAsync(count);

        foreach (var word in incorrectWords)
        {
            databaseContext.IncorrectWords.Add(new IncorrectWord { Word = word });
        }
        await databaseContext.SaveChangesAsync();

        return incorrectWords;
    }

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