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
        Save();
    }
    public List<string> GetMixedWords()
    {
        var random = new Random();
        return CorrectWords.Concat(IncorrectWords).OrderBy(x => random.Next()).ToList();
    }
    
    public static (int score, List<string> correctWords, List<string> incorrectWords) CalculateScore(List<string> userCorrect, List<string> userIncorrect, Guid flashcardId)
    {
        
        var correctWords = GetCorrectWordsById(flashcardId);
        var incorrectWords = GetIncorrectWordsById(flashcardId);
        
        var correctMatches = correctWords.Intersect(userCorrect).Count();
        var incorrectMatches = incorrectWords.Intersect(userIncorrect).Count();
        var score = correctMatches + incorrectMatches;
        
        return (score, correctWords, incorrectWords);
    }
    
    private void Save() //TODO: needs a database. same for nex 2 functions
    {
        
    }
    
    public static List<string> GetCorrectWordsById(Guid id)
    {
        return null!;
    }

    public static List<string> GetIncorrectWordsById(Guid id)
    {
        return null!;
    }
}