namespace NutritionalRecipeBook.Domain.Entities
{
    public class Review : BaseEntity
    {

        public double Rating { get; set; }

        public string? Comment { get; set; }

        public string UserId { get; set; }

        public string AuthorName { get; set; }

        public Guid RecipeId { get; set; }

        public Review(double rating, string comment, string userId, Guid recipeId, string authorName)
        {
            Rating = rating;
            Comment = comment;
            UserId = userId;
            RecipeId = recipeId;
            AuthorName = authorName;
        }
    }
}
