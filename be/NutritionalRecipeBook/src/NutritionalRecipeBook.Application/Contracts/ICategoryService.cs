using NutritionalRecipeBook.Domain.Entities;
using NutritionalRecipeBook.Domain.Results;

namespace NutritionalRecipeBook.Application.Contracts
{
    public interface ICategoryService
    {
        public Task<Result<List<Category>>> GetAllAsync();
    }
}
