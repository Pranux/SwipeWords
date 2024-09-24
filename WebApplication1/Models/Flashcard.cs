using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using WebApplication1.Data;
using JsonSerializer = Newtonsoft.Json.JsonSerializer;


namespace WebApplication1.Models;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;  

public class Flashcard
{
    public Guid Id { get;}     
    private List<string> CorrectWords { get; }
    private List<string> IncorrectWords { get;} 
    
    public Flashcard(FlashcardGameDatabaseContext databaseContext, int wordCount = 5)
    {
        Id = Guid.NewGuid(); 
        var random = new Random();

        var numCorrectWords = random.Next((int)(wordCount * 0.25), (int)(wordCount * 0.75));
        
        CorrectWords = databaseContext.GetSelectedCorrectWords(numCorrectWords);
        IncorrectWords = databaseContext.GetSelectedIncorrectWords(wordCount-numCorrectWords);
    }
    ~Flashcard() //TODO: would be nice to have a db instead of json; that's just a temporary solution; save and retrieve parts as well
    {
        SaveToJson();
    }
    public List<string> GetMixedWords()
    {
        var random = new Random();
        return CorrectWords.Concat(IncorrectWords).OrderBy(x => random.Next()).ToList();
    }
    
    public static int CalculateScore (List<string> userCorrect, List<string> userIncorrect, Guid flashcardId)
    {

        var correctWords = GetCorrectWordsById(flashcardId);
        var incorrectWords = GetIncorrectWordsById(flashcardId);
        var correctMatches = correctWords.Intersect(userCorrect).Count();
        var incorrectMatches = incorrectWords.Intersect(userIncorrect).Count();
        
        return correctMatches + incorrectMatches;
    }
    
    private void SaveToJson()
    {
  
        var jsonFilePath = Path.Combine(Directory.GetCurrentDirectory(), "flashcards.json");
        
        var flashcardData = new
        {
            Id = this.Id,
            CorrectWords = this.CorrectWords,
            IncorrectWords = this.IncorrectWords
        };
        
        var json = JsonSerializer.Serialize(flashcardData, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(jsonFilePath, json);
    }
    
    public static List<string> GetCorrectWordsById(Guid id)
    {
        var jsonFilePath = Path.Combine(Directory.GetCurrentDirectory(), "flashcards.json");

        if (!File.Exists(jsonFilePath)) return null!;
        
        var jsonContent = File.ReadAllText(jsonFilePath);
        var flashcard = JsonSerializer.Deserialize<Flashcard>(jsonContent);

        if (flashcard != null && flashcard.Id == id)
        {
            return flashcard.CorrectWords;
        }
        return null!;
    }

    public static List<string> GetIncorrectWordsById(Guid id)
    {
        var jsonFilePath = Path.Combine(Directory.GetCurrentDirectory(), "flashcards.json");

        if (!File.Exists(jsonFilePath)) return null!;
        
        var jsonContent = File.ReadAllText(jsonFilePath);
        var flashcard = JsonSerializer.Deserialize<Flashcard>(jsonContent);

        if (flashcard != null && flashcard.Id == id)
        {
            return flashcard.IncorrectWords;
        }
        return null!;
    }
    
    
    
}