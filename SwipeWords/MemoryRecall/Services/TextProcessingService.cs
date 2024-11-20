namespace SwipeWords.MemoryRecall.Services;

public class TextProcessingService
{
    public List<int> GeneratePlaceholderPositions(string text, double placeholderPercentage)
    {
        var words = text.Split(' ').ToList();
        var wordCount = words.Count;
        var placeholderCount = (int)Math.Ceiling(wordCount * placeholderPercentage);

        var random = new Random();
        var placeholderPositions = new HashSet<int>();

        while (placeholderPositions.Count < placeholderCount)
        {
            var position = random.Next(wordCount);
            placeholderPositions.Add(position);
        }

        return placeholderPositions.OrderBy(p => p).ToList();
    }

    public string GenerateTextWithPlaceholders(string text, List<int> placeholderPositions)
    {
        var words = text.Split(' ').ToArray();

        foreach (var pos in placeholderPositions.Where(pos => pos >= 0 && pos < words.Length))
        {
            words[pos] = "_";
        }

        return string.Join(" ", words);
    }

    public static List<string> GetCorrectWordsFromPositions(string text, List<int> removedWordPositions)
    {
        var words = text.Split(' ').ToList();
        var correctWords = new List<string>();

        foreach (var position in removedWordPositions)
        {
            if (position < 0 || position >= words.Count)
            {
                throw new InvalidOperationException($"Invalid placeholder position: {position}");
            }
            correctWords.Add(words[position]);
        }

        return correctWords;
    }
}