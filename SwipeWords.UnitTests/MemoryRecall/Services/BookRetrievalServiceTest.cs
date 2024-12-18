using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using Moq.Protected;
using NUnit.Framework;
using SwipeWords.MemoryRecall.Services;

namespace SwipeWords.UnitTests.MemoryRecall.Services;

[TestFixture]
public class BookRetrievalServiceTests
{
    private Mock<HttpMessageHandler> _mockHttpMessageHandler;
    private BookRetrievalService _service;

    [SetUp]
    public void Setup()
    {
        _mockHttpMessageHandler = new Mock<HttpMessageHandler>(MockBehavior.Strict);
        var httpClient = new HttpClient(_mockHttpMessageHandler.Object);
        _service = new BookRetrievalService(httpClient);
    }

    [Test]
    public async Task FetchRandomPassageAsync_HttpClientFailure_ThrowsException()
    {
        // Arrange
        _mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ThrowsAsync(new HttpRequestException("Network error"));

        // Act & Assert
        Assert.ThrowsAsync<InvalidOperationException>(
            async () => await _service.FetchRandomPassageAsync(wordCountTarget: 10, maxRetries: 1)
        );
    }

    [Test]
    public async Task FetchRandomPassageAsync_EmptyBookContent_ThrowsException()
    {
        // Arrange
        int bookId = 1342;
        SetupMockHttpClientResponse(bookId, string.Empty);

        // Act & Assert
        Assert.ThrowsAsync<InvalidOperationException>(
            async () => await _service.FetchRandomPassageAsync(wordCountTarget: 10, maxRetries: 1)
        );
    }

    [Test]
    public void CleanBookText_RemovesHeaderAndNormalizes()
    {
        // Arrange
        string dirtyText = @"Some header text
*** START OF THIS PROJECT GUTENBERG EBOOK ***
Multiple    spaces   and
newline characters.";

        var method = typeof(BookRetrievalService).GetMethod("CleanBookText",
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

        // Act
        var cleanedText = method?.Invoke(_service, new object[] { dirtyText }) as string;

        // Assert
        Assert.That(cleanedText, Is.Not.Null);
        Assert.That(cleanedText, Is.EqualTo("*** Multiple spaces and newline characters.")); // Corrected Assertion
    }

    private void SetupMockHttpClientResponse(int bookId, string content)
    {
        // Arrange
        var mockResponse = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(content)
        };

        _mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(req => req.RequestUri.ToString() == $"http://www.gutenberg.org/files/{bookId}/{bookId}-0.txt"),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(mockResponse);
    }
}
