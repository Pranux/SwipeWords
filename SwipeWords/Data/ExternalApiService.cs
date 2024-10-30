
namespace SwipeWords.Data;

public class ExternalApiService
{
    private const string FileUrl = "https://www.dcs.bbk.ac.uk/~ROGER/wikipedia.dat";

    private const int TotalLines = 4473; // Known total number of lines
                                         // I know that it's not the best approach
                                         // but apparently, there is no api for this purposes

    public async Task<List<string>> GetCorrectWordsAsync(int count)
    {
        return await GetWordsAsync(count, true);
    }

    public async Task<List<string>> GetIncorrectWordsAsync(int count)
    {
        return await GetWordsAsync(count, false);
    }

    private async Task<List<string>> GetWordsAsync(int count, bool correct)
    {
        using var httpClient = new HttpClient();
        var wordSet = new HashSet<string>();
        var random = new Random();

        var startLines = new List<int>();
        for (var i = 0; i < count; i++) startLines.Add(random.Next(0, TotalLines));

        foreach (var startLine in startLines)
        {
            using var response = await httpClient.GetAsync(FileUrl);
            response.EnsureSuccessStatusCode();

            await using var stream = await response.Content.ReadAsStreamAsync();
            using var reader = new StreamReader(stream);

            var currentLine = 0;

            while (await reader.ReadLineAsync() is { } line)
            {
                if (currentLine >= startLine)
                {
                    var isCorrect = line.StartsWith("$");
                    var word = isCorrect ? line.Substring(1) : line;

                    if (isCorrect == correct && wordSet.Add(word)) break;
                }

                currentLine++;
            }

            if (wordSet.Count >= count)
                break;
        }

        return wordSet.ToList();
    }
}