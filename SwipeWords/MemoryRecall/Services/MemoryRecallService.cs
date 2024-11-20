using Microsoft.EntityFrameworkCore;
using SwipeWords.MemoryRecall.Data;

namespace SwipeWords.MemoryRecall.Services;

public class MemoryRecallService(
    MemoryRecallDatabaseContext context,
    TextProcessingService textProcessingService,
    BookRetrievalService bookRetrievalService)
{
    public async Task<(Guid TextId, string OriginalText)> FetchAndSaveTextAsync(int wordCount, double placeholderPercentage)
    {
        var rawText = await bookRetrievalService.FetchRandomPassageAsync(wordCount);
        if (string.IsNullOrEmpty(rawText))
        {
            throw new InvalidOperationException("Failed to fetch text.");
        }

        var textEntity = new SpeedReadingText
        {
            SpeedReadingTextId = Guid.NewGuid(),
            Content = rawText
        };

        context.SpeedReadingTexts.Add(textEntity);

        var positions = textProcessingService.GeneratePlaceholderPositions(rawText, placeholderPercentage);
        var recallEntity = new UserMemoryRecall
        {
            MemoryRecallId = Guid.NewGuid(),
            SpeedReadingTextId = textEntity.SpeedReadingTextId,
            RemovedWordPositions = positions
        };

        context.UserMemoryRecalls.Add(recallEntity);
        await context.SaveChangesAsync();

        return (recallEntity.MemoryRecallId, rawText);
    }

    public string GetTextWithPlaceholders(Guid recallId)
    {
        var recallEntity = context.UserMemoryRecalls
            .Include(r => r.SpeedReadingText)
            .FirstOrDefault(r => r.MemoryRecallId == recallId)
            ?? throw new InvalidOperationException("Recall entry not found.");

        return textProcessingService.GenerateTextWithPlaceholders(
            recallEntity.SpeedReadingText.Content,
            recallEntity.RemovedWordPositions
        );
    }

    public (int Score, List<string> CorrectWords) CompareUserGuesses(Guid recallId, List<string> userGuesses)
    {
        var recallEntry = context.UserMemoryRecalls
                              .Include(r => r.SpeedReadingText)
                              .FirstOrDefault(r => r.MemoryRecallId == recallId)
                          ?? throw new InvalidOperationException("Recall entry not found.");

        var correctWords = TextProcessingService.GetCorrectWordsFromPositions(
            recallEntry.SpeedReadingText.Content,
            recallEntry.RemovedWordPositions
        );

        if (userGuesses.Count != correctWords.Count)
        {
            throw new InvalidOperationException("The number of guesses does not match the number of placeholders.");
        }

        var score = correctWords.Where((t, i) => string.Equals(userGuesses[i], t, StringComparison.OrdinalIgnoreCase)).Count();

        return (score, correctWords);
    }
}