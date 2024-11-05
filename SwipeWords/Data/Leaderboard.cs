using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SwipeWords.Data;

public class Leaderboard
{
    [Key] public string UserName { get; set; }

    public int MaxScore { get; set; }

    [ForeignKey(nameof(UserId))] public Guid UserId { get; set; }
    
}