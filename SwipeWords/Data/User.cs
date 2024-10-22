using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using System.Text;

namespace SwipeWords.Data;

public class User
{
    [Key] public Guid UserId { get; set; }

    public string Name { get; set; }

    public string PasswordHash { get; set; }

    public string PasswordSalt { get; set; }
    
}