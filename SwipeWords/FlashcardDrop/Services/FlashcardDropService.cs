using SwipeWords.Models;
using SwipeWords.Data;
using System;
using System.Collections.Generic;
using SwipeWords.Extensions;

namespace SwipeWords.FlashcardDrop.Services;

public class FlashcardDropService
{
    private readonly FlashcardGameDatabaseContext _context;
    
    public FlashcardDropService(FlashcardGameDatabaseContext context)
    {
        _context = context;
    }

    public List<string> SelectWord(List<string> mixedWords)
    {
        Random random = new Random();
        var randomIndex = random.Next(mixedWords.Count);
        var word = new List<string> { mixedWords[randomIndex] };
        return word;
    }

    public bool CheckIfCorrect(List<string> word, Guid flashcardId)
    {
        var correctWords = _context.GetCorrectWordsById(flashcardId);
        if (correctWords.Intersect(word).Any())
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public int CalculateScore(int elapsedTime, string difficulty)
    {
        if (difficulty == "Easy")
        {
            return elapsedTime * 100;
        }
        else if (difficulty == "Normal")
        {
            return elapsedTime * 110;
        }
        else
        {
            return elapsedTime * 125;
        }
    }
}