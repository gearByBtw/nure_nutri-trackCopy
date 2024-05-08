using Microsoft.EntityFrameworkCore;
using NutritionalRecipeBook.Domain.Entities;
using NutritionalRecipeBook.Infrastructure.Contracts;
using System.Linq.Expressions;

namespace NutritionalRecipeBook.Infrastructure.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        protected readonly DbContext _context;

        protected readonly DbSet<T> _dbSet;

        public GenericRepository(DbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public Task<T?> GetByIdAsync(Guid id)
        {
            return _dbSet.FirstOrDefaultAsync(u => u.Id.Equals(id));
        }

        public Task<List<T>> GetAllAsync() => _dbSet.ToListAsync();

        public Task<List<T>> GetManyByPredicateAsync(Expression<Func<T, bool>> predicate)
        {
            return _dbSet
                .Where(predicate)
                .ToListAsync();
        }

        public Task<T?> GetOneByPredicateAsync(Expression<Func<T, bool>> predicate)
        {
            return _dbSet.FirstOrDefaultAsync(predicate);
        }

        public async Task CreateAsync(T item)
        {
            await _dbSet.AddAsync(item);
            await _context.SaveChangesAsync();
        }

        public async Task CreateManyAsync(List<T> items)
        {
            await _dbSet.AddRangeAsync(items);
            await _context.SaveChangesAsync();
        }

        public Task UpdateAsync(T item)
        {
            _dbSet.Update(item);

            return _context.SaveChangesAsync();
        }

        public Task UpdateManyAsync(List<T> items)
        {
            _dbSet.UpdateRange(items);

            return _context.SaveChangesAsync();
        }

        public async Task RemoveByIdAsync(Guid id)
        {
            var itemToRemove = await GetByIdAsync(id);

            if (itemToRemove != null)
            {
                _context.Remove(itemToRemove);
            }

            await _context.SaveChangesAsync();
        }

        public async Task RemoveAsync(T item)
        {
            _context.Remove(item);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveManyAsync(List<T> items)
        {
            _context.RemoveRange(items);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveByPredicateAsync(Expression<Func<T, bool>> predicate)
        {
            var itemsToRemove = await GetManyByPredicateAsync(predicate);

            foreach (var item in itemsToRemove)
            {
                _context.Remove(item);
            }

            await _context.SaveChangesAsync();
        }
    }
}
