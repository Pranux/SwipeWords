namespace SwipeWords.Data
{
    public class ExternalApiService
    {
        private readonly List<WordSource> _wordSources;
        private readonly HashSet<string> _usedWords;

        public ExternalApiService()
        {
            _wordSources = new List<WordSource>
            {
                new WordSource { Url = "https://www.dcs.bbk.ac.uk/~roger/holbrook-missp.dat", Difficulty = "Combined", TotalLines = 42267 },
                new WordSource { Url = "https://www.dcs.bbk.ac.uk/~roger/aspell.dat", Difficulty = "Easy", TotalLines = 981 },
                new WordSource { Url = "https://www.dcs.bbk.ac.uk/~roger/missp.dat", Difficulty = "Medium", TotalLines = 3000 },
                new WordSource { Url = "https://www.dcs.bbk.ac.uk/~roger/wikipedia.dat", Difficulty = "Difficult", TotalLines = 4377 }
            };
            _usedWords = new HashSet<string>();
        }

        public async Task<List<string>> GetCorrectWordsAsync(int count, bool useScalingMode = false, string difficulty = null)
        {
            return await GetWordsAsync(count, true, useScalingMode, difficulty);
        }

        public async Task<List<string>> GetIncorrectWordsAsync(int count, bool useScalingMode = false, string difficulty = null)
        {
            return await GetWordsAsync(count, false, useScalingMode, difficulty);
        }

        private async Task<List<string>> GetWordsAsync(int count, bool correct, bool useScalingMode, string difficulty)
        {
            if (useScalingMode)
            {
                int easyCount = count / 3;
                int mediumCount = count / 3;
                int difficultCount = count - easyCount - mediumCount;

                var easyWords = await FetchWordsFromSource(_wordSources.First(source => source.Difficulty == "Easy"), easyCount, correct);
                var mediumWords = await FetchWordsFromSource(_wordSources.First(source => source.Difficulty == "Medium"), mediumCount, correct);
                var difficultWords = await FetchWordsFromSource(_wordSources.First(source => source.Difficulty == "Difficult"), difficultCount, correct);

                return easyWords.Concat(mediumWords).Concat(difficultWords).ToList();
            }
            else
            {
                var wordSource = _wordSources.FirstOrDefault(source => source.Difficulty == difficulty) ??
                                 _wordSources.First(source => source.Difficulty == "Difficult");
                return await FetchWordsFromSource(wordSource, count, correct);
            }
        }

        private async Task<List<string>> FetchWordsFromSource(WordSource source, int count, bool correct)
        {
            using var httpClient = new HttpClient();
            var wordSet = new HashSet<string>();
            var random = new Random();
            var startLines = new List<int>();
            for (var i = 0; i < count; i++) startLines.Add(random.Next(0, source.TotalLines));

            foreach (var startLine in startLines)
            {
                using var response = await httpClient.GetAsync(source.Url);
                response.EnsureSuccessStatusCode();

                await using var stream = await response.Content.ReadAsStreamAsync();
                using var reader = new StreamReader(stream);

                var currentLine = 0;
                bool wrappedAround = false;

                while (wordSet.Count < count)
                {
                    if (currentLine == source.TotalLines)
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
                        word = RemoveTrailingNumber(word);

                        if (isCorrect == correct && !_usedWords.Contains(word) && wordSet.Add(word))
                        {
                            _usedWords.Add(word);
                            break;
                        }
                    }

                    currentLine++;
                }

                if (wordSet.Count >= count) break;
            }

            return wordSet.ToList();
        }

        private string RemoveTrailingNumber(string word)
        {
            return System.Text.RegularExpressions.Regex.Replace(word, @"\s\d+$", "");
        }

        public void ResetUsedWords()
        {
            _usedWords.Clear();
        }
    }

    public class WordSource
    {
        public string Url { get; set; }
        public string Difficulty { get; set; }
        public int TotalLines { get; set; }
    }
}