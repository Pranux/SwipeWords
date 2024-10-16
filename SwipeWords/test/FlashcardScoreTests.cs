using Microsoft.EntityFrameworkCore;
using SwipeWords.Data;
using SwipeWords.Models;
using Xunit;

public class FlashcardScoreTests
{
    private FlashcardGameDatabaseContext GetInMemoryDbContext()
    {
        var options = new DbContextOptionsBuilder<FlashcardGameDatabaseContext>()
            .UseInMemoryDatabase("FlashcardTestDb")
            .Options;

        var context = new FlashcardGameDatabaseContext(options);
        context.Flashcards.Add(new FlashcardEntity
        {
            Id = Guid.NewGuid(),
            CorrectWords = "apple,banana,orange",
            IncorrectWords = "carrot,potato,tomato"
        });
        context.SaveChanges();

        return context;
    }

    [Fact]
    public void CalculateScore_ShouldReturnCorrectScoreAndWords()
    {
        // Arrange
        var context = GetInMemoryDbContext();
        var flashcardEntity = context.Flashcards.First();
        var flashcardId = flashcardEntity.Id;

        var userCorrect = new List<string> { "apple", "banana" };
        var userIncorrect = new List<string> { "carrot" };

        // Act
        var (score, correctWords, incorrectWords) =
            Flashcard.CalculateScore(userCorrect, userIncorrect, flashcardId, context);

        // Assert
        Assert.Equal(3, score);
        Assert.Equal(new List<string> { "apple", "banana", "orange" }, correctWords);
        Assert.Equal(new List<string> { "carrot", "potato", "tomato" }, incorrectWords);
    }
}