using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Data;

public record User
{
    [Key]
    public int UserId { get; set; }
    
    public string Name { get; set; }
    
    public string Email { get; set; }
    
    public DateTime CreationDate { get; set; }
    
}