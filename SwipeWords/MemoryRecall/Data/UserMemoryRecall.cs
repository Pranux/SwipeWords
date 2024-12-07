using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SwipeWords.MemoryRecall.Data;

public class UserMemoryRecall
{
    [Key]
    public Guid MemoryRecallId { get; set; }
    public Guid SpeedReadingTextId { get; set; }
    public List<int> RemovedWordPositions { get; set; }

    [ForeignKey(nameof(SpeedReadingTextId))]
    public SpeedReadingText SpeedReadingText { get; set; }
}