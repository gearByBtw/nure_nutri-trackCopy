using Microsoft.AspNetCore.Identity;

namespace NutritionalRecipeBook.Domain.Entities
{
    public class User : IdentityUser
    {
        public List<UserRecipe> FavoriteRecipes { get; set; }

        public List<UserIngredient> ShoppingList { get; set; }
    }
}
