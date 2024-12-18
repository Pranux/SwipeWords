using System;
using System.Collections.Generic;
using NUnit.Framework;
using SwipeWords.MemoryRecall.Services;

namespace SwipeWords.UnitTests.MemoryRecall.Services
{
    [TestFixture]
    public class TextProcessingServiceTests
    {
        private TextProcessingService _service;

        [SetUp]
        public void Setup()
        {
            _service = new TextProcessingService();
        }

        [Test]
        public void GeneratePlaceholderPositions_ValidInput_ReturnsCorrectCount()
        {
            // Arrange
            var text = "This is a simple test string with several words.";
            double placeholderPercentage = 0.3;

            // Act
            var placeholderPositions = _service.GeneratePlaceholderPositions(text, placeholderPercentage);

            // Assert
            var expectedCount = (int)Math.Ceiling(text.Split(' ').Length * placeholderPercentage);
            Assert.That(placeholderPositions.Count, Is.EqualTo(expectedCount));
            Assert.That(placeholderPositions, Is.Unique);
            Assert.That(placeholderPositions, Has.All.InRange(0, text.Split(' ').Length - 1));
        }

        [Test]
        public void GenerateTextWithPlaceholders_ValidInput_ReplacesCorrectWords()
        {
            // Arrange
            var text = "This is a test string for placeholders.";
            var placeholderPositions = new List<int> { 1, 3 };

            // Act
            var result = _service.GenerateTextWithPlaceholders(text, placeholderPositions);

            // Assert
            var expected = "This _ a _ string for placeholders.";
            Assert.That(result, Is.EqualTo(expected));
        }

        [Test]
        public void GenerateTextWithPlaceholders_InvalidPositions_IgnoresOutOfRange()
        {
            // Arrange
            var text = "This is a test.";
            var placeholderPositions = new List<int> { 0, 10, -5 }; // Invalid positions

            // Act
            var result = _service.GenerateTextWithPlaceholders(text, placeholderPositions);

            // Assert
            var expected = "_ is a test.";
            Assert.That(result, Is.EqualTo(expected));
        }

        [Test]
        public void GetCorrectWordsFromPositions_ValidPositions_ReturnsCorrectWords()
        {
            // Arrange
            var text = "This is a test string.";
            var positions = new List<int> { 0, 2, 4 };

            // Act
            var result = TextProcessingService.GetCorrectWordsFromPositions(text, positions);

            // Assert
            var expected = new List<string> { "This", "a", "string." };
            Assert.That(result, Is.EqualTo(expected));
        }

        [Test]
        public void GetCorrectWordsFromPositions_InvalidPositions_ThrowsException()
        {
            // Arrange
            var text = "This is a test.";
            var positions = new List<int> { -1, 5 }; // Out-of-bounds positions

            // Act & Assert
            var ex = Assert.Throws<InvalidOperationException>(
                () => TextProcessingService.GetCorrectWordsFromPositions(text, positions));
            Assert.That(ex.Message, Does.Contain("Invalid placeholder position"));
        }
    }
}