using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Data;

public class IncorrectWord
{
    [Key]
    public int WordId { get; set; }
    
    public string Word { get; set; }
    
    public int Frequency { get; set; }
}