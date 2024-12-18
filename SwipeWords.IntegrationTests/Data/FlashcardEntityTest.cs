using Microsoft.EntityFrameworkCore;
using SwipeWords.Data;

namespace SwipeWords.IntegrationTests.Data;

[TestFixture]
[TestOf(typeof(FlashcardEntity))]
public class FlashcardEntityIntegrationTest
{
    private FlashcardGameDatabaseContext _dbContext;

    [SetUp]
    public void SetUp()
    {
        var options = new DbContextOptionsBuilder<FlashcardGameDatabaseContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;

        _dbContext = new FlashcardGameDatabaseContext(options);
        _dbContext.Database.EnsureCreated();
    }

    // Dispose the database context after each test
    [TearDown]
    public void TearDown()
    {
        _dbContext.Database.EnsureDeleted();
        _dbContext.Dispose();
    }

    [Test]
    public async Task AddFlashcardEntity_ShouldSaveToDatabase()
    {
        var flashcard = new FlashcardEntity
        {
            Id = Guid.NewGuid(),
            CorrectWords = "correct1, correct2",
            IncorrectWords = "incorrect1, incorrect2"
        };

        await _dbContext.AddAsync(flashcard);
        await _dbContext.SaveChangesAsync();

        // Check if the entity has been saved
        var savedFlashcard = await _dbContext.Set<FlashcardEntity>()
                                              .FirstOrDefaultAsync(f => f.Id == flashcard.Id);
        Assert.IsNotNull(savedFlashcard);
        Assert.AreEqual(flashcard.CorrectWords, savedFlashcard.CorrectWords);
        Assert.AreEqual(flashcard.IncorrectWords, savedFlashcard.IncorrectWords);
    }

    [Test]
    public async Task GetFlashcardEntity_ShouldRetrieveFromDatabase()
    {
        var flashcard = new FlashcardEntity
        {
            Id = Guid.NewGuid(),
            CorrectWords = "correct1, correct2",
            IncorrectWords = "incorrect1, incorrect2"
        };
        await _dbContext.AddAsync(flashcard);
        await _dbContext.SaveChangesAsync();
        
        var retrievedFlashcard = await _dbContext.Set<FlashcardEntity>()
                                                 .FirstOrDefaultAsync(f => f.Id == flashcard.Id);
        
        Assert.IsNotNull(retrievedFlashcard);
        Assert.AreEqual(flashcard.Id, retrievedFlashcard.Id);
        Assert.AreEqual(flashcard.CorrectWords, retrievedFlashcard.CorrectWords);
        Assert.AreEqual(flashcard.IncorrectWords, retrievedFlashcard.IncorrectWords);
    }

    [Test]
    public async Task UpdateFlashcardEntity_ShouldPersistChanges()
    {
        var flashcard = new FlashcardEntity
        {
            Id = Guid.NewGuid(),
            CorrectWords = "correct1, correct2",
            IncorrectWords = "incorrect1, incorrect2"
        };
        await _dbContext.AddAsync(flashcard);
        await _dbContext.SaveChangesAsync();

        // Modify the entity
        flashcard.CorrectWords = "newCorrect1, newCorrect2";
        flashcard.IncorrectWords = "newIncorrect1, newIncorrect2";
        _dbContext.Update(flashcard);
        await _dbContext.SaveChangesAsync();
        
        var updatedFlashcard = await _dbContext.Set<FlashcardEntity>()
                                               .FirstOrDefaultAsync(f => f.Id == flashcard.Id);
        Assert.IsNotNull(updatedFlashcard);
        Assert.AreEqual("newCorrect1, newCorrect2", updatedFlashcard.CorrectWords);
        Assert.AreEqual("newIncorrect1, newIncorrect2", updatedFlashcard.IncorrectWords);
    }

    [Test]
    public async Task DeleteFlashcardEntity_ShouldRemoveFromDatabase()
    {
        var flashcard = new FlashcardEntity
        {
            Id = Guid.NewGuid(),
            CorrectWords = "correct1, correct2",
            IncorrectWords = "incorrect1, incorrect2"
        };
        await _dbContext.AddAsync(flashcard);
        await _dbContext.SaveChangesAsync();
        
        _dbContext.Remove(flashcard);
        await _dbContext.SaveChangesAsync();

        var deletedFlashcard = await _dbContext.Set<FlashcardEntity>()
                                               .FirstOrDefaultAsync(f => f.Id == flashcard.Id);
        Assert.IsNull(deletedFlashcard);
    }
}