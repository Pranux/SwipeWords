namespace WebApplication1.Models;
    
// solution to not being able to have a single list of all created flashcards in program.cs
// can later be moved to a different class or, if smarter solution appears, deleted
public static class FlashcardStorage
{
    public static List<Flashcard> Flashcards { get; } = new List<Flashcard>();
}   