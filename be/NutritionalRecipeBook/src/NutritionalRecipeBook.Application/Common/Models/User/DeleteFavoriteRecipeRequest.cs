namespace NutritionalRecipeBook.Application.Common.Models.User
{
    public class DeleteFavoriteRecipeRequest
    {
        public string UserId { get; set; }

        public Guid RecipeId { get; set; }
    }
}
