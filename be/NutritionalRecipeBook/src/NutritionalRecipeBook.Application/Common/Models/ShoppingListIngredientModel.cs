namespace NutritionalRecipeBook.Application.Common.Models
{
    public class ShoppingListIngredientModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public int Quantity { get; set; }

        public bool IsBougth { get; set; }
    }
}
