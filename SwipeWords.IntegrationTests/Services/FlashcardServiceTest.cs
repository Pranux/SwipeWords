using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SwipeWords.Data;
using SwipeWords.Models;
using SwipeWords.Services;

namespace SwipeWords.IntegrationTests.Services
{
    [TestFixture]
    [TestOf(typeof(FlashcardService))]
    public class FlashcardServiceIntegrationTest
    {
        private FlashcardGameDatabaseContext _dbContext;
        private Mock<ILogger<FlashcardService>> _mockLogger;
        private Mock<IExternalApiService> _mockExternalApiService;
        private FlashcardService _flashcardService;

        [SetUp]
        public void SetUp()
        {
            // Setup in-memory database
            var options = new DbContextOptionsBuilder<FlashcardGameDatabaseContext>()
                .UseInMemoryDatabase(databaseName: "FlashcardServiceTestDb")
                .Options;

            _dbContext = new FlashcardGameDatabaseContext(options);
            _dbContext.Database.EnsureCreated();

            // Setup mock logger
            _mockLogger = new Mock<ILogger<FlashcardService>>();

            // Setup mock ExternalApiService
            _mockExternalApiService = new Mock<IExternalApiService>();

            // Provide mock implementations for GetCorrectWordsAsync and GetIncorrectWordsAsync
            _mockExternalApiService
                .Setup(s => s.GetCorrectWordsAsync(It.IsAny<int>(), It.IsAny<bool>(), It.IsAny<WordSource.Difficulties>()))
                .ReturnsAsync((int count, bool _, WordSource.Difficulties __) =>
                    Enumerable.Range(1, count).Select(i => $"correct{i}").ToList());

            _mockExternalApiService
                .Setup(s => s.GetIncorrectWordsAsync(It.IsAny<int>(), It.IsAny<bool>(), It.IsAny<WordSource.Difficulties>()))
                .ReturnsAsync((int count, bool _, WordSource.Difficulties __) =>
                    Enumerable.Range(1, count).Select(i => $"incorrect{i}").ToList());

            // Create flashcard service
            _flashcardService = new FlashcardService(_dbContext, _mockLogger.Object);
        }

        [TearDown]
        public void TearDown()
        {
            _dbContext.Database.EnsureDeleted();
            _dbContext.Dispose();
        }

        [Test]
        public async Task GetFlashcardsAsync_WithDefaultParameters_ShouldCreateFlashcard()
        {
            // Arrange
            int wordCount = 6;
            bool useScalingMode = false;
            var difficulty = WordSource.Difficulties.Medium;

            // Act
            var flashcard = await _flashcardService.GetFlashcardsAsync(wordCount, useScalingMode, difficulty);

            // Assert
            Assert.IsNotNull(flashcard);
            Assert.IsNotNull(flashcard.Id);
            Assert.AreEqual(wordCount, flashcard.CorrectWords.Count + flashcard.IncorrectWords.Count);

            // Verify database persistence
            var savedFlashcard = await _dbContext.Flashcards.FirstOrDefaultAsync(f => f.Id == flashcard.Id);
            Assert.IsNotNull(savedFlashcard);
        }

        [Test]
        public async Task GetFlashcardsAsync_WithScalingMode_ShouldCreateFlashcardWithVariedDifficulty()
        {
            // Arrange
            int wordCount = 12;
            bool useScalingMode = true;
            var difficulty = WordSource.Difficulties.Combined;

            // Act
            var flashcard = await _flashcardService.GetFlashcardsAsync(wordCount, useScalingMode, difficulty);

            // Assert
            Assert.IsNotNull(flashcard);
            Assert.IsNotNull(flashcard.Id);
            Assert.AreEqual(wordCount, flashcard.CorrectWords.Count + flashcard.IncorrectWords.Count);

            // Verify database persistence
            var savedFlashcard = await _dbContext.Flashcards.FirstOrDefaultAsync(f => f.Id == flashcard.Id);
            Assert.IsNotNull(savedFlashcard);
        }

        [Test]
        public void CalculateScore_WithCorrectAndIncorrectWords_ShouldCalculateAccurately()
        {
            // Arrange
            var flashcard = new Flashcard();
            var correctWords = new List<string> { "correct1", "correct2", "correct3" };
            var incorrectWords = new List<string> { "incorrect1", "incorrect2" };
    
            flashcard.SetWords(correctWords, incorrectWords);

            _dbContext.Flashcards.Add(new FlashcardEntity
            {
                Id = flashcard.Id,
                CorrectWords = string.Join(",", correctWords),
                IncorrectWords = string.Join(",", incorrectWords)
            });
            _dbContext.SaveChanges();

            var userCorrectWords = new List<string> { "correct1", "correct3" };
            var userIncorrectWords = new List<string> { "incorrect1" };

            // Act
            var (score, correctWordMatches, incorrectWordMatches) = Flashcard.CalculateScore(
                userCorrectWords,
                userIncorrectWords,
                flashcard.Id,
                _dbContext
            );

            // Assert
            Assert.AreEqual(3, score);
            Assert.AreEqual(3, correctWordMatches.Count);
            Assert.AreEqual(2, incorrectWordMatches.Count);
        }

        [Test]
        public void CalculateScore_WithNoCorrectWords_ShouldHaveZeroScore()
        {
            // Arrange
            var flashcard = new Flashcard();
            var correctWords = new List<string> { "correct1", "correct2" };
            var incorrectWords = new List<string> { "incorrect1", "incorrect2" };

            flashcard.SetWords(correctWords, incorrectWords);

            _dbContext.Flashcards.Add(new FlashcardEntity
            {
                Id = flashcard.Id,
                CorrectWords = string.Join(",", correctWords),
                IncorrectWords = string.Join(",", incorrectWords)
            });
            _dbContext.SaveChanges();

            var userCorrectWords = new List<string>();
            var userIncorrectWords = new List<string> { "nonexistent1", "nonexistent2" };

            // Act
            var (score, correctWordMatches, incorrectWordMatches) = Flashcard.CalculateScore(
                userCorrectWords,
                userIncorrectWords,
                flashcard.Id,
                _dbContext
            );

            // Assert
            Assert.AreEqual(0, score);
            Assert.AreEqual(2, correctWordMatches.Count);
            Assert.AreEqual(2, incorrectWordMatches.Count);
        }
    }
}