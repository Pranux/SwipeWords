using Microsoft.EntityFrameworkCore;
using SwipeWords.Data;
using SwipeWords.Models;
using Xunit;

public class FlashcardTests
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
    public void GetCorrectWordsById_ShouldReturnCorrectWords()
    {
        // Arrange
        var context = GetInMemoryDbContext();
        var flashcardEntity = context.Flashcards.First();
        var flashcardId = flashcardEntity.Id;

        // Act
        var correctWords = Flashcard.GetCorrectWordsById(context, flashcardId);

        // Assert
        Assert.Equal(new List<string> { "apple", "banana", "orange" }, correctWords);
    }

    [Fact]
    public void GetIncorrectWordsById_ShouldReturnIncorrectWords()
    {
        // Arrange
        var context = GetInMemoryDbContext();
        var flashcardEntity = context.Flashcards.First();
        var flashcardId = flashcardEntity.Id;

        // Act
        var incorrectWords = Flashcard.GetIncorrectWordsById(context, flashcardId);

        // Assert
        Assert.Equal(new List<string> { "carrot", "potato", "tomato" }, incorrectWords);
    }
}