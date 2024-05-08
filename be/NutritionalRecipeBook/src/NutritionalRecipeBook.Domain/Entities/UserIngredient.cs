

namespace NutritionalRecipeBook.Domain.Entities
{
    public class UserIngredient : BaseEntity
    {
        public string UserId { get; set; }

        public User User { get; set; }

        public Guid IngredientId { get; set; }

        public Ingredient Ingredient { get; set; }

        public int Quantity { get; set; }

        public bool IsBougth { get; set; }
    }
}
