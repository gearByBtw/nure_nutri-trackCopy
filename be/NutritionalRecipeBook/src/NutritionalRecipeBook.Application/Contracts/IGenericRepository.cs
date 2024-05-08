using NutritionalRecipeBook.Domain.Entities;
using System.Linq.Expressions;

namespace NutritionalRecipeBook.Infrastructure.Contracts
{
    public interface IGenericRepository<T> where T : BaseEntity
    {
        public Task<T?> GetByIdAsync(Guid id);

        public Task<List<T>> GetManyByPredicateAsync(Expression<Func<T, bool>> predicate);

        public Task<T?> GetOneByPredicateAsync(Expression<Func<T, bool>> predicate);

        public Task UpdateAsync(T item);

        public Task UpdateManyAsync(List<T> item);

        public Task<List<T>> GetAllAsync();

        public Task CreateAsync(T item);

        public Task CreateManyAsync(List<T> items);

        public Task RemoveByIdAsync(Guid id);

        public Task RemoveAsync(T item);

        public Task RemoveManyAsync(List<T> items);

        public Task RemoveByPredicateAsync(Expression<Func<T, bool>> predicate);
    }
}
