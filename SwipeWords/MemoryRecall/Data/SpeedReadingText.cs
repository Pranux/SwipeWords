using System.ComponentModel.DataAnnotations;

namespace SwipeWords.Data;

public class SpeedReadingText
{
    [Key]
    public int Id { get; set; }
    public string Content { get; set; }
}