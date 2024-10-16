using System.ComponentModel.DataAnnotations;

namespace SwipeWords.Data;

public record IncorrectWord
{
    [Key] public int WordId { get; set; }

    public string Word { get; set; }

    public int Frequency { get; set; }
}