using Microsoft.EntityFrameworkCore;
using SwipeWords.Models;


namespace SwipeWords.Data;

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