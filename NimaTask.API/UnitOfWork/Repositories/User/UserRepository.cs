namespace NimaTask.API.UnitOfWork.Repositories.User;

public class UserRepository : GenericRepository<NTUser>, IUserRepository
{
    public UserRepository(NimaTaskDbContext context, Microsoft.Extensions.Logging.ILogger logger) : base(context, logger)
    {
    }
}
