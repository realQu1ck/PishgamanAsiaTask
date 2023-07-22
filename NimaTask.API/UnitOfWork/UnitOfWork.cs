using NimaTask.API.UnitOfWork.Repositories.Role;
using NimaTask.API.UnitOfWork.Repositories.User;
using NimaTask.API.UnitOfWork.Repositories.UserRole;

namespace NimaTask.API.UnitOfWork;

public class UnitOfWork : IUnitOfWork
{
    private readonly NimaTaskDbContext _context;
    private readonly Microsoft.Extensions.Logging.ILogger _logger;
    public IRoleRepository RoleRepository { get; private set; }
    public IUserRoleRepository UserRoleRepository { get; private set; }
    public IUserRepository UserRepository { get; private set; }

    public UnitOfWork(NimaTaskDbContext context, ILoggerFactory logger)
    {
        _context = context;
        _logger = logger.CreateLogger<UnitOfWork>();

        RoleRepository = new RoleRepository(_context, _logger);
        UserRoleRepository = new UserRoleRepository(_context, _logger);
        UserRepository = new UserRepository(_context, _logger);
    }


    public void Dispose()
    {
        _context.Dispose();
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }

    public async Task EnsureCreated()
    {
        await _context.Database.EnsureCreatedAsync();
    }
}
