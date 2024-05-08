using NutritionalRecipeBook.Application.Common.Models.User;
using NutritionalRecipeBook.Domain.Results;

namespace NutritionalRecipeBook.Application.Contracts
{
    public interface IUserService
    {
        public Task<Result> AddFavoriteRecipe(AddFavoriteRecipeRequest request);

        public Task<Result> AddRecipeIngredientsToShopList(AddRecipeIngredientsToShopListRequest request);

        public Task<Result> UpdateShopList(UpdateShopListRequest request);

        public Task<Result> DeleteFavoriteRecipe(DeleteFavoriteRecipeRequest request);

        public Task<Result<GetUserResonse>> GetById(string id);
    }
}
