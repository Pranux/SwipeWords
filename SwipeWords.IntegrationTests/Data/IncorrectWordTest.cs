using Microsoft.EntityFrameworkCore;
using SwipeWords.Data;

namespace SwipeWords.IntegrationTests.Data;

[TestFixture]
[TestOf(typeof(IncorrectWord))]
public class IncorrectWordIntegrationTests
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
    public async Task CanInsertAndRetrieveIncorrectWord()
    {
        var incorrectWord = new IncorrectWord
        {
            WordId = 1,
            Word = "example",
            Frequency = 5
        };
        
        _dbContext.IncorrectWords.Add(incorrectWord);
        await _dbContext.SaveChangesAsync();
        
        var retrievedWord = await _dbContext.IncorrectWords.FirstOrDefaultAsync(w => w.WordId == 1);
        Assert.IsNotNull(retrievedWord);
        Assert.AreEqual("example", retrievedWord.Word);
        Assert.AreEqual(5, retrievedWord.Frequency);
    }
    
    [Test]
    public async Task CanUpdateIncorrectWord()
    {
        var incorrectWord = new IncorrectWord
        {
            WordId = 1,
            Word = "example",
            Frequency = 5
        };

        _dbContext.IncorrectWords.Add(incorrectWord);
        await _dbContext.SaveChangesAsync();
        
        incorrectWord.Word = "updatedExample";
        incorrectWord.Frequency = 10;
        _dbContext.IncorrectWords.Update(incorrectWord);
        await _dbContext.SaveChangesAsync();
        
        var updatedWord = await _dbContext.IncorrectWords.FirstOrDefaultAsync(w => w.WordId == 1);
        Assert.IsNotNull(updatedWord);
        Assert.AreEqual("updatedExample", updatedWord.Word);
        Assert.AreEqual(10, updatedWord.Frequency);
    }

    [Test]
    public async Task CanDeleteIncorrectWord()
    {
        var incorrectWord = new IncorrectWord
        {
            WordId = 1,
            Word = "example",
            Frequency = 5
        };

        _dbContext.IncorrectWords.Add(incorrectWord);
        await _dbContext.SaveChangesAsync();
        
        _dbContext.IncorrectWords.Remove(incorrectWord);
        await _dbContext.SaveChangesAsync();
        
        var deletedWord = await _dbContext.IncorrectWords.FirstOrDefaultAsync(w => w.WordId == 1);
        Assert.IsNull(deletedWord);
    }
}