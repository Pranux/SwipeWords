using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models;
using Xunit;

public class FlashcardTests
{
    private FlashcardGameDatabaseContext GetInMemoryDbContext()
    {
        var options = new DbContextOptionsBuilder<FlashcardGameDatabaseContext>()
            .UseInMemoryDatabase(databaseName: "FlashcardTestDb")
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
        var correctWords = context.GetCorrectWordsById(flashcardId);

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
        var incorrectWords = context.GetIncorrectWordsById(flashcardId);

        // Assert
        Assert.Equal(new List<string> { "carrot", "potato", "tomato" }, incorrectWords);
    }
}