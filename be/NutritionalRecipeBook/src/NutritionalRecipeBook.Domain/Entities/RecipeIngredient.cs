namespace NutritionalRecipeBook.Domain.Entities
{
    public class RecipeIngredient : BaseEntity
    {
        public Guid RecipeId { get; set; }

        public Recipe Recipe { get; set; }

        public Guid IngredientId { get; set; }

        public Ingredient Ingredient { get; set; }
    }
}
