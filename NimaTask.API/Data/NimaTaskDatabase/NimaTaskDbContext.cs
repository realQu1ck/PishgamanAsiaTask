namespace NimaTask.API.Data.NimaTaskDatabase;

public class NimaTaskDbContext : DbContext
{
    public NimaTaskDbContext(DbContextOptions options) : base(options)
    {
        
    }

    public DbSet<NTRole> NTRoles { get; set; }
    public DbSet<NTUserRole> NTUserRoles { get; set; }
    public DbSet<NTUser> NTUsers { get; set; }

    public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
    {
        foreach (var entry in ChangeTracker.Entries<BaseModel>())
        {
            if (entry.State == EntityState.Modified)
            {
                entry.Entity.ModifiedDateTime = DateTime.Now;
            }
            if (entry.State == EntityState.Added)
            {
                entry.Entity.CreationDateTime = DateTime.Now;
            }
        }
        return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}
