namespace NutritionalRecipeBook.Application.Common.Models.User
{
    public class AddRecipeIngredientsToShopListRequest
    {
        public List<ShoppingListIngredientModel> Ingredients { get; set; }

        public string UserId { get; set; }
    }
}