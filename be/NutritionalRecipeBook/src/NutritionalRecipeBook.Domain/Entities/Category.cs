

namespace NutritionalRecipeBook.Domain.Entities
{
    public class Category : BaseEntity
    {
        public string Name { get; set; }

        public Category(string name)
        {
            Name = name;
        }
    }
}
