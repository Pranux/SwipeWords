using System.Net;
using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Moq.Protected;
using NUnit.Framework;
using SwipeWords.Services;
using System.Text;

namespace SwipeWords.IntegrationTests.Services
{
    [TestFixture]
    [TestOf(typeof(ExternalApiService))]
    public class ExternalApiServiceIntegrationTest
    {
        private ExternalApiService _externalApiService;
        private Mock<HttpMessageHandler> _mockHttpMessageHandler;
        private HttpClient _mockHttpClient;

        [SetUp]
        public void SetUp()
        {
            // Setup mock HttpMessageHandler
            _mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            _mockHttpClient = new HttpClient(_mockHttpMessageHandler.Object)
            {
                BaseAddress = new Uri("https://www.dcs.bbk.ac.uk/~roger/")
            };

            // Initialize the ExternalApiService with the mock HttpClient
            _externalApiService = new ExternalApiService(_mockHttpClient);
        }

        [TearDown]
        public void TearDown()
        {
            _mockHttpClient.Dispose();
        }
        
        [Test]
        public async Task GetCorrectWords_WhenSourceContainsWords_ShouldReturnCorrectWords()
        {
            // Arrange
            var mockResponseContent = "$correct1\n$correct2\n$correct3\ncorrect4\ncorrect5";
            SetupMockHttpResponse(mockResponseContent);

            // Act
            var correctWords = await _externalApiService.GetCorrectWordsAsync(3);

            // Assert
            Assert.AreEqual(3, correctWords.Count);
            Assert.IsTrue(correctWords.All(w => !string.IsNullOrWhiteSpace(w)));
        }

        [Test]
        public async Task GetIncorrectWords_WhenSourceContainsWords_ShouldReturnIncorrectWords()
        {
            // Arrange
            var mockResponseContent = "incorrect1\nincorrect2\n$correct3\nincorrect4\nincorrect5";
            SetupMockHttpResponse(mockResponseContent);

            // Act
            var incorrectWords = await _externalApiService.GetIncorrectWordsAsync(3);

            // Assert
            Assert.AreEqual(3, incorrectWords.Count);
            Assert.IsTrue(incorrectWords.All(w => !string.IsNullOrWhiteSpace(w)));
        }

        [Test]
        public async Task GetWordsWithScalingMode_ShouldReturnWordsFromDifferentDifficulties()
        {
            // Arrange
            var mockEasyResponseContent = "$easy1\n$easy2\n$easy3";
            var mockMediumResponseContent = "$medium1\n$medium2\n$medium3";
            var mockHardResponseContent = "$hard1\n$hard2\n$hard3";

            // Setup multiple mock responses for different difficulty sources
            SetupMockHttpResponse(mockEasyResponseContent, WordSource.Difficulties.Easy);
            SetupMockHttpResponse(mockMediumResponseContent, WordSource.Difficulties.Medium);
            SetupMockHttpResponse(mockHardResponseContent, WordSource.Difficulties.Hard);

            // Act
            var scaledWords = await _externalApiService.GetCorrectWordsAsync(9, useScalingMode: true);

            // Assert
            Assert.AreEqual(9, scaledWords.Count);
            
            // Verify distribution across difficulties (approximately)
            var easyWords = scaledWords.Take(3).ToList();
            var mediumWords = scaledWords.Skip(3).Take(3).ToList();
            var hardWords = scaledWords.Skip(6).Take(3).ToList();

            Assert.IsTrue(easyWords.All(w => !string.IsNullOrWhiteSpace(w)));
            Assert.IsTrue(mediumWords.All(w => !string.IsNullOrWhiteSpace(w)));
            Assert.IsTrue(hardWords.All(w => !string.IsNullOrWhiteSpace(w)));
        }

        [Test]
        public void ResetUsedWords_ShouldClearPreviouslyUsedWords()
        {
            // Arrange
            var mockResponseContent = "$correct1\n$correct2\n$correct3";
            SetupMockHttpResponse(mockResponseContent);

            // Act & Assert
            // First, get some words
            var initialWords = _externalApiService.GetCorrectWordsAsync(2).Result;
            Assert.AreEqual(2, initialWords.Count);

            // Reset used words
            _externalApiService.ResetUsedWords();

            // Get words again - should be able to get the same words
            var subsequentWords = _externalApiService.GetCorrectWordsAsync(2).Result;
            Assert.AreEqual(2, subsequentWords.Count);
        }

        private void SetupMockHttpResponse(string responseContent, WordSource.Difficulties difficulty = WordSource.Difficulties.Hard)
        {
            var url = difficulty switch
            {
                WordSource.Difficulties.Easy => "https://www.dcs.bbk.ac.uk/~roger/aspell.dat",
                WordSource.Difficulties.Medium => "https://www.dcs.bbk.ac.uk/~roger/missp.dat",
                WordSource.Difficulties.Hard => "https://www.dcs.bbk.ac.uk/~roger/wikipedia.dat",
                _ => "https://www.dcs.bbk.ac.uk/~roger/holbrook-missp.dat"
            };

            _mockHttpMessageHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req => req.RequestUri.ToString() == url),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(responseContent, Encoding.UTF8)
                });
        }
    }
}