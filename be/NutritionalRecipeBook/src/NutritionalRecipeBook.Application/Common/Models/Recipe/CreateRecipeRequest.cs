using NutritionalRecipeBook.Domain.Entities;

namespace NutritionalRecipeBook.Application.Common.Models.Recipe
{
    public class CreateRecipeRequest
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public string CookingProcess { get; set; }

        public TimeSpan CookingTime { get; set; }

        public int ServingSizeInGrams { get; set; }

        public Category Category { get; set; }

        public List<Ingredient> NewIngredients { get; set; }

        public List<Ingredient> ExistingIngredients { get; set; }

        public string UserId { get; set; }
    }
}
