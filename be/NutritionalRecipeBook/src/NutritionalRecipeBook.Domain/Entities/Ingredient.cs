

namespace NutritionalRecipeBook.Domain.Entities
{
    public class Ingredient : BaseEntity
    {
        public string Name { get; set; }

        public Ingredient(string name)
        {
            Name = name;
        }
    }
}
