namespace NimaTask.API.Data.NLogDatabase;

public class NlogDbContext : DbContext
{
    public NlogDbContext(DbContextOptions<NlogDbContext> options) : base(options)
    {
        
    }

    public DbSet<Logs> Logs { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}   
