using System.ComponentModel.DataAnnotations;

namespace SwipeWords.Data;

public record User
{
    [Key] public int UserId { get; set; }

    public string Name { get; set; }

    public string Email { get; set; }

    public DateTime CreationDate { get; set; }
}