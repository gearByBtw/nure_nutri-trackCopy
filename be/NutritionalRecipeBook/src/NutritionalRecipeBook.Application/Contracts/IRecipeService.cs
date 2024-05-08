using NutritionalRecipeBook.Application.Common.Collections;
using NutritionalRecipeBook.Application.Common.Models;
using NutritionalRecipeBook.Application.Common.Models.Recipe;
using NutritionalRecipeBook.Domain.Entities;
using NutritionalRecipeBook.Domain.Results;

namespace NutritionalRecipeBook.Application.Contracts
{
    public interface IRecipeService
    {
        public Task<Result<PagedList<GetRecipeResponse>>> GetAllAsync(SearchParams parameters);

        public Task<Result<Recipe>> GetByIdAsync(Guid id);

        public Task<Result<GetRecipeResponse>> GetByIdWithRelationsAsync(Guid id);

        public Task<Result> CreateAsync(CreateRecipeRequest request);

        public Task<Result> UpdateAsync(UpdateRecipeRequest request);

        public Task<Result> DeleteByIdAsync(Guid id, string userId);
    }
}
