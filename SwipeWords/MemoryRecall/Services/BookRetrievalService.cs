using System.Collections.Concurrent;
using System.Text.RegularExpressions;

namespace SwipeWords.MemoryRecall.Services;

public interface IBookRetrievalService
{
    Task<string> FetchRandomPassageAsync(int wordCountTarget, int maxRetries = 5);
}

public class BookRetrievalService : IBookRetrievalService
{
    private readonly HttpClient _httpClient;
    private static readonly List<int> RandomBookIds =
        [1342, 1661, 11, 2701, 74, 1232, 1952, 345, 5200, 4300, 84, 1400, 160, 23];

    private readonly ConcurrentDictionary<int, string> _cachedBooks = new();

    public BookRetrievalService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<string> FetchRandomPassageAsync(int wordCountTarget, int maxRetries = 5)
    {
        var random = new Random();

        for (int retry = 0; retry < maxRetries; retry++)
        {
            try
            {
                int bookId = RandomBookIds[random.Next(RandomBookIds.Count)];

                string bookContent = await GetBookContentAsync(bookId);
                if (string.IsNullOrEmpty(bookContent))
                    throw new InvalidOperationException("Failed to retrieve book content.");

                string cleanText = CleanBookText(bookContent);
                var sentences = SplitIntoSentences(cleanText);

                var passage = ExtractRandomPassage(sentences, wordCountTarget, random);
                if (!string.IsNullOrEmpty(passage))
                    return passage;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Attempt {retry + 1}/{maxRetries} failed: {ex.Message}");
            }
        }

        throw new InvalidOperationException("Could not extract a meaningful passage after filtering.");
    }

    private async Task<string> GetBookContentAsync(int bookId)
    {
        if (_cachedBooks.TryGetValue(bookId, out var cachedContent))
            return cachedContent;

        string url = $"http://www.gutenberg.org/files/{bookId}/{bookId}-0.txt";

        using var response = await _httpClient.GetAsync(url);
        if (!response.IsSuccessStatusCode)
            throw new InvalidOperationException("Failed to fetch book content from the server.");

        var bookContent = await response.Content.ReadAsStringAsync();
        _cachedBooks[bookId] = bookContent;
        return bookContent;
    }

    private string CleanBookText(string text)
    {
        if (text.Contains("*** START OF THIS PROJECT GUTENBERG EBOOK"))
        {
            text = text.Split("*** START OF THIS PROJECT GUTENBERG EBOOK")[1];
        }

        text = Regex.Replace(text, @"[\r\n]+", " ");
        text = Regex.Replace(text, @"\s{2,}", " ");

        return text.Trim();
    }

    private List<string> SplitIntoSentences(string text)
    {
        return Regex.Split(
            text,
            @"(?<!\w\.\w.)(?<![A-Z][a-z]\.)(?<=\.|\?)\s"
        ).Where(s => !string.IsNullOrWhiteSpace(s)).ToList();
    }

    private static string ExtractRandomPassage(List<string> sentences, int wordCountTarget, Random random)
    {
        while (sentences.Count > 10)
        {
            var startIndex = random.Next(sentences.Count - 10);
            var passage = new List<string>();
            var wordCount = 0;

            foreach (var sentence in sentences.Skip(startIndex))
            {
                passage.Add(sentence.Trim());
                wordCount += sentence.Split(' ', StringSplitOptions.RemoveEmptyEntries).Length;

                if (wordCount < wordCountTarget) continue;
                var finalPassage = string.Join(" ", passage).Trim();

                if (!finalPassage.Contains("CHAPTER") && !finalPassage.Contains("[Illustration]"))
                    return finalPassage;
            }

            sentences = sentences.Skip(startIndex + 10).ToList();
        }

        return null;
    }
}