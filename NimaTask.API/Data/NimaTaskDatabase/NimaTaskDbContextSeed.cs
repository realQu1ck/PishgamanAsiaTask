using NimaTask.API.Helpers;

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
        EnsureCreated().Wait();
        SeedRoles().Wait();
        SeedUsers().Wait();
        SeedUsersRole().Wait();
    }

    public async Task EnsureCreated()
    {
        using (var scope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
        {
            var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

            await unitOfWork.EnsureCreated();
        }
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

    public async Task SeedUsers()
    {
        using (var scope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
        {
            var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
            var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();

            if (!await unitOfWork.UserRepository.AnyAsync(x => x.PhoneNumber == "09352877011"))
            {
                var user = configuration.GetSection("Users").Get<List<NTUser>>();

                foreach (var item in user)
                {
                    item.Password = SecurePasswordHasher.Hash(item.Password);
                    await unitOfWork.UserRepository.AddAsync(item);
                }

                await unitOfWork.SaveChangesAsync();
            }
        }
    }

    public async Task SeedUsersRole()
    {
        using (var scope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
        {
            var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
            var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();

            if (!await unitOfWork.UserRoleRepository.AnyAsync(x => x.UserId == 1))
            {
                var user = configuration.GetSection("UserRoles").Get<List<NTUserRole>>();

                foreach (var item in user)
                {
                    await unitOfWork.UserRoleRepository.AddAsync(item);
                }

                await unitOfWork.SaveChangesAsync();
            }
        }
    }
}
