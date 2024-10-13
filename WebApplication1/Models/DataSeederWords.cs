using WebApplication1.Data;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace WebApplication1.Models
{
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
}