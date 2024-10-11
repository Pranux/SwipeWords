namespace WebApplication1.Data;
using System.ComponentModel.DataAnnotations;
public class FlashcardEntity
{
       
    [Key]
    public Guid Id { get; set; }
    public string CorrectWords { get; set; }
    public string IncorrectWords { get; set; }
}