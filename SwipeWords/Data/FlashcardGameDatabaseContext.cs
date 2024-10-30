using Microsoft.EntityFrameworkCore;


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