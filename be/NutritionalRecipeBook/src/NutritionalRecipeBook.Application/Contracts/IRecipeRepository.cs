using NutritionalRecipeBook.Application.Common.Collections;
using NutritionalRecipeBook.Application.Common.Models;
using NutritionalRecipeBook.Domain.Entities;

namespace NutritionalRecipeBook.Infrastructure.Contracts
{
    public interface IRecipeRepository : IGenericRepository<Recipe>
    {
        public Task<Recipe?> GetByIdWithRelationsAsync(Guid id);

        public Task<PagedList<Recipe>> GetAllSearchedAsync(SearchParams parameters);
    }
}
