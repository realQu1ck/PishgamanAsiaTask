namespace NimaTask.API.Data.NimaTaskDatabase;

public class NimaTaskDbContextSeed
{
    private readonly IServiceProvider serviceProvider;

    public NimaTaskDbContextSeed(IServiceProvider serviceProvider)
    {
        this.serviceProvider = serviceProvider;
    }

    public void Seed()
    {
        SeedRoles().Wait();
    }

    public async Task SeedRoles()
    {
        using (var scope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
        {
            var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
            if (!await unitOfWork.RoleRepository.AnyAsync(x => x.Role == "Read"))
            {
                await unitOfWork.RoleRepository.AddAsync(new NTRole
                {
                    Role = "Read"
                });
                await unitOfWork.RoleRepository.AddAsync(new NTRole
                {
                    Role = "Write"
                });
                await unitOfWork.SaveChangesAsync();
            }
        }
    }
}
