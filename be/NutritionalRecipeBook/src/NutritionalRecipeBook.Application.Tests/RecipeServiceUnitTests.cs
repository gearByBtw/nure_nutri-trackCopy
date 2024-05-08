using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Moq;
using NutritionalRecipeBook.Application.Common.Collections;
using NutritionalRecipeBook.Application.Common.Models;
using NutritionalRecipeBook.Application.Common.Models.Recipe;
using NutritionalRecipeBook.Application.Contracts;
using NutritionalRecipeBook.Application.Services;
using NutritionalRecipeBook.Domain.Entities;
using NutritionalRecipeBook.Infrastructure.Contracts;
using System.Linq.Expressions;

namespace NutritionalRecipeBook.Application.UnitTests
{
    public class RecipeServiceUnitTests
    {
        private Mock<IGenericRepository<Ingredient>> _ingredientRepositoryMock;

        private Mock<IRecipeRepository> _recipeRepositoryMock;

        private Mock<IGenericRepository<RecipeIngredient>> _recipeIngredientRepositoryMock;

        private Mock<IGenericRepository<Category>> _categoryRepositoryMock;

        private Mock<INutritionService> _nutritionServiceMock;

        private Mock<IIdentityService> _identityServiceMock;

        private Mock<IMapper> _mapperMock;

        private List<User> _users = TestData.GetUsers();

        public RecipeServiceUnitTests()
        {
            _ingredientRepositoryMock = new();
            _recipeRepositoryMock = new();
            _recipeIngredientRepositoryMock = new();
            _categoryRepositoryMock = new();
            _nutritionServiceMock = new();
            _mapperMock = new();
            _identityServiceMock = new();
        }

        [Fact]
        public async Task Should_GetAllAsync_WhenValidData_ReturnsAllRecipes()
        {
            var searchParams = new SearchParams
            {
                PageNumber = 1,
                PageSize = 3,
                SearchTerm = null,
                Filters = null,
                MinCalories = null,
                MaxCalories = null
            };

            var recipes = TestData.GetRecipes();
            var mappedRecipes = TestData.GetRecipeResponses(recipes);
            var pagedRecipes = new PagedList<Recipe>(recipes, searchParams.PageNumber, searchParams.PageSize);

            _recipeRepositoryMock
                .Setup(repo => repo.GetAllSearchedAsync(searchParams))
                .ReturnsAsync(pagedRecipes);

            _mapperMock
                .Setup(mapper => mapper.Map<List<GetRecipeResponse>>(It.IsAny<List<Recipe>>()))
                .Returns(mappedRecipes);

            var recipeService = new RecipeService(
                _ingredientRepositoryMock.Object,
                _recipeRepositoryMock.Object,
                _recipeIngredientRepositoryMock.Object,
                _categoryRepositoryMock.Object,
                _nutritionServiceMock.Object,
                _mapperMock.Object,
                _identityServiceMock.Object
                );

            var result = await recipeService.GetAllAsync(searchParams);


            result.Should().NotBeNull();
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().NotBeNull();
            result.Value.Should().HaveCount(recipes.Count);
            result.Value.Should().BeEquivalentTo(mappedRecipes);
        }

        [Fact]
        public async Task Should_GetByIdAsync_WhenValidId_ReturnsRecipe()
        {
            var recipes = TestData.GetRecipes();

            var recipe = recipes.FirstOrDefault();

            var recipeId = recipe!.Id;

            _recipeRepositoryMock
                .Setup(repo => repo.GetByIdAsync(recipeId))
                .ReturnsAsync(recipes.FirstOrDefault(r => r.Id == recipeId));

            var recipeService = new RecipeService(
                _ingredientRepositoryMock.Object,
                _recipeRepositoryMock.Object,
                _recipeIngredientRepositoryMock.Object,
                _categoryRepositoryMock.Object,
                _nutritionServiceMock.Object,
                _mapperMock.Object,
                _identityServiceMock.Object
                );

            var result = await recipeService.GetByIdAsync(recipeId);

            result.Should().NotBeNull();
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().NotBeNull();
            result.Value.Should().BeEquivalentTo(recipe);
        }

        [Fact]
        public async Task Should_GetByIdAsync_WhenInvalidId_ReturnsRecipeNotFound()
        {
            var recipes = TestData.GetRecipes();

            var recipe = recipes.FirstOrDefault();

            var recipeId = Guid.NewGuid();

            _recipeRepositoryMock
                .Setup(repo => repo.GetByIdAsync(recipeId))
                .ReturnsAsync(recipes.FirstOrDefault(r => r.Id == recipeId));

            var recipeService = new RecipeService(
                _ingredientRepositoryMock.Object,
                _recipeRepositoryMock.Object,
                _recipeIngredientRepositoryMock.Object,
                _categoryRepositoryMock.Object,
                _nutritionServiceMock.Object,
                _mapperMock.Object,
                _identityServiceMock.Object
                );

            var result = await recipeService.GetByIdAsync(recipeId);

            result.Should().NotBeNull();
            result.IsFailure.Should().BeTrue();
            result.Error.Code.Should().Be("404");
            result.Error.Message.Should().Be("Such recipe doesn't exist.");
        }

        [Fact]
        public async Task Should_GetByIdWithRelationsAsync_WhenValidId_ReturnsRecipe()
        {
            var recipes = TestData.GetRecipes();
            var mappedRecipes = TestData.GetRecipeResponses(recipes);

            var recipe = recipes.FirstOrDefault();
            var mappedRecipe = mappedRecipes.FirstOrDefault();

            var recipeId = recipe!.Id;

            _recipeRepositoryMock
                .Setup(repo => repo.GetByIdWithRelationsAsync(recipeId))
                .ReturnsAsync(recipes.FirstOrDefault(r => r.Id == recipeId));

            _mapperMock
                .Setup(mapper => mapper.Map<GetRecipeResponse>(It.IsAny<Recipe>()))
                .Returns(mappedRecipe!);

            var recipeService = new RecipeService(
                _ingredientRepositoryMock.Object,
                _recipeRepositoryMock.Object,
                _recipeIngredientRepositoryMock.Object,
                _categoryRepositoryMock.Object,
                _nutritionServiceMock.Object,
                _mapperMock.Object,
                _identityServiceMock.Object
                );

            var result = await recipeService.GetByIdWithRelationsAsync(recipeId);

            result.Should().NotBeNull();
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().NotBeNull();
            result.Value.Category.Should().NotBeNull();
            result.Value.Ingredients.Should().NotBeNull();
            result.Value.Ingredients.Should().HaveCount(recipe.Ingredients.Count);
            result.Value.Reviews.Should().NotBeNull();
            result.Value.Reviews.Should().HaveCount(recipe.Reviews.Count);
            result.Value.Should().BeEquivalentTo(mappedRecipe);
        }

        [Fact]
        public async Task Should_GetByIdWithRelationsAsync_WhenInvalidId_ReturnsRecipeNotFound()
        {
            var recipes = TestData.GetRecipes();
            var mappedRecipes = TestData.GetRecipeResponses(recipes);

            var recipe = recipes.FirstOrDefault();
            var mappedRecipe = mappedRecipes.FirstOrDefault();

            var recipeId = Guid.NewGuid();

            _recipeRepositoryMock
                .Setup(repo => repo.GetByIdWithRelationsAsync(recipeId))
                .ReturnsAsync(recipes.FirstOrDefault(r => r.Id == recipeId));

            _mapperMock
                .Setup(mapper => mapper.Map<GetRecipeResponse>(It.IsAny<Recipe>()))
                .Returns(mappedRecipe!);

            var recipeService = new RecipeService(
                _ingredientRepositoryMock.Object,
                _recipeRepositoryMock.Object,
                _recipeIngredientRepositoryMock.Object,
                _categoryRepositoryMock.Object,
                _nutritionServiceMock.Object,
                _mapperMock.Object,
                _identityServiceMock.Object
                );

            var result = await recipeService.GetByIdWithRelationsAsync(recipeId);

            result.Should().NotBeNull();
            result.IsFailure.Should().BeTrue();
            result.Error.Should().NotBeNull();
            result.Error.Code.Should().Be("404");
            result.Error.Message.Should().Be("Such recipe doesn't exist.");
        }

        [Fact]
        public async Task Should_CreateAsync_WhenValidData_CreatesRecipe()
        {
            var newIngredients = new List<Ingredient>
            {
                new Ingredient ("NewIngredient 1"),
                new Ingredient ("NewIngredient 2")
            };

            var existingIngredients = new List<Ingredient>
            {
                new Ingredient ("ExistingIngredient 1"),
                new Ingredient ("ExistingIngredient 2")
            };

            var request = new CreateRecipeRequest
            {
                Name = "Name1",
                Description = "Description1",
                CookingProcess = "CookingProcess1",
                CookingTime = TimeSpan.FromMinutes(45),
                ServingSizeInGrams = 250,
                Category = TestData.GetCategories().FirstOrDefault()!,
                NewIngredients = newIngredients,
                ExistingIngredients = existingIngredients,
                UserId = Guid.NewGuid().ToString(),
            };


            _nutritionServiceMock
                .Setup(service => service.GetRecipeCalories(request.ServingSizeInGrams, It.IsAny<IEnumerable<Ingredient>>()))
                .ReturnsAsync(500);

            _categoryRepositoryMock
                .Setup(repo => repo.GetByIdAsync(request.Category.Id))
                .ReturnsAsync(request.Category);

            _recipeRepositoryMock
                .Setup(repo => repo.CreateAsync(It.IsAny<Recipe>()))
                .Returns(Task.CompletedTask);

            _ingredientRepositoryMock
                .Setup(repo => repo.GetManyByPredicateAsync(It.IsAny<Expression<Func<Ingredient, bool>>>()))
                .ReturnsAsync(new List<Ingredient>());

            _ingredientRepositoryMock
                .Setup(repo => repo.CreateAsync(It.IsAny<Ingredient>()))
                .Returns(Task.CompletedTask);

            _recipeIngredientRepositoryMock
                .Setup(repo => repo.CreateManyAsync(It.IsAny<List<RecipeIngredient>>()))
                .Returns(Task.CompletedTask);

            var recipeService = new RecipeService(
                _ingredientRepositoryMock.Object,
                _recipeRepositoryMock.Object,
                _recipeIngredientRepositoryMock.Object,
                _categoryRepositoryMock.Object,
                _nutritionServiceMock.Object,
                _mapperMock.Object,
                _identityServiceMock.Object
                );

            var result = await recipeService.CreateAsync(request);
            result.IsSuccess.Should().BeTrue();
            _recipeRepositoryMock.Verify(repo => repo.CreateAsync(It.IsAny<Recipe>()), Times.Once);
        }

        [Fact]
        public async Task Should_UpdateAsync_WhenValidData_UpdatesRecipe()
        {
            var request = new UpdateRecipeRequest
            {
                Id = Guid.NewGuid(),
                Name = "Name1",
                Description = "Description1",
                CookingProcess = "CookingProcess1",
                CookingTime = TimeSpan.FromMinutes(30),
                ServingSizeInGrams = 250,
                Category = TestData.GetCategories().FirstOrDefault()!,
                NewIngredients = TestData.GetIngredients(),
                ExistingIngredients = TestData.GetIngredients(),
                UserId = _users.FirstOrDefault()!.Id
            };

            _identityServiceMock
                .Setup(service => service.FindUserByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(_users.FirstOrDefault());

            var recipe = TestData.GetRecipes().FirstOrDefault();
            recipe!.UserId = _users.FirstOrDefault()!.Id;

            _recipeRepositoryMock
                .Setup(repo => repo.GetByIdWithRelationsAsync(It.IsAny<Guid>()))
                .ReturnsAsync(recipe);

            _recipeIngredientRepositoryMock
                .Setup(repo => repo.GetManyByPredicateAsync(It.IsAny<Expression<Func<RecipeIngredient, bool>>>()))
                .ReturnsAsync(new List<RecipeIngredient>());

            _ingredientRepositoryMock
                .Setup(repo => repo.CreateManyAsync(It.IsAny<List<Ingredient>>()))
                .Returns(Task.CompletedTask);

            _recipeIngredientRepositoryMock
                .Setup(repo => repo.CreateManyAsync(It.IsAny<List<RecipeIngredient>>()))
                .Returns(Task.CompletedTask);

            _categoryRepositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(request.Category);

            _nutritionServiceMock
                .Setup(service => service.GetRecipeCalories(It.IsAny<int>(), It.IsAny<IEnumerable<Ingredient>>()))
                .ReturnsAsync(500);

            var recipeService = new RecipeService(
                _ingredientRepositoryMock.Object,
                _recipeRepositoryMock.Object,
                _recipeIngredientRepositoryMock.Object,
                _categoryRepositoryMock.Object,
                _nutritionServiceMock.Object,
                _mapperMock.Object,
                _identityServiceMock.Object
                );

            var result = await recipeService.UpdateAsync(request);

            result.IsSuccess.Should().BeTrue();
            _recipeRepositoryMock.Verify(repo => repo.UpdateAsync(It.IsAny<Recipe>()), Times.Once);
        }

        [Fact]
        public async Task Should_UpdateAsync_WhenInvalidUserId_ReturnsUserNotFound()
        {
            var request = new UpdateRecipeRequest
            {
                Id = Guid.NewGuid(),
                Name = "Name1",
                Description = "Description1",
                CookingProcess = "CookingProcess1",
                CookingTime = TimeSpan.FromMinutes(30),
                ServingSizeInGrams = 250,
                Category = TestData.GetCategories().FirstOrDefault()!,
                NewIngredients = TestData.GetIngredients(),
                ExistingIngredients = TestData.GetIngredients(),
                UserId = "InvalidId"
            };

            _identityServiceMock
                .Setup(service => service.FindUserByIdAsync(It.IsAny<string>()))
                .ReturnsAsync((User)null);

            var recipe = TestData.GetRecipes().FirstOrDefault();
            recipe!.UserId = _users.FirstOrDefault()!.Id;

            var recipeService = new RecipeService(
                _ingredientRepositoryMock.Object,
                _recipeRepositoryMock.Object,
                _recipeIngredientRepositoryMock.Object,
                _categoryRepositoryMock.Object,
                _nutritionServiceMock.Object,
                _mapperMock.Object,
                _identityServiceMock.Object
                );

            var result = await recipeService.UpdateAsync(request);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().NotBeNull();
            result.Error.Code.Should().Be("404");
            result.Error.Message.Should().Be("Such user doesn't exist");
        }

        [Fact]
        public async Task Should_UpdateAsync_WhenInvalidRecipeId_ReturnsRecipeNotFound()
        {
            var request = new UpdateRecipeRequest
            {
                Id = Guid.NewGuid(),
                Name = "Name1",
                Description = "Description1",
                CookingProcess = "CookingProcess1",
                CookingTime = TimeSpan.FromMinutes(30),
                ServingSizeInGrams = 250,
                Category = TestData.GetCategories().FirstOrDefault()!,
                NewIngredients = TestData.GetIngredients(),
                ExistingIngredients = TestData.GetIngredients(),
                UserId = _users.FirstOrDefault()!.Id
            };

            _identityServiceMock
                .Setup(service => service.FindUserByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(_users.FirstOrDefault());

            var recipe = TestData.GetRecipes().FirstOrDefault();
            recipe!.UserId = _users.FirstOrDefault()!.Id;

            _recipeRepositoryMock
                .Setup(repo => repo.GetByIdWithRelationsAsync(It.IsAny<Guid>()))
                .ReturnsAsync((Recipe)null!);

            var recipeService = new RecipeService(
                _ingredientRepositoryMock.Object,
                _recipeRepositoryMock.Object,
                _recipeIngredientRepositoryMock.Object,
                _categoryRepositoryMock.Object,
                _nutritionServiceMock.Object,
                _mapperMock.Object,
                _identityServiceMock.Object
                );

            var result = await recipeService.UpdateAsync(request);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().NotBeNull();
            result.Error.Code.Should().Be("404");
            result.Error.Message.Should().Be("Such recipe doesn't exist");
        }

        [Fact]
        public async Task Should_UpdateAsync_WhenInvalidUserId_ReturnsAccessDenied()
        {
            var request = new UpdateRecipeRequest
            {
                Id = Guid.NewGuid(),
                Name = "Name1",
                Description = "Description1",
                CookingProcess = "CookingProcess1",
                CookingTime = TimeSpan.FromMinutes(30),
                ServingSizeInGrams = 250,
                Category = TestData.GetCategories().FirstOrDefault()!,
                NewIngredients = TestData.GetIngredients(),
                ExistingIngredients = TestData.GetIngredients(),
                UserId = _users.FirstOrDefault()!.Id
            };

            _identityServiceMock
                .Setup(service => service.FindUserByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(_users.FirstOrDefault());

            var recipe = TestData.GetRecipes().FirstOrDefault();
            recipe!.UserId = _users.LastOrDefault()!.Id;

            _recipeRepositoryMock
                .Setup(repo => repo.GetByIdWithRelationsAsync(It.IsAny<Guid>()))
                .ReturnsAsync(recipe);

            var recipeService = new RecipeService(
                _ingredientRepositoryMock.Object,
                _recipeRepositoryMock.Object,
                _recipeIngredientRepositoryMock.Object,
                _categoryRepositoryMock.Object,
                _nutritionServiceMock.Object,
                _mapperMock.Object,
                _identityServiceMock.Object
                );

            var result = await recipeService.UpdateAsync(request);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().NotBeNull();
            result.Error.Code.Should().Be("403");
            result.Error.Message.Should().Be("Recipes can be updated only by creator");
        }

        [Fact]
        public async Task Should_DeleteById_WhenValidData_DeletesRecipe()
        {
            var userId = _users.FirstOrDefault()!.Id;

            _identityServiceMock
                .Setup(service => service.FindUserByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(_users.FirstOrDefault());

            var recipe = TestData.GetRecipes().FirstOrDefault();
            recipe.UserId = userId;

            _recipeRepositoryMock
                .Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(recipe);

            _recipeRepositoryMock
                .Setup(repo => repo.RemoveByIdAsync(It.IsAny<Guid>()))
                .Returns(Task.CompletedTask);

            var recipeService = new RecipeService(
                _ingredientRepositoryMock.Object,
                _recipeRepositoryMock.Object,
                _recipeIngredientRepositoryMock.Object,
                _categoryRepositoryMock.Object,
                _nutritionServiceMock.Object,
                _mapperMock.Object,
                _identityServiceMock.Object
                );

            var result = await recipeService.DeleteByIdAsync(recipe!.Id, userId);

            result.IsSuccess.Should().BeTrue();
            _recipeRepositoryMock.Verify(repo => repo.RemoveByIdAsync(It.IsAny<Guid>()), Times.Once); 
        }

        [Fact]
        public async Task Should_DeleteById_WhenInvalidRecipeId_ReturnsRecipeNotFound()
        {
            var userId = _users.FirstOrDefault()!.Id;

            _identityServiceMock
                .Setup(service => service.FindUserByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(_users.FirstOrDefault());

            var recipe = TestData.GetRecipes().FirstOrDefault();

            _recipeRepositoryMock
                .Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync((Recipe)null);

            var recipeService = new RecipeService(
                _ingredientRepositoryMock.Object,
                _recipeRepositoryMock.Object,
                _recipeIngredientRepositoryMock.Object,
                _categoryRepositoryMock.Object,
                _nutritionServiceMock.Object,
                _mapperMock.Object,
                _identityServiceMock.Object
                );

            var result = await recipeService.DeleteByIdAsync(recipe!.Id, userId);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().NotBeNull();
            result.Error.Code.Should().Be("404");
            result.Error.Message.Should().Be("Such recipe doesn't exist.");
        }

        [Fact]
        public async Task Should_DeleteById_WhenInvalidUserId_ReturnsAccessDenied()
        {
            var userId = _users.FirstOrDefault()!.Id;

            _identityServiceMock
                .Setup(service => service.FindUserByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(_users.FirstOrDefault());

            var recipe = TestData.GetRecipes().FirstOrDefault();
            recipe!.UserId = _users.LastOrDefault()!.Id;

            _recipeRepositoryMock
                .Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(recipe);

            var recipeService = new RecipeService(
                _ingredientRepositoryMock.Object,
                _recipeRepositoryMock.Object,
                _recipeIngredientRepositoryMock.Object,
                _categoryRepositoryMock.Object,
                _nutritionServiceMock.Object,
                _mapperMock.Object,
                _identityServiceMock.Object
                );

            var result = await recipeService.DeleteByIdAsync(recipe.Id, userId);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().NotBeNull();
            result.Error.Code.Should().Be("403");
            result.Error.Message.Should().Be("Recipes can be deleted only by creator");
        }

        [Fact]
        public async Task Should_DeleteById_WhenInvalidUserId_ReturnsUserNotFound()
        {
            _identityServiceMock
                .Setup(service => service.FindUserByIdAsync(It.IsAny<string>()))
                .ReturnsAsync((User)null);

            var recipe = TestData.GetRecipes().FirstOrDefault();

            var recipeService = new RecipeService(
                _ingredientRepositoryMock.Object,
                _recipeRepositoryMock.Object,
                _recipeIngredientRepositoryMock.Object,
                _categoryRepositoryMock.Object,
                _nutritionServiceMock.Object,
                _mapperMock.Object,
                _identityServiceMock.Object
                );

            var result = await recipeService.DeleteByIdAsync(recipe.Id, recipe.UserId);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().NotBeNull();
            result.Error.Code.Should().Be("404");
            result.Error.Message.Should().Be("Such user doesn't exist");
        }
    }
}
