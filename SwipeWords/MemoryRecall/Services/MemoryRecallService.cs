using Microsoft.EntityFrameworkCore;
using SwipeWords.MemoryRecall.Data;

namespace SwipeWords.MemoryRecall.Services;

public interface IMemoryRecallService
{
    Task<(Guid TextId, string OriginalText)> FetchAndSaveTextAsync(int wordCount, double placeholderPercentage);
    string GetTextWithPlaceholders(Guid recallId);
    (int Score, List<string> CorrectWords) CompareUserGuesses(Guid recallId, List<string> userGuesses);
    
}

public class MemoryRecallService : IMemoryRecallService 
{
    private readonly MemoryRecallDatabaseContext _context;
    private readonly ITextProcessingService _textProcessingService;
    private readonly IBookRetrievalService _bookRetrievalService;
    
    public MemoryRecallService(
        MemoryRecallDatabaseContext context,
        ITextProcessingService textProcessingService,
        IBookRetrievalService bookRetrievalService)
    {
        _context = context;
        _textProcessingService = textProcessingService;
        _bookRetrievalService = bookRetrievalService;
    }
    
    public async Task<(Guid TextId, string OriginalText)> FetchAndSaveTextAsync(int wordCount, double placeholderPercentage)
    {
        var rawText = await _bookRetrievalService.FetchRandomPassageAsync(wordCount);
        if (string.IsNullOrEmpty(rawText))
        {
            throw new InvalidOperationException("Failed to fetch text.");
        }

        var textEntity = new SpeedReadingText
        {
            SpeedReadingTextId = Guid.NewGuid(),
            Content = rawText
        };

        _context.SpeedReadingTexts.Add(textEntity);

        var positions = _textProcessingService.GeneratePlaceholderPositions(rawText, placeholderPercentage);
        var recallEntity = new UserMemoryRecall
        {
            MemoryRecallId = Guid.NewGuid(),
            SpeedReadingTextId = textEntity.SpeedReadingTextId,
            RemovedWordPositions = positions
        };

        _context.UserMemoryRecalls.Add(recallEntity);
        await _context.SaveChangesAsync();

        return (recallEntity.MemoryRecallId, rawText);
    }

    public string GetTextWithPlaceholders(Guid recallId)
    {
        var recallEntity = _context.UserMemoryRecalls
            .Include(r => r.SpeedReadingText)
            .FirstOrDefault(r => r.MemoryRecallId == recallId)
            ?? throw new InvalidOperationException("Recall entry not found.");

        return _textProcessingService.GenerateTextWithPlaceholders(
            recallEntity.SpeedReadingText.Content,
            recallEntity.RemovedWordPositions
        );
    }

    public (int Score, List<string> CorrectWords) CompareUserGuesses(Guid recallId, List<string> userGuesses)
    {
        var recallEntry = _context.UserMemoryRecalls
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