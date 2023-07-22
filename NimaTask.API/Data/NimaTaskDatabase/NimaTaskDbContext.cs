namespace NimaTask.API.Data.NimaTaskDatabase;

public class NimaTaskDbContext : DbContext
{
    public NimaTaskDbContext(DbContextOptions options) : base(options)
    {

    }

    public DbSet<NTRole> NTRoles { get; set; }
    public DbSet<NTUserRole> NTUserRoles { get; set; }
    public DbSet<NTUser> NTUsers { get; set; }
    public DbSet<NTUserToken> NTUserTokens { get; set; }
    public DbSet<NTUserTokenLog> NTUserTokenLogs { get; set; }

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
        modelBuilder.Entity<NTUserRole>().Property(x => x.RoleId)
            .IsRequired();
        modelBuilder.Entity<NTUserRole>().Property(x => x.UserId)
            .IsRequired();

        modelBuilder.Entity<NTRole>().Property(x => x.Role).IsRequired();

        modelBuilder.Entity<NTUser>().Property(x => x.Name)
            .HasMaxLength(50)
            .IsRequired();

        modelBuilder.Entity<NTUser>().Property(x => x.Family)
            .HasMaxLength(100)
            .IsRequired();

        modelBuilder.Entity<NTUser>().Property(x => x.Parent)
            .HasMaxLength(150)
            .IsRequired();

        modelBuilder.Entity<NTUser>().Property(x => x.Meli)
            .HasMaxLength(11)
            .IsRequired();

        modelBuilder.Entity<NTUser>().Property(x => x.Picture).IsRequired(false);
    }
}
