namespace NimaTask.API.UnitOfWork.Generic;

public interface IGenericRepository<T> where T : class
{
    public Task<ICollection<T>> GetAllAsync();
    public DbSet<T> GetDbSet();
    public Task<T> GetAsync(Guid id);
    public Task<bool> AnyAsync(Expression<Func<T, bool>> predicate);
    public Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate);
    Task<IEnumerable<T>> WhereAsync(Expression<Func<T, bool>> predicate);
    public Task<T> AddAsync(T model);
    public Task<bool> UpdateAsync(T model);
    public bool DeleteAsync(T model);
    public Task<T> Last();
}
