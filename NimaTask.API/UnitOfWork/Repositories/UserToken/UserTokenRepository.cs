namespace NimaTask.API.UnitOfWork.Repositories.UserToken;

public class UserTokenRepository : GenericRepository<NTUserToken>, IUserTokenRepository
{
    public UserTokenRepository(NimaTaskDbContext context, Microsoft.Extensions.Logging.ILogger logger) : base(context, logger)
    {
    }

    public override async Task<bool> UpdateAsync(NTUserToken model)
    {
        try
        {
            var find = await dbSet.Where(x => x.Id == model.Id).FirstOrDefaultAsync();
            if (find == null)
                return false;

            find.Token = model.Token;

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "UPDATE Error", typeof(UserTokenRepository));
            return false;
        }
    }
}
