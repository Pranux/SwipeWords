using System.ComponentModel.DataAnnotations;

namespace SwipeWords.Data;

public class FlashcardEntity
{
    [Key] public Guid Id { get; set; }

    public string CorrectWords { get; set; }
    public string IncorrectWords { get; set; }
}