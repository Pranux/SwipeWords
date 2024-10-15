using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace WebApplication1.Data;

public record CorrectWord
{
    [Key]
    public int WordId { get; set; }
    
    public string Word { get; set; }
    
    public int Frequency { get; set; }
}