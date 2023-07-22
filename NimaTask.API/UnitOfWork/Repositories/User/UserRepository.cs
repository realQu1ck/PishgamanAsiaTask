namespace NimaTask.API.UnitOfWork.Repositories.User;

public class UserRepository : GenericRepository<NTUser>, IUserRepository
{
    public UserRepository(NimaTaskDbContext context, Microsoft.Extensions.Logging.ILogger logger) : base(context, logger)
    {
    }
    public override async Task<bool> UpdateAsync(NTUser model)
    {
        try
        {
            var find = await dbSet.Where(x => x.Id == model.Id).FirstOrDefaultAsync();
            if (find == null)
                return false;

            find.Family = model.Family;
            find.PhoneNumber = model.PhoneNumber;

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "UPDATE Error", typeof(UserRepository));
            return false;
        }
    }
}
