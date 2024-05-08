using AutoMapper;
using NutritionalRecipeBook.Application.Common.Models;
using NutritionalRecipeBook.Application.Common.Models.User;
using NutritionalRecipeBook.Application.Contracts;
using NutritionalRecipeBook.Domain.Entities;
using NutritionalRecipeBook.Domain.Results;
using NutritionalRecipeBook.Infrastructure.Contracts;

namespace NutritionalRecipeBook.Application.Services
{
    public class UserService : IUserService
    {

        private readonly IGenericRepository<UserRecipe> _userRecipeRepository;

        private readonly IGenericRepository<UserIngredient> _userIngredientRepository;

        private readonly IRecipeRepository _recipeRepository;

        private readonly IMapper _mapper;

        private readonly IIdentityService _identityService;

        public UserService(IGenericRepository<UserRecipe> userRecipeRepository,
            IGenericRepository<UserIngredient> userIngredientRepository,
            IRecipeRepository recipeRepository,
            IMapper mapper,
            IIdentityService identityService)
        {
            _userRecipeRepository = userRecipeRepository;
            _userIngredientRepository = userIngredientRepository;
            _recipeRepository = recipeRepository;
            _mapper = mapper;
            _identityService = identityService;
        }

        public async Task<Result<GetUserResonse>> GetById(string id)
        {
            var user = await _identityService.GetUserByIdWithRelationsAsync(id);

            if (user is null)
            {
                return Result<GetUserResonse>.Failure(new Error("404", "Such user doesn't exist"));
            }

            var response = _mapper.Map<GetUserResonse>(user);

            return Result<GetUserResonse>.Success(response);
        }

        public async Task<Result> AddFavoriteRecipe(AddFavoriteRecipeRequest request)
        {
            var user = await _identityService.FindUserByIdAsync(request.UserId);

            if (user is null)
            {
                return Result.Failure(new Error("404", "Such user doesn't exist"));
            }

            var recipe = await _recipeRepository.GetByIdAsync(request.RecipeId);

            if (recipe is null)
            {
                return Result.Failure(new Error("404", "Such recipe doesn't exist"));
            }

            var recipeFromFavoriteList = await _userRecipeRepository.GetOneByPredicateAsync(ur => ur.UserId == user.Id && ur.RecipeId == recipe.Id);

            if (recipeFromFavoriteList is null)
            {
                var recipeToAdd = new UserRecipe { User = user, Recipe = recipe };

                await _userRecipeRepository.CreateAsync(recipeToAdd);

                return Result.Success();
            }

            return Result.Failure(new Error("400", "The recipe is already added"));
        }

        public async Task<Result> DeleteFavoriteRecipe(DeleteFavoriteRecipeRequest request)
        {
            var user = await _identityService.FindUserByIdAsync(request.UserId);

            if (user is null)
            {
                return Result.Failure(new Error("404", "Such user doesn't exist"));
            }

            var recipe = await _recipeRepository.GetByIdAsync(request.RecipeId);

            if (recipe is null)
            {
                return Result.Failure(new Error("404", "Such recipe doesn't exist"));
            }

            var recipeFromFavoriteList = await _userRecipeRepository.GetOneByPredicateAsync(ur => ur.UserId == user.Id && ur.RecipeId == recipe.Id);

            if (recipeFromFavoriteList is null)
            {

                return Result.Failure(new Error("404", "Such recipe doesn't belong to the list"));
            }

            await _userRecipeRepository.RemoveAsync(recipeFromFavoriteList);

            return Result.Success();
        }

        public async Task<Result> AddRecipeIngredientsToShopList(AddRecipeIngredientsToShopListRequest request)
        {
            var user = await _identityService.FindUserByIdAsync(request.UserId);

            if (user is null)
            {
                return Result.Failure(new Error("404", "Such user doesn't exist"));
            }

            if (request.Ingredients is null || request.Ingredients.Count() == 0)
            {
                return Result.Failure(new Error("400", "No ingredients provided"));
            }

            await AddExistingIngredientsForShopList(request.Ingredients, request.UserId);

            return Result.Success();
        }

        public async Task<Result> UpdateShopList(UpdateShopListRequest request)
        {
            var user = await _identityService.FindUserByIdAsync(request.UserId);

            if (user is null)
            {
                return Result.Failure(new Error("404", "Such user doesn't exist"));
            }

            if (request.ExistingIngredients != null)
            {
                await UpdateExistingIngredientsForShopList(request.ExistingIngredients, request.UserId);
            }

            if (request.NewIngredients != null && request.NewIngredients.Count > 0)
            {
                await AddNewIngredientsForShopList(request.NewIngredients, request.UserId);
            }

            return Result.Success();
        }

        private async Task AddNewIngredientsForShopList(List<ShoppingListIngredientModel> ingredients, string userId)
        {
            var newIngredientsToAddToList = ingredients
                .Select(i => new UserIngredient { Ingredient = new Ingredient(i.Name), UserId = userId, Quantity = i.Quantity, IsBougth = i.IsBougth }).ToList();

            await _userIngredientRepository.CreateManyAsync(newIngredientsToAddToList);
        }

        private async Task UpdateExistingIngredientsForShopList(List<ShoppingListIngredientModel> ingredients, string userId)
        {
            var shoppingListIngredients = await _userIngredientRepository
            .GetManyByPredicateAsync(i => ingredients.Select(i => i.Id).Contains(i.IngredientId) && i.UserId == userId);

            var ingredientsToRemove = await _userIngredientRepository
            .GetManyByPredicateAsync(i => !ingredients.Select(i => i.Id).Contains(i.IngredientId) && i.UserId == userId);

            var ingredientsToAdd = new List<UserIngredient>();
            var ingredientsToUpdate = new List<UserIngredient>();

            foreach (var ingredient in ingredients)
            {
                var listIngredient = shoppingListIngredients.FirstOrDefault(ui => ui.IngredientId == ingredient.Id);

                if (listIngredient is null)
                {
                    ingredientsToAdd
                        .Add(new UserIngredient { IngredientId = ingredient.Id, UserId = userId, Quantity = ingredient.Quantity, IsBougth = ingredient.IsBougth });

                    continue;
                }

                listIngredient.Quantity = ingredient.Quantity;
                listIngredient.IsBougth = ingredient.IsBougth;
                ingredientsToUpdate.Add(listIngredient);
            }

            await _userIngredientRepository.CreateManyAsync(ingredientsToAdd);
            await _userIngredientRepository.RemoveManyAsync(ingredientsToRemove);
            await _userIngredientRepository.UpdateManyAsync(ingredientsToUpdate);
        }

        private async Task AddExistingIngredientsForShopList(List<ShoppingListIngredientModel> ingredients, string userId)
        {
            var shoppingListIngredients = await _userIngredientRepository
            .GetManyByPredicateAsync(i => ingredients.Select(i => i.Id).Contains(i.IngredientId) && i.UserId == userId);

            var ingredientsToAdd = new List<UserIngredient>();
            var ingredientsToUpdate = new List<UserIngredient>();

            foreach (var ingredient in ingredients)
            {
                var listIngredient = shoppingListIngredients.FirstOrDefault(ui => ui.IngredientId == ingredient.Id);

                if (listIngredient is null)
                {
                    ingredientsToAdd.Add(new UserIngredient { IngredientId = ingredient.Id, UserId = userId, Quantity = ingredient.Quantity });
                    continue;
                }

                listIngredient.Quantity += ingredient.Quantity;
                ingredientsToUpdate.Add(listIngredient);
            }

            await _userIngredientRepository.CreateManyAsync(ingredientsToAdd);
            await _userIngredientRepository.UpdateManyAsync(ingredientsToUpdate);
        }
    }
}
