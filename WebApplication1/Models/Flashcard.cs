using System;
using System.Collections.Generic;
using System.Linq;


namespace WebApplication1.Models;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class Flashcard
{
    public Guid Id { get; private set; }        
    public List<string> Words { get; private set; } // buffer for selected amount of correct words
    public List<string> MixedWords { get; private set; } // for storing both correct and incorrect words

    private List<string> CorrectWords { get; set; }  // for storing correct words read from file
    private List<string> IncorrectWords { get; set; }  // for storing incorrect words read from file
    
    // Constructor
    public Flashcard(DatabaseContext databaseContext, int wordCount = 5, double exclusionPercentage = 0.30)
    {
        Id = Guid.NewGuid(); 
        
        CorrectWords = databaseContext.GetSelectedCorrectWords(wordCount, exclusionPercentage);
        IncorrectWords = databaseContext.GetSelectedIncorrectWords(wordCount, exclusionPercentage);

        MixedWords = MixInIncorrectWords(CorrectWords); 
    }
    
    
    private List<string> MixInIncorrectWords(List<string> correctWords)
    {
        var mixedWords = new List<string>(correctWords); // start with correct words
        var random = new Random();
        int numIncorrect = random.Next(0, 3); // replace 0-2 correct words with incorrect ones

        for (int i = 0; i < numIncorrect; i++)
        {
            mixedWords[i] = IncorrectWords[random.Next(IncorrectWords.Count)];
        }

        return mixedWords;
    }

    // Method to check if the user's answer is correct
    public bool IsAnswerCorrect(bool userAnswer)
    {
        // compare the buffer of correct words and mixed words to determine if any are incorrect
        bool allWordsCorrect = Words.SequenceEqual(MixedWords);
        return userAnswer == allWordsCorrect;
    }

    // returning flashcard data for communication with the frontend
    public Dictionary<string, object> GetFlashcardData()
    {
        return new Dictionary<string, object>
        {
            { "id", Id },
            { "words", MixedWords }
        };
    }
}