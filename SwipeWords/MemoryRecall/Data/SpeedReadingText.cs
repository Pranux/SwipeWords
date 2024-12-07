using System.ComponentModel.DataAnnotations;

namespace SwipeWords.MemoryRecall.Data
{
    public class SpeedReadingText
    {
        [Key]
        public Guid SpeedReadingTextId { get; set; }
        public string Content { get; set; }
    }
}