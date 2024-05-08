namespace NutritionalRecipeBook.Application.Common.Models
{
    public class PaginationParams
    {
        public int PageNumber { get; set; } = 1;

        public int PageSize { get; set; } = 10;
    }
}
