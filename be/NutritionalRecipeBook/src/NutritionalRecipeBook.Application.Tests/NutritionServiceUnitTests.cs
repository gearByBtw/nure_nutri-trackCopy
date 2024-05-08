using FluentAssertions;
using NutritionalRecipeBook.Application.Services;
using NutritionalRecipeBook.Domain.Entities;
using System.Text;

namespace NutritionalRecipeBook.Application.UnitTests
{
    public class NutritionServiceUnitTests
    {
        [Theory]
        [InlineData(100)]
        public async Task Should_ReturnRecipeCalories(int sizeInGrams)
        {
            var ingredients = TestData.GetIngredients();
            var nutritionData = TestData.GetNutritionData();
            var queryString = PrepareQueryString(ingredients);

            var nutritionixClient = TestData.MockINutritionixClient(nutritionData, queryString).Object;

            var nutritionService = new NutritionService(nutritionixClient);

            var result = await nutritionService.GetRecipeCalories(sizeInGrams, ingredients);

            result.Should().Be(120);
        }

        [Theory]
        [InlineData(100)]
        public async Task Should_ReturnDefaultValue_EmptyQueryString(int sizeInGrams)
        {
            var ingredients = TestData.GetIngredients();
            var nutritionData = TestData.GetNutritionData();
            var queryString = "";
            
            var nutritionixClient = TestData.MockINutritionixClient(nutritionData, queryString).Object;

            var nutritionService = new NutritionService(nutritionixClient);

            var result = await nutritionService.GetRecipeCalories(sizeInGrams, ingredients);

            result.Should().Be(0.0);
        }

        [Theory]
        [InlineData(100)]
        public async Task Should_ReturnDefaultValue_NutritionDataIsNull(int sizeInGrams)
        {
            var ingredients = TestData.GetIngredients();
            var nutritionData = TestData.GetNutritionData();
            var queryString = "Inappropriate data";

            var nutritionixClient = TestData.MockINutritionixClient(nutritionData, queryString).Object;

            var nutritionService = new NutritionService(nutritionixClient);

            var result = await nutritionService.GetRecipeCalories(sizeInGrams, ingredients);

            result.Should().Be(0.0);
        }

        private string PrepareQueryString(IEnumerable<Ingredient> ingredients)
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
    }
}
