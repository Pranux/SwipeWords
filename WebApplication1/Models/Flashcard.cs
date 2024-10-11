using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
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

        Save(databaseContext);
    }
    ~Flashcard() //TODO: would be nice to have a db instead of json; that's just a temporary solution; save and retrieve parts as well
    {
    }
    public List<string> GetMixedWords()
    {
        var random = new Random();
        return CorrectWords.Concat(IncorrectWords).OrderBy(x => random.Next()).ToList();
    }
    
    public static (int score, List<string> correctWords, List<string> incorrectWords) CalculateScore(
        List<string> userCorrect,
        List<string> userIncorrect,
        Guid flashcardId,
        FlashcardGameDatabaseContext databaseContext)
    {
        
        var correctWords = GetCorrectWordsById(flashcardId, databaseContext);
        var incorrectWords = GetIncorrectWordsById(flashcardId, databaseContext);
        
        var correctMatches = correctWords.Intersect(userCorrect).Count();
        var incorrectMatches = incorrectWords.Intersect(userIncorrect).Count();
        var score = correctMatches + incorrectMatches;
        
        return (score, correctWords, incorrectWords);
    }
    
    private void Save(FlashcardGameDatabaseContext databaseContext) //TODO: needs a database. same for nex 2 functions
    {
        var flashcardEntity = new FlashcardEntity
        {
            Id = this.Id,
            CorrectWords = string.Join(",", this.CorrectWords),
            IncorrectWords = string.Join(",", this.IncorrectWords)
        };
        databaseContext.Add(flashcardEntity);
        databaseContext.SaveChanges();
        
    }

    public static List<string> GetCorrectWordsById(Guid id, FlashcardGameDatabaseContext databaseContext)
    {
        var flashcardEntity = databaseContext.Flashcards.Find(id);
        if (flashcardEntity == null)
        {
            throw new Exception("Flashcard not found");
        }
        return flashcardEntity.CorrectWords.Split(",").ToList();
    }

    public static List<string> GetIncorrectWordsById(Guid id, FlashcardGameDatabaseContext databaseContext)
    {
        var flashcardEntity = databaseContext.Flashcards.Find(id);
        if (flashcardEntity == null)
        {
            throw new Exception("Flashcard not found");
        }
        return flashcardEntity.IncorrectWords.Split(",").ToList();
    }
}