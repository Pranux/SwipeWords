namespace SwipeWords.Data;

public class ExternalApiService
{
    private const string FileUrl = "https://www.dcs.bbk.ac.uk/~ROGER/wikipedia.dat";
    private const int TotalLines = 4473; // Known total number of lines

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
            bool wrappedAround = false;

            while (wordSet.Count < count)
            {
                if (currentLine == TotalLines)
                {
                    currentLine = 0;
                    wrappedAround = true;
                    reader.DiscardBufferedData();
                    reader.BaseStream.Seek(0, SeekOrigin.Begin);
                }

                var line = await reader.ReadLineAsync();
                if (line == null) break;

                if (currentLine >= startLine || wrappedAround)
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