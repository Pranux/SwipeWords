using Microsoft.EntityFrameworkCore;

namespace SwipeWords.MemoryRecall.Data;

public class MemoryRecallDatabaseContext : DbContext
{
    public MemoryRecallDatabaseContext(DbContextOptions<MemoryRecallDatabaseContext> options)
        : base(options)
    {
    }

    public DbSet<SpeedReadingText> SpeedReadingTexts { get; set; }
    public DbSet<UserMemoryRecall> UserMemoryRecalls { get; set; }
}