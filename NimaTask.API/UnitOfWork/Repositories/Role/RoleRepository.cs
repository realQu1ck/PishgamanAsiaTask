namespace NimaTask.API.UnitOfWork.Repositories.Role;

public class RoleRepository : GenericRepository<NTRole>, IRoleRepository
{
    public RoleRepository(NimaTaskDbContext context, Microsoft.Extensions.Logging.ILogger logger) : base(context, logger)
    {
    }
}
