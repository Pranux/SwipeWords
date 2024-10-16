using Newtonsoft.Json;
using SwipeWords.Data;

namespace SwipeWords.Models;

public static class DataSeederWords
{
    public static List<CorrectWord> GetCorrectWordsFromFile(string filePath)
    {
        using (var streamReader = new StreamReader(filePath))
        {
            var jsonData = streamReader.ReadToEnd();
            return JsonConvert.DeserializeObject<List<CorrectWord>>(jsonData);
        }
    }

    public static List<IncorrectWord> GetIncorrectWordsFromFile(string filePath)
    {
        using (var streamReader = new StreamReader(filePath))
        {
            var jsonData = streamReader.ReadToEnd();
            return JsonConvert.DeserializeObject<List<IncorrectWord>>(jsonData);
        }
    }
}