namespace NimaTask.API.UnitOfWork.Repositories.UserRole;

public class UserRoleRepository : GenericRepository<NTUserRole>, IUserRoleRepository
{
    public UserRoleRepository(NimaTaskDbContext context, Microsoft.Extensions.Logging.ILogger logger) : base(context, logger)
    {
    }
}
