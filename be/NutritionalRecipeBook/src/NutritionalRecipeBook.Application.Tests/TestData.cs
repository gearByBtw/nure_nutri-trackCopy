using Microsoft.AspNetCore.Identity;
using Moq;
using NutritionalRecipeBook.Application.Common.Models;
using NutritionalRecipeBook.Application.Common.Models.Recipe;
using NutritionalRecipeBook.Application.Common.Models.User;
using NutritionalRecipeBook.Domain.Entities;
using NutritionalRecipeBook.Domain.ValueObjects;
using Nutritionix;

namespace NutritionalRecipeBook.Application.UnitTests
{
    public static class TestData
    {
        public static List<Recipe> GetRecipes()
        {
            var categories = GetCategories();
            var ingredients = GetIngredients();
            var reviews = GetReviews();
            var recipeSpecifications = GetRecipeSpecifications();
            var recipes = new List<Recipe>();

            for (int i = 0; i < recipeSpecifications.Count && i < categories.Count; i++)
            {
                var recipe = new Recipe(recipeSpecifications[i], categories[i], "userId");

                var recipeIngredients = new List<RecipeIngredient>();

                foreach (var ingredient in ingredients)
                {
                    recipeIngredients
                        .Add(new RecipeIngredient
                        {
                            IngredientId = ingredient.Id,
                            Ingredient = ingredient,
                            RecipeId = recipe.Id,
                            Recipe = recipe
                        });
                }

                recipe.Ingredients = recipeIngredients;
                recipe.Reviews = reviews;
                recipes.Add(recipe);
            }

            return recipes;
        }

        public static List<GetRecipeResponse> GetRecipeResponses(List<Recipe> recipes)
        {
            var recipeResponses = new List<GetRecipeResponse>();

            foreach (var recipe in recipes)
            {
                var recipeResponse = new GetRecipeResponse
                {
                    Id = recipe.Id,
                    Calories = recipe.Calories,
                    Category = recipe.Category,
                    CookingProcess = recipe.CookingProcess,
                    CookingTime = recipe.CookingTime,
                    Description = recipe.Description,
                    Name = recipe.Name,
                    Reviews = recipe.Reviews,
                    ServingSizeInGrams = recipe.ServingSizeInGrams,
                    UserId = recipe.UserId,
                    Ingredients = recipe.Ingredients.Select(x => x.Ingredient)
                };

                recipeResponses.Add(recipeResponse);
            }

            return recipeResponses;
        }

        public static List<RecipeSpecification> GetRecipeSpecifications()
        {
            return new List<RecipeSpecification>
            {
                new RecipeSpecification("Recipe 1", "Description 1", "Cooking Process 1", TimeSpan.FromMinutes(30), 200, 500),
                new RecipeSpecification("Recipe 2", "Description 2", "Cooking Process 2", TimeSpan.FromMinutes(45), 300, 600),
                new RecipeSpecification("Recipe 3", "Description 3", "Cooking Process 3", TimeSpan.FromMinutes(60), 400, 600),
            };
        }

        public static List<Review> GetReviews()
        {
            return new List<Review>
            {
                new Review(4.5, "Great recipe!", Guid.NewGuid().ToString(), Guid.NewGuid(), "Author 1"),
                new Review(3.5, "Like it!", Guid.NewGuid().ToString(), Guid.NewGuid(), "Author 2"),
                new Review(5.0, "Delicious!", Guid.NewGuid().ToString(), Guid.NewGuid(), "Author 3"),
            };
        }

        public static List<Category> GetCategories()
        {
            return new List<Category>
            {
                new Category("Category 1"),
                new Category("Category 2"),
                new Category("Category 3")
            };
        }

        public static List<Ingredient> GetIngredients()
        {
            return new List<Ingredient>
            {
                new Ingredient("Ingredient 1"),
                new Ingredient("Ingredient 2"),
                new Ingredient("Ingredient 3")
            };
        }

        public static List<ShoppingListIngredientModel> GetShoppingListIngredients()
        {
            return new List<ShoppingListIngredientModel>
            {
                new ShoppingListIngredientModel
                {
                    Id = Guid.NewGuid(),
                    Name = "Name",
                    Quantity = 1,
                    IsBougth = false
                },
                new ShoppingListIngredientModel
                {
                    Id = Guid.NewGuid(),
                    Name = "Name",
                    Quantity = 1,
                    IsBougth = false
                },
                new ShoppingListIngredientModel
                {
                    Id = Guid.NewGuid(),
                    Name = "Name",
                    Quantity = 1,
                    IsBougth = false
                }
            };
        }

        public static NutritionData GetNutritionData()
        {
            return new NutritionData
            {
                Foods = new List<Food>
                {
                    new Food { Name = "Food1", Calories = 50 },
                    new Food { Name = "Food2", Calories = 70 }
                }
            };
        }

        public static List<User> GetUsers()
        {
            return new List<User>
            {
                new User { Id = Guid.NewGuid().ToString(), UserName = "TestName1" },
                new User { Id = Guid.NewGuid().ToString(), UserName = "TestName2" },
                new User { Id = Guid.NewGuid().ToString(), UserName = "TestName3" }
            };
        }

        public static List<GetUserResonse> GetUserResponses(List<User> users)
        {
            var userResponses = new List<GetUserResonse>();

            foreach (var user in users)
            {
                var userResponse = new GetUserResonse
                {
                    Id = user.Id,
                    UserName = user.UserName!
                };

                userResponses.Add(userResponse);
            }

            return userResponses;
        }

        public static Mock<UserManager<TUser>> MockUserManager<TUser>(List<TUser> users, string userId) where TUser : User
        {
            var store = new Mock<IUserStore<TUser>>();
            var mgr = new Mock<UserManager<TUser>>(store.Object, null, null, null, null, null, null, null, null);
            mgr.Object.UserValidators.Add(new UserValidator<TUser>());
            mgr.Object.PasswordValidators.Add(new PasswordValidator<TUser>());

            mgr.Setup(x => x.DeleteAsync(It.IsAny<TUser>()))
                .ReturnsAsync(IdentityResult.Success);

            mgr.Setup(x => x.CreateAsync(It.IsAny<TUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success)
                .Callback<TUser, string>((x, y) => users.Add(x));

            mgr.Setup(x => x.UpdateAsync(It.IsAny<TUser>()))
                .ReturnsAsync(IdentityResult.Success);

            mgr.Setup(m => m.FindByIdAsync(userId))
                .ReturnsAsync(users.Where(u => u.Id == userId).FirstOrDefault());

            return mgr;
        }

        public static Mock<INutritionixClient> MockINutritionixClient(NutritionData nutritionData, string queryString)
        {
            var nutritionixClientMock = new Mock<INutritionixClient>();

            nutritionixClientMock
                .Setup(client => client.GetNutritionData(It.IsAny<string>()))
                .ReturnsAsync((string query) => query == queryString ? nutritionData : null);

            return nutritionixClientMock;
        }
    }
}
