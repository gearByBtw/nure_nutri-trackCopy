namespace Models
{
    public class SearchParams : PaginationParams
    {
        public string? SearchTerm { get; set; }

        public string? Filters { get; set; }

        public int? MinCalories { get; set; }

        public int? MaxCalories { get; set; }
    }
}
