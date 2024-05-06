using System.Linq.Expressions;
using WebApi.Models;

namespace WebApi.Data.Interfaces;

public interface IRepository<T> where T : BaseModel
{
    Task<IEnumerable<T>> GetListAsync(
        Expression<Func<T, bool>> filter = null,
        Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null);
        
    Task<List<T>> ListAsync(Expression<Func<T, bool>> expression);
        
    Task<T> GetByIdAsync(Guid id);

    Task<T> GetByIdAsync(Guid id, string includeProperties);

    Task<T> GetByPredicateAsync(Expression<Func<T, bool>> predicate);

    Task<List<T>> GetListByPredicateAsync(Expression<Func<T, bool>> predicate);

    Task AddAsync(T obj);

    Task UpdateAsync(Guid id, T updateObj);

    Task DeleteAsync(T entity);
}