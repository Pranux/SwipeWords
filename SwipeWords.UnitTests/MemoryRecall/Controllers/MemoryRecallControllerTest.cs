using Moq;
using NUnit.Framework;
using Microsoft.AspNetCore.Mvc;
using SwipeWords.MemoryRecall.Controllers;
using SwipeWords.MemoryRecall.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SwipeWords.UnitTests.MemoryRecall.Controllers
{
    [TestFixture]
    [TestOf(typeof(MemoryRecallController))]
    public class MemoryRecallControllerTests
    {
        private Mock<IMemoryRecallService> _mockMemoryRecallService;
        private MemoryRecallController _controller;

        [SetUp]
        public void SetUp()
        {
            _mockMemoryRecallService = new Mock<IMemoryRecallService>();
            _controller = new MemoryRecallController(_mockMemoryRecallService.Object);
        }

        [Test]
        public async Task FetchAndProcess_ValidParameters_ReturnsOkWithTextIdAndOriginalText()
        {
            // Arrange
            var expectedTextId = Guid.NewGuid();
            var expectedOriginalText = "Some sample text";
            int expectedWordCount = 200;
            double expectedPlaceholderPercentage = 25.0;

            // Specifically set up the method with the exact parameters
            _mockMemoryRecallService
                .Setup(service => service.FetchAndSaveTextAsync(expectedWordCount, expectedPlaceholderPercentage / 100))
                .ReturnsAsync((expectedTextId, expectedOriginalText));

            // Act
            var result = await _controller.FetchAndProcess(expectedWordCount, expectedPlaceholderPercentage);

            // Assert
            Assert.That(result, Is.TypeOf<OkObjectResult>());
            var okResult = result as OkObjectResult;
            var resultValue = okResult?.Value;
            
            // Use reflection to access dynamic properties
            var textIdProperty = resultValue?.GetType().GetProperty("textId");
            var originalTextProperty = resultValue?.GetType().GetProperty("originalText");
            
            Assert.That(textIdProperty?.GetValue(resultValue), Is.EqualTo(expectedTextId));
            Assert.That(originalTextProperty?.GetValue(resultValue), Is.EqualTo(expectedOriginalText));

            // Verify the method was called with the correct parameters
            _mockMemoryRecallService.Verify(
                service => service.FetchAndSaveTextAsync(expectedWordCount, expectedPlaceholderPercentage / 100), 
                Times.Once
            );
        }

        [Test]
        public void GetPlaceholderText_ReturnsOkResult_WithPlaceholderText()
        {
            // Arrange
            var textId = Guid.NewGuid();
            var expectedPlaceholderText = "This is a text with placeholders";
            _mockMemoryRecallService
                .Setup(service => service.GetTextWithPlaceholders(textId))
                .Returns(expectedPlaceholderText);

            // Act
            var result = _controller.GetPlaceholderText(textId);

            // Assert
            Assert.That(result, Is.TypeOf<OkObjectResult>());
            var okResult = result as OkObjectResult;
            var resultValue = okResult?.Value;
            
            // Use reflection to access the property
            var placeholderTextProperty = resultValue?.GetType().GetProperty("placeholderText");
            Assert.That(placeholderTextProperty?.GetValue(resultValue), Is.EqualTo(expectedPlaceholderText));
        }

        [Test]
        public void GetPlaceholderText_ReturnsNotFound_WhenTextNotFound()
        {
            // Arrange
            var textId = Guid.NewGuid();
            _mockMemoryRecallService
                .Setup(service => service.GetTextWithPlaceholders(textId))
                .Throws(new InvalidOperationException("Text not found"));

            // Act
            var result = _controller.GetPlaceholderText(textId);

            // Assert
            Assert.That(result, Is.TypeOf<NotFoundObjectResult>());
            var notFoundResult = result as NotFoundObjectResult;
            
            // Use reflection to check the error property
            var errorProperty = notFoundResult?.Value?.GetType().GetProperty("error");
            Assert.That(errorProperty?.GetValue(notFoundResult?.Value), Is.EqualTo("Text not found"));
        }

        [Test]
        public void SubmitRecall_ReturnsOkResult_WithScoreAndCorrectWords()
        {
            // Arrange
            var recallId = Guid.NewGuid();
            var userGuesses = new List<string> { "word1", "word2" };
            var expectedScore = 5;
            var expectedCorrectWords = new List<string> { "word1" };

            _mockMemoryRecallService
                .Setup(service => service.CompareUserGuesses(recallId, userGuesses))
                .Returns((expectedScore, expectedCorrectWords));

            // Act
            var result = _controller.SubmitRecall(recallId, userGuesses);

            // Assert
            Assert.That(result, Is.TypeOf<OkObjectResult>());
            var okResult = result as OkObjectResult;
            var resultValue = okResult?.Value;
            
            // Use reflection to access dynamic properties
            var scoreProperty = resultValue?.GetType().GetProperty("score");
            var correctWordsProperty = resultValue?.GetType().GetProperty("correctWords");
            
            Assert.That(scoreProperty?.GetValue(resultValue), Is.EqualTo(expectedScore));
            Assert.That(correctWordsProperty?.GetValue(resultValue), Is.EqualTo(expectedCorrectWords));
        }

        [Test]
        public void SubmitRecall_ReturnsBadRequest_WhenRecallIdIsInvalid()
        {
            // Arrange
            var recallId = Guid.NewGuid();
            var userGuesses = new List<string> { "word1", "word2" };
            _mockMemoryRecallService
                .Setup(service => service.CompareUserGuesses(recallId, userGuesses))
                .Throws(new InvalidOperationException("Recall ID is invalid"));

            // Act
            var result = _controller.SubmitRecall(recallId, userGuesses);

            // Assert
            Assert.That(result, Is.TypeOf<BadRequestObjectResult>());
            var badRequestResult = result as BadRequestObjectResult;
            
            // Use reflection to check the error property
            var errorProperty = badRequestResult?.Value?.GetType().GetProperty("error");
            Assert.That(errorProperty?.GetValue(badRequestResult?.Value), Is.EqualTo("Recall ID is invalid"));
        }
    }
}