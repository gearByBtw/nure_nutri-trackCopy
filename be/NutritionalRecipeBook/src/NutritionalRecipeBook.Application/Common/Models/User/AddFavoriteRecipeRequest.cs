namespace NutritionalRecipeBook.Application.Common.Models.User
{
    public class AddFavoriteRecipeRequest
    {
        public string UserId { get; set; }

        public Guid RecipeId { get; set; }
    }
}
