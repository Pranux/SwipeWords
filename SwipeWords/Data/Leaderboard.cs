using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SwipeWords.Data;

public record Leaderboard
{
    [Key] public int Id { get; set; }

    public int MaxScore { get; set; }
    public int RankPosition { get; set; }

    [ForeignKey(nameof(UserId))] public int UserId { get; set; }

    public User User { get; set; }
}