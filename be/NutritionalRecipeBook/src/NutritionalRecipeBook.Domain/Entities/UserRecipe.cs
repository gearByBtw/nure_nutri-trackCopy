namespace NutritionalRecipeBook.Domain.Entities
{
    public class UserRecipe : BaseEntity
    {
        public Guid RecipeId { get; set; }

        public Recipe Recipe { get; set; }

        public string UserId { get; set; }

        public User User { get; set; }
    }
}
