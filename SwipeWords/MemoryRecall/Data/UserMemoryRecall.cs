using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SwipeWords.Data;

public class UserMemoryRecall
{
    [Key]
    public int Id { get; set; }
    public Guid UserId { get; set; }
    public int SpeedReadingTextId { get; set; }
    public string RemovedWordPositions { get; set; }

    [ForeignKey(nameof(SpeedReadingTextId))]
    public SpeedReadingText SpeedReadingText { get; set; }
}