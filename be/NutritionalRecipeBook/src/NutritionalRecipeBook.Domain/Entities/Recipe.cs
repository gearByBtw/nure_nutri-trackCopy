using NutritionalRecipeBook.Domain.ValueObjects;

namespace NutritionalRecipeBook.Domain.Entities
{
    public class Recipe : BaseEntity
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public string CookingProcess { get; set; }

        public TimeSpan CookingTime { get; set; }

        public int ServingSizeInGrams { get; set; }

        public double Calories { get; set; }

        public Category Category { get; set; }

        public string UserId { get; set; }

        public List<Review> Reviews { get; set; }

        public List<RecipeIngredient> Ingredients { get; set; }

        public Recipe() { }

        public Recipe(RecipeSpecification specification, Category category, string userId)
        {
            Name = specification.Name;
            Description = specification.Description;
            CookingProcess = specification.CookingProcess;
            CookingTime = specification.CookingTime;
            ServingSizeInGrams = specification.ServingSizeInGrams;
            Calories = specification.Calories;
            Category = category;
            UserId = userId;
        }
    }
}
