namespace NutritionalRecipeBook.Application.Common.Models
{
    public class AddReviewRequest
    {
        public double Rating { get; set; }

        public string? Comment { get; set; }

        public string UserId { get; set; }

        public Guid RecipeId { get; set; }
    }
}
