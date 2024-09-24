using WebApplication1.Data;

namespace WebApplication1.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

public static class DataSeederWords
{
    public static List<CorrectWord> GetCorrectWordsFromFile(string filePath)
    {
        var jsonData = File.ReadAllText(filePath);
        return JsonConvert.DeserializeObject<List<CorrectWord>>(jsonData);
    }

    public static List<IncorrectWord> GetIncorrectWordsFromFile(string filePath)
    {
        var jsonData = File.ReadAllText(filePath);
        return JsonConvert.DeserializeObject<List<IncorrectWord>>(jsonData);
    }
}