using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NutritionalRecipeBook.Application.Common.Collections;
using NutritionalRecipeBook.Application.Common.Models;
using NutritionalRecipeBook.Domain.Entities;
using NutritionalRecipeBook.Infrastructure.Contracts;

namespace NutritionalRecipeBook.Infrastructure.Repositories
{
    public class RecipeRepository : GenericRepository<Recipe>, IRecipeRepository
    {
        public RecipeRepository(DbContext context) : base(context) { }

        public Task<Recipe?> GetByIdWithRelationsAsync(Guid id)
        {
            return _dbSet.Include(r => r.Category)
                .Include(r => r.Ingredients)
                .ThenInclude(ri => ri.Ingredient)
                .Include(r => r.Reviews)
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<PagedList<Recipe>> GetAllSearchedAsync(SearchParams parameters)
        {
            var query = _dbSet.AsQueryable();

            if (!string.IsNullOrEmpty(parameters.SearchTerm))
            {
                query = query.Where(r => r.Name.ToLower().Contains(parameters.SearchTerm.ToLower()) || r.Description.ToLower().Contains(parameters.SearchTerm.ToLower()));
            }

            var filters = parameters.Filters?.Split(',');

            if (!filters.IsNullOrEmpty())
            {
                query = query.Where(r => filters.Contains(r.Category.Name) || r.Ingredients.Any(ri => filters.Contains(ri.Ingredient.Name)));
            }

            if (parameters.MinCalories.HasValue)
            {
                query = query.Where(r => r.Calories >= parameters.MinCalories);
            }

            if (parameters.MaxCalories.HasValue)
            {
                query = query.Where(r => r.Calories <= parameters.MaxCalories);
            }

            query = query.OrderBy(r => r.Id).Include(r => r.Ingredients).ThenInclude(ri => ri.Ingredient).Include(r => r.Category).Include(r => r.Reviews);

            var count = await query.CountAsync();
            var items = await query.Skip((parameters.PageNumber - 1) * parameters.PageSize).Take(parameters.PageSize).ToListAsync();

            return new PagedList<Recipe>(items, count, parameters.PageNumber, parameters.PageSize);
        }
    }
}
