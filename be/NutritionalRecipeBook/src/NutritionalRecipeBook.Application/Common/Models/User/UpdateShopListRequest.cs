

namespace NutritionalRecipeBook.Application.Common.Models.User
{
    public class UpdateShopListRequest
    {
        public List<ShoppingListIngredientModel> ExistingIngredients { get; set; }

        public List<ShoppingListIngredientModel> NewIngredients { get; set; }

        public string UserId { get; set; }
    }
}
