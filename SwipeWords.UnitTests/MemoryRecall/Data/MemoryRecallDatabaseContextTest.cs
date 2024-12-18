using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using SwipeWords.MemoryRecall.Data;
using System;
using System.Linq;

namespace SwipeWords.UnitTests.MemoryRecall.Data
{
    [TestFixture]
    public class MemoryRecallDatabaseContextTests
    {
        private MemoryRecallDatabaseContext _context;
        private DbContextOptions<MemoryRecallDatabaseContext> _options;

        [SetUp]
        public void Setup()
        {
            // Create in-memory database options
            _options = new DbContextOptionsBuilder<MemoryRecallDatabaseContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            // Create a new context for each test
            _context = new MemoryRecallDatabaseContext(_options);
        }

        [TearDown]
        public void TearDown()
        {
            _context.Dispose();
        }

        [Test]
        public void SpeedReadingTexts_ShouldBeInitializedAndAllowAddingEntities()
        {
            // Arrange
            var speedReadingText = new SpeedReadingText
            {
                SpeedReadingTextId = Guid.NewGuid(),
                Content = "Test Content"
            };

            // Act
            _context.SpeedReadingTexts.Add(speedReadingText);
            _context.SaveChanges();

            // Assert
            Assert.That(_context.SpeedReadingTexts, Is.Not.Null);
            Assert.That(_context.SpeedReadingTexts.Count(), Is.EqualTo(1));
            Assert.That(_context.SpeedReadingTexts.First(), Is.EqualTo(speedReadingText));
        }

        [Test]
        public void UserMemoryRecalls_ShouldBeInitializedAndAllowAddingEntities()
        {
            // Arrange
            var userMemoryRecall = new UserMemoryRecall
            {
                MemoryRecallId = Guid.NewGuid(),
                SpeedReadingTextId = Guid.NewGuid(),
                RemovedWordPositions = new List<int> { 1, 3, 5 }
            };

            // Act
            _context.UserMemoryRecalls.Add(userMemoryRecall);
            _context.SaveChanges();

            // Assert
            Assert.That(_context.UserMemoryRecalls, Is.Not.Null);
            Assert.That(_context.UserMemoryRecalls.Count(), Is.EqualTo(1));
            Assert.That(_context.UserMemoryRecalls.First(), Is.EqualTo(userMemoryRecall));
        }

        [Test]
        public void Constructor_ShouldAcceptDbContextOptions()
        {
            // Arrange & Act
            using var context = new MemoryRecallDatabaseContext(_options);

            // Assert
            Assert.That(context, Is.Not.Null);
            Assert.That(context, Is.InstanceOf<DbContext>());
            Assert.That(context, Is.InstanceOf<IMemoryRecallDatabaseContext>());
        }

        [Test]
        public void DbSets_ShouldSupportBasicDatabaseOperations()
        {
            // Arrange
            var speedReadingText = new SpeedReadingText
            {
                SpeedReadingTextId = Guid.NewGuid(),
                Content = "Test Content"
            };

            var userMemoryRecall = new UserMemoryRecall
            {
                MemoryRecallId = Guid.NewGuid(),
                SpeedReadingTextId = speedReadingText.SpeedReadingTextId,
                RemovedWordPositions = new List<int> { 1, 3, 5 }
            };

            // Act
            _context.SpeedReadingTexts.Add(speedReadingText);
            _context.UserMemoryRecalls.Add(userMemoryRecall);
            _context.SaveChanges();

            // Retrieve
            var retrievedText = _context.SpeedReadingTexts.Find(speedReadingText.SpeedReadingTextId);
            var retrievedRecall = _context.UserMemoryRecalls.Find(userMemoryRecall.MemoryRecallId);

            // Assert
            Assert.That(retrievedText, Is.Not.Null);
            Assert.That(retrievedRecall, Is.Not.Null);
            Assert.That(retrievedText.Content, Is.EqualTo(speedReadingText.Content));
            Assert.That(retrievedRecall.RemovedWordPositions, Is.EqualTo(userMemoryRecall.RemovedWordPositions));
        }
    }
}