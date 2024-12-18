using Microsoft.EntityFrameworkCore;

namespace SwipeWords.MemoryRecall.Data;

public interface IMemoryRecallDatabaseContext;

public class MemoryRecallDatabaseContext : DbContext, IMemoryRecallDatabaseContext
{
    public MemoryRecallDatabaseContext(DbContextOptions<MemoryRecallDatabaseContext> options)
        : base(options)
    {
    }

    public DbSet<SpeedReadingText> SpeedReadingTexts { get; set; }
    public DbSet<UserMemoryRecall> UserMemoryRecalls { get; set; }
}