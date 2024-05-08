using NutritionalRecipeBook.Application.Contracts;
using NutritionalRecipeBook.Domain.Entities;
using Nutritionix;
using System.Text;

namespace NutritionalRecipeBook.Application.Services
{
    public class NutritionService : INutritionService
    {
        private readonly INutritionixClient _nutritionixClient;

        public NutritionService(INutritionixClient nutritionixClient)
        {
            _nutritionixClient = nutritionixClient;
        }

        public async Task<double> GetRecipeCalories(int sizeInGrams, IEnumerable<Ingredient> ingredients)
        {
            var query = PrepareQueryStringForIngredients(ingredients);

            if (string.IsNullOrEmpty(query))
            {
                return default;
            }

            var data = await _nutritionixClient.GetNutritionData(query);

            if (data is null || data.Foods.Count() == 0)
            {
                return default;
            }

            return CalculateCalories(data, sizeInGrams);
        }

        private string PrepareQueryStringForIngredients(IEnumerable<Ingredient> ingredients)
        {
            var names = new StringBuilder();

            if (ingredients is null || ingredients.Count() == 0)
            {
                return names.ToString();
            }

            foreach (var item in ingredients)
            {
                names.Append($"{item.Name}, ");
            }

            names.Remove(names.Length - 2, 2);

            return names.ToString();
        }

        private double CalculateCalories(NutritionData data, int sizeInGrams)
        {
            return (data.Foods.Sum(f => f.Calories) / 100) * sizeInGrams;
        }
    }
}
