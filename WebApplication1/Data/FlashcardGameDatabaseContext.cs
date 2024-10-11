using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;


namespace WebApplication1.Data;

public class FlashcardGameDatabaseContext : DbContext
{
    public FlashcardGameDatabaseContext(DbContextOptions<FlashcardGameDatabaseContext> options)
        : base(options)
    {
    }

    public DbSet<CorrectWord> CorrectWords { get; set; }
    public DbSet<IncorrectWord> IncorrectWords { get; set; }
    
    public DbSet<FlashcardEntity> Flashcards { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        var correctWords = DataSeederWords.GetCorrectWordsFromFile("./Data/correctWords.json");
        var incorrectWords = DataSeederWords.GetIncorrectWordsFromFile("./Data/incorrectWords.json");

        modelBuilder.Entity<CorrectWord>().HasData(correctWords.ToArray());
        modelBuilder.Entity<IncorrectWord>().HasData(incorrectWords.ToArray());
    }
            
    public List<string> GetSelectedCorrectWords(int amount , double exclusionPercentage = 0.3 )
    {
        int totalWords = CorrectWords.Count();
        int offset = (int)(totalWords * exclusionPercentage);

        var selectedWords = CorrectWords
            .OrderByDescending(cw => cw.Frequency)
            .Skip(offset)
            .OrderBy(cw => Guid.NewGuid())
            .Take(amount)
            .Select(cw => cw.Word)
            .ToList();

        return selectedWords;
    }

    public List<string> GetSelectedIncorrectWords(int amount, double exclusionPercentage = 0.3 )
    {
        int totalWords = IncorrectWords.Count();
        int offset = (int)(totalWords * exclusionPercentage);

        var selectedWords = IncorrectWords
            .OrderByDescending(iw => iw.Frequency)
            .Skip(offset)
            .OrderBy(iw => Guid.NewGuid())
            .Take(amount)
            .Select(iw => iw.Word)
            .ToList();

        return selectedWords;
    }
}

public class UsersDatabaseContext : DbContext
{
    public UsersDatabaseContext(DbContextOptions<UsersDatabaseContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Leaderboard> Leaderboards { get; set; }
    
}