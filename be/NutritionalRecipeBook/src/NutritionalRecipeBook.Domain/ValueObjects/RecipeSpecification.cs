

namespace NutritionalRecipeBook.Domain.ValueObjects
{
    public class RecipeSpecification
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public string CookingProcess { get; set; }

        public TimeSpan CookingTime { get; set; }

        public int ServingSizeInGrams { get; set; }

        public double Calories { get; set; }

        public RecipeSpecification(string name, string description, string cookingProcess, TimeSpan time, int size, double calories)
        {
            Name = name;
            Description = description;
            CookingProcess = cookingProcess;
            CookingTime = time;
            ServingSizeInGrams = size;
            Calories = calories;
        }
    }
}
