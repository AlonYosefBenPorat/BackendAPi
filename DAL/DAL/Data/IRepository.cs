using System.Linq.Expressions;


namespace DAL.Data
{
    public interface IRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetByIdAsync(Guid id);
        Task AddAsync(T entity);
        Task DeleteAsync(Guid id);
        Task UpdateAsync(T entity);
        Task<IEnumerable<T>> FindAllAsync(Expression<Func<T, bool>> predicate);
        Task<T?> FindOneAsync(Expression<Func<T, bool>> predicate); 
        Task DeleteAsync(Expression<Func<T, bool>> predicate);
    }
}
