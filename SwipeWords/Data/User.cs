using System.ComponentModel.DataAnnotations;

namespace SwipeWords.Data;

public record User
{
    [Key] public Guid UserId { get; set; }
    public string Name { get; set; }
    public string PasswordHash { get; set; }
    public string PasswordSalt { get; set; }
    
}