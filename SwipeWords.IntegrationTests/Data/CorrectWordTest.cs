using Microsoft.EntityFrameworkCore;
using SwipeWords.Data;

namespace SwipeWords.IntegrationTests.Data;

[TestFixture]
[TestOf(typeof(CorrectWord))]
public class CorrectWordIntegrationTests
{
    private FlashcardGameDatabaseContext _dbContext;

    [SetUp]
    public void SetUp()
    {
        var options = new DbContextOptionsBuilder<FlashcardGameDatabaseContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;

        _dbContext = new FlashcardGameDatabaseContext(options);
    }

    // Dispose the database context after each test
    [TearDown]
    public void TearDown()
    {
        _dbContext.Database.EnsureDeleted();
        _dbContext.Dispose();
    }

    [Test]
    public async Task CanInsertAndRetrieveCorrectWord()
    {
        var correctWord = new CorrectWord
        {
            WordId = 1,
            Word = "example",
            Frequency = 5
        };
        
        _dbContext.CorrectWords.Add(correctWord);
        await _dbContext.SaveChangesAsync();
        
        var retrievedWord = await _dbContext.CorrectWords.FirstOrDefaultAsync(w => w.WordId == 1);
        Assert.IsNotNull(retrievedWord);
        Assert.AreEqual("example", retrievedWord.Word);
        Assert.AreEqual(5, retrievedWord.Frequency);
    }

    [Test]
    public async Task CanUpdateCorrectWord()
    {
        var correctWord = new CorrectWord
        {
            WordId = 1,
            Word = "example",
            Frequency = 5
        };

        _dbContext.CorrectWords.Add(correctWord);
        await _dbContext.SaveChangesAsync();

        //Update the entity and save changes
        correctWord.Word = "updatedExample";
        correctWord.Frequency = 10;
        _dbContext.CorrectWords.Update(correctWord);
        await _dbContext.SaveChangesAsync();
        
        var updatedWord = await _dbContext.CorrectWords.FirstOrDefaultAsync(w => w.WordId == 1);
        Assert.IsNotNull(updatedWord);
        Assert.AreEqual("updatedExample", updatedWord.Word);
        Assert.AreEqual(10, updatedWord.Frequency);
    }

    [Test]
    public async Task CanDeleteCorrectWord()
    {
        var correctWord = new CorrectWord
        {
            WordId = 1,
            Word = "example",
            Frequency = 5
        };

        _dbContext.CorrectWords.Add(correctWord);
        await _dbContext.SaveChangesAsync();
        
        _dbContext.CorrectWords.Remove(correctWord);
        await _dbContext.SaveChangesAsync();

        var deletedWord = await _dbContext.CorrectWords.FirstOrDefaultAsync(w => w.WordId == 1);
        Assert.IsNull(deletedWord);
    }
}
