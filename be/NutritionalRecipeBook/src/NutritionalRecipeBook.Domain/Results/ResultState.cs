namespace NutritionalRecipeBook.Domain.Results
{
    public enum ResultState
    {
        Ok = 0,
        BadRequest = 1,
        UnprocessableEntity = 2,
        NotFound = 3,
        Unauthorized = 4
    }
}
