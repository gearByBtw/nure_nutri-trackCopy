using NutritionalRecipeBook.Application.Contracts;
using NutritionalRecipeBook.Domain.Entities;
using NutritionalRecipeBook.Domain.Results;
using NutritionalRecipeBook.Infrastructure.Contracts;

namespace NutritionalRecipeBook.Application.Services
{
    public class IngredientService : IIngredientService
    {
        private readonly IGenericRepository<Ingredient> _ingredientRepository;

        public IngredientService(IGenericRepository<Ingredient> ingredientRepository)
        {
            _ingredientRepository = ingredientRepository;
        }

        public async Task<Result<List<Ingredient>>> GetAllAsync()
        {
            var ingredients = await _ingredientRepository.GetAllAsync();
            return Result<List<Ingredient>>.Success(ingredients);
        }
    }
}
