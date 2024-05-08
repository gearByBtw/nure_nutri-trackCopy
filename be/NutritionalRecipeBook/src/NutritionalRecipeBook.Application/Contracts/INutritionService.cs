using NutritionalRecipeBook.Domain.Entities;

namespace NutritionalRecipeBook.Application.Contracts
{
    public interface INutritionService
    {
        public Task<double> GetRecipeCalories(int sizeInGrams, IEnumerable<Ingredient> ingredients);
    }
}
