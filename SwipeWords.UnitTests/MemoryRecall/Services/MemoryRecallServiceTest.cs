using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using SwipeWords.MemoryRecall.Data;
using SwipeWords.MemoryRecall.Services;

namespace SwipeWords.UnitTests.MemoryRecall.Services
{
    [TestFixture]
    public class MemoryRecallServiceTests
    {
        private MemoryRecallDatabaseContext _dbContext;
        private MemoryRecallService _service;
        private Mock<ITextProcessingService> _mockTextProcessingService;
        private Mock<IBookRetrievalService> _mockBookRetrievalService;

        [SetUp]
        public void Setup()
        {
            // Setup in-memory database
            var options = new DbContextOptionsBuilder<MemoryRecallDatabaseContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Unique DB per test
                .Options;
            _dbContext = new MemoryRecallDatabaseContext(options);

            // Mock dependencies
            _mockTextProcessingService = new Mock<ITextProcessingService>();
            _mockBookRetrievalService = new Mock<IBookRetrievalService>();

            // Initialize the service with in-memory context
            _service = new MemoryRecallService(
                _dbContext,
                _mockTextProcessingService.Object,
                _mockBookRetrievalService.Object
            );
        }

        [TearDown]
        public void TearDown()
        {
            _dbContext.Dispose();
        }
        
        [Test]
        public async Task FetchAndSaveTextAsync_EmptyText_ThrowsException()
        {
            // Arrange
            int wordCount = 10;
            double placeholderPercentage = 0.3;

            _mockBookRetrievalService
                .Setup(s => s.FetchRandomPassageAsync(wordCount, It.IsAny<int>()))
                .ReturnsAsync(string.Empty); // Simulate an empty response

            // Act & Assert
            Assert.ThrowsAsync<InvalidOperationException>(
                async () => await _service.FetchAndSaveTextAsync(wordCount, placeholderPercentage)
            );

            // Assert that no entities were added
            Assert.That(_dbContext.SpeedReadingTexts.Count(), Is.EqualTo(0));
            Assert.That(_dbContext.UserMemoryRecalls.Count(), Is.EqualTo(0));
        }
        
        [Test]
        public async Task FetchAndSaveTextAsync_ValidText_SavesEntities()
        {
            // Arrange
            int wordCount = 10;
            double placeholderPercentage = 0.3;
            string validText = "This is a valid text passage for testing.";

            _mockBookRetrievalService
                .Setup(s => s.FetchRandomPassageAsync(wordCount, It.IsAny<int>()))
                .ReturnsAsync(validText);

            _mockTextProcessingService
                .Setup(s => s.GeneratePlaceholderPositions(validText, placeholderPercentage))
                .Returns(new List<int> { 1, 3 }); // Simulate placeholder positions

            // Act
            var (recallId, originalText) = await _service.FetchAndSaveTextAsync(wordCount, placeholderPercentage);

            // Assert
            Assert.That(recallId, Is.Not.EqualTo(Guid.Empty));
            Assert.That(originalText, Is.EqualTo(validText));

            // Verify that entities are saved correctly
            var savedText = _dbContext.SpeedReadingTexts.FirstOrDefault();
            Assert.That(savedText, Is.Not.Null);
            Assert.That(savedText.Content, Is.EqualTo(validText));

            var recallEntry = _dbContext.UserMemoryRecalls.FirstOrDefault();
            Assert.That(recallEntry, Is.Not.Null);
            Assert.That(recallEntry.SpeedReadingTextId, Is.EqualTo(savedText.SpeedReadingTextId));
            Assert.That(recallEntry.RemovedWordPositions, Is.EqualTo(new List<int> { 1, 3 }));
        }

    }
}
