namespace NimaTask.API.UnitOfWork.Repositories.UserTokenLog;

public class UserTokenLogRepository : GenericRepository<NTUserTokenLog>, IUserTokenLogRepository
{
    public UserTokenLogRepository(NimaTaskDbContext context, Microsoft.Extensions.Logging.ILogger logger) : base(context, logger)
    {
    }
}
