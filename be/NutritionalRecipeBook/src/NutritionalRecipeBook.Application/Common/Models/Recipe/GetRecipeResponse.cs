

using NutritionalRecipeBook.Domain.Entities;

namespace NutritionalRecipeBook.Application.Common.Models.Recipe
{
    public class GetRecipeResponse
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string CookingProcess { get; set; }

        public TimeSpan CookingTime { get; set; }

        public int ServingSizeInGrams { get; set; }

        public Category Category { get; set; }

        public double Calories { get; set; }

        public List<Review> Reviews { get; set; }

        public IEnumerable<Ingredient> Ingredients { get; set; }

        public string UserId { get; set; }
    }
}
