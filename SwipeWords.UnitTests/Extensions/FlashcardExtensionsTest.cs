using Moq;
using SwipeWords.Data;
using SwipeWords.Extensions;
using Microsoft.EntityFrameworkCore;

namespace SwipeWords.UnitTests.Extensions;

[TestFixture]
[TestOf(typeof(FlashcardExtensions))]
public class FlashcardExtensionsTests
{
    private Mock<DbSet<FlashcardEntity>> _mockFlashcards;
    private Mock<FlashcardGameDatabaseContext> _mockContext;

    [SetUp]
    public void SetUp()
    {
        _mockFlashcards = new Mock<DbSet<FlashcardEntity>>();
        _mockContext = new Mock<FlashcardGameDatabaseContext>(new DbContextOptions<FlashcardGameDatabaseContext>());
        _mockContext.Setup(m => m.Flashcards).Returns(_mockFlashcards.Object);
    }

    [Test]
    public void GetCorrectWordsById_FlashcardExists_ReturnsCorrectWordsList()
    {
        var flashcardId = Guid.NewGuid();
        var flashcardEntity = new FlashcardEntity
        {
            Id = flashcardId,
            CorrectWords = "word1,word2,word3",
            IncorrectWords = "wrong1,wrong2"
        };

        var flashcards = new List<FlashcardEntity> { flashcardEntity }.AsQueryable();

        _mockFlashcards.As<IQueryable<FlashcardEntity>>()
            .Setup(m => m.Provider).Returns(flashcards.Provider);
        _mockFlashcards.As<IQueryable<FlashcardEntity>>()
            .Setup(m => m.Expression).Returns(flashcards.Expression);
        _mockFlashcards.As<IQueryable<FlashcardEntity>>()
            .Setup(m => m.ElementType).Returns(flashcards.ElementType);
        _mockFlashcards.As<IQueryable<FlashcardEntity>>()
            .Setup(m => m.GetEnumerator()).Returns(flashcards.GetEnumerator());

        _mockFlashcards.Setup(m => m.Find(flashcardId)).Returns(flashcardEntity);
        
        var result = _mockContext.Object.GetCorrectWordsById(flashcardId);
        
        Assert.AreEqual(3, result.Count);
        Assert.AreEqual("word1", result[0]);
        Assert.AreEqual("word2", result[1]);
        Assert.AreEqual("word3", result[2]);
    }

    [Test]
    public void GetCorrectWordsById_FlashcardDoesNotExist_ThrowsException()
    {
        var flashcardId = Guid.NewGuid();

        _mockFlashcards.Setup(m => m.Find(flashcardId)).Returns((FlashcardEntity)null);
        
        Assert.Throws<Exception>(() => _mockContext.Object.GetCorrectWordsById(flashcardId), "Flashcard not found");
    }

    [Test]
    public void GetIncorrectWordsById_FlashcardExists_ReturnsIncorrectWordsList()
    {
        var flashcardId = Guid.NewGuid();
        var flashcardEntity = new FlashcardEntity
        {
            Id = flashcardId,
            CorrectWords = "word1,word2,word3",
            IncorrectWords = "wrong1,wrong2"
        };

        var flashcards = new List<FlashcardEntity> { flashcardEntity }.AsQueryable();

        _mockFlashcards.As<IQueryable<FlashcardEntity>>()
            .Setup(m => m.Provider).Returns(flashcards.Provider);
        _mockFlashcards.As<IQueryable<FlashcardEntity>>()
            .Setup(m => m.Expression).Returns(flashcards.Expression);
        _mockFlashcards.As<IQueryable<FlashcardEntity>>()
            .Setup(m => m.ElementType).Returns(flashcards.ElementType);
        _mockFlashcards.As<IQueryable<FlashcardEntity>>()
            .Setup(m => m.GetEnumerator()).Returns(flashcards.GetEnumerator());

        _mockFlashcards.Setup(m => m.Find(flashcardId)).Returns(flashcardEntity);
        
        var result = _mockContext.Object.GetIncorrectWordsById(flashcardId);
        
        Assert.AreEqual(2, result.Count);
        Assert.AreEqual("wrong1", result[0]);
        Assert.AreEqual("wrong2", result[1]);
    }

    [Test]
    public void GetIncorrectWordsById_FlashcardDoesNotExist_ThrowsException()
    {
        var flashcardId = Guid.NewGuid();

        _mockFlashcards.Setup(m => m.Find(flashcardId)).Returns((FlashcardEntity)null);
        
        Assert.Throws<Exception>(() => _mockContext.Object.GetIncorrectWordsById(flashcardId), "Flashcard not found");
    }
}