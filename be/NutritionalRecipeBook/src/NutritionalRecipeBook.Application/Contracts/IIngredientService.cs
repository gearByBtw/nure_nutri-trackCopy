using NutritionalRecipeBook.Domain.Entities;
using NutritionalRecipeBook.Domain.Results;

namespace NutritionalRecipeBook.Application.Contracts
{
    public interface IIngredientService
    {
        public Task<Result<List<Ingredient>>> GetAllAsync();
    }
}
