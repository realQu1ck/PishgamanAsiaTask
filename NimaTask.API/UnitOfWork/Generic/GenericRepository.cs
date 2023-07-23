namespace NimaTask.API.UnitOfWork.Generic;

public class GenericRepository<T> : IGenericRepository<T> where T : class
{
    protected NimaTaskDbContext context;
    internal DbSet<T> dbSet;
    public readonly Microsoft.Extensions.Logging.ILogger _logger;

    public GenericRepository(NimaTaskDbContext context, Microsoft.Extensions.Logging.ILogger logger)
    {
        this.context = context;
        dbSet = context.Set<T>();
        _logger = logger;
    }

    public virtual async Task<T> AddAsync(T model)
    {
        var res = await dbSet.AddAsync(model);
        return res.Entity;
    }

    public async Task<bool> AnyAsync(Expression<Func<T, bool>> predicate)
    {
        return await dbSet.AnyAsync(predicate);
    }

    public bool DeleteAsync(T model)
    {
        dbSet.Remove(model);
        return true;
    }

    public async Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate)
    {
        return await dbSet.FirstOrDefaultAsync(predicate);
    }

    public virtual async Task<ICollection<T>> GetAllAsync()
    {
        return await dbSet.ToListAsync();
    }

    public async Task<T> GetAsync(int id)
    {
        return await dbSet.FindAsync(id);
    }

    public virtual async Task<bool> UpdateAsync(T model)
    {
        var modelFind = await dbSet.FindAsync(model);
        context.Entry(modelFind).CurrentValues.SetValues(model);
        return true;
    }

    public async Task<IEnumerable<T>> WhereAsync(Expression<Func<T, bool>> predicate)
    {
        return await dbSet.Where(predicate).ToListAsync();
    }

    public virtual ICollection<T> DecryptedList(ICollection<T> models)
    {
        return models;
    }

    public virtual T Decrypt(T model)
    {
        return model;
    }

    public virtual T EncryptString(T model)
    {
        return model;
    }

    public async Task<T> Last()
    {
        var res = await dbSet.ToListAsync();
        return res.Last();
    }

    public DbSet<T> GetDbSet()
    {
        return dbSet;
    }
}
