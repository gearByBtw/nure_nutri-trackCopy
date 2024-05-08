using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Moq;
using NutritionalRecipeBook.Application.Common.Models.Recipe;
using NutritionalRecipeBook.Application.Common.Models.User;
using NutritionalRecipeBook.Application.Contracts;
using NutritionalRecipeBook.Application.Services;
using NutritionalRecipeBook.Domain.Entities;
using NutritionalRecipeBook.Infrastructure.Contracts;
using System.Linq.Expressions;

namespace NutritionalRecipeBook.Application.UnitTests
{
    public class UserServiceUnitTests
    {
        private Mock<IRecipeRepository> _recipeRepositoryMock;

        private Mock<IGenericRepository<UserIngredient>> _userIngredientRepositoryMock;

        private Mock<IGenericRepository<UserRecipe>> _userRecipeRepositoryMock;

        private Mock<IIdentityService> _identityServiceMock;

        private Mock<IMapper> _mapperMock;

        private List<User> _users = TestData.GetUsers();

        public UserServiceUnitTests()
        {
            _recipeRepositoryMock = new();
            _userIngredientRepositoryMock = new();
            _userRecipeRepositoryMock = new();
            _mapperMock = new();
            _identityServiceMock = new();
        }

        [Fact]
        public async Task Should_GetByIdAsync_WhenValidId_ReturnsUserResponse()
        {
            var mappedUsers = TestData.GetUserResponses(_users);

            _identityServiceMock
                .Setup(service => service.GetUserByIdWithRelationsAsync(It.IsAny<string>()))
                .ReturnsAsync(_users.FirstOrDefault());

            _mapperMock
                .Setup(mapper => mapper.Map<GetUserResonse>(It.IsAny<User>()))
                .Returns(mappedUsers.FirstOrDefault()!);

            var userService = new UserService(
                _userRecipeRepositoryMock.Object,
                _userIngredientRepositoryMock.Object,
                _recipeRepositoryMock.Object,
                _mapperMock.Object,
                _identityServiceMock.Object
                );

            var result = await userService.GetById(_users.FirstOrDefault()!.Id);

            result.IsSuccess.Should().BeTrue();
            _identityServiceMock.Verify(service => service.GetUserByIdWithRelationsAsync(It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task Should_GetByIdAsync_WhenInvalidId_ReturnsUserNotFound()
        {
            var users = TestData.GetUsers();
            var mappedUsers = TestData.GetUserResponses(_users);

            _identityServiceMock
                .Setup(service => service.GetUserByIdWithRelationsAsync(It.IsAny<string>()))
                .ReturnsAsync((User)null);

            _mapperMock
                .Setup(mapper => mapper.Map<GetUserResonse>(It.IsAny<User>()))
                .Returns(mappedUsers.FirstOrDefault()!);

            var userService = new UserService(
                _userRecipeRepositoryMock.Object,
                _userIngredientRepositoryMock.Object,
                _recipeRepositoryMock.Object,
                _mapperMock.Object,
                _identityServiceMock.Object
                );

            var result = await userService.GetById(users.FirstOrDefault()!.Id);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().NotBeNull();
            result.Error.Code.Should().Be("404");
            result.Error.Message.Should().Be("Such user doesn't exist");
        }

        [Fact]
        public async Task Should_AddFavoriteRecipe_WhenValidData_AddsFavoriteRecipes()
        {
            var recipe = TestData.GetRecipes().First();

            var request = new AddFavoriteRecipeRequest
            {
                RecipeId = recipe.Id,
                UserId = _users.FirstOrDefault()!.Id,
            };

            _identityServiceMock
                .Setup(service => service.FindUserByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(_users.FirstOrDefault());

            _recipeRepositoryMock
                .Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(recipe);

            _userRecipeRepositoryMock
                .Setup(repo => repo.GetOneByPredicateAsync(It.IsAny<Expression<Func<UserRecipe, bool>>>()))
                .ReturnsAsync((UserRecipe)null);

            _userRecipeRepositoryMock
                .Setup(repo => repo.CreateAsync(It.IsAny<UserRecipe>()))
                .Returns(Task.CompletedTask);

            var userService = new UserService(
                _userRecipeRepositoryMock.Object,
                _userIngredientRepositoryMock.Object,
                _recipeRepositoryMock.Object,
                _mapperMock.Object,
                _identityServiceMock.Object
                );

            var result = await userService.AddFavoriteRecipe(request);

            result.IsSuccess.Should().BeTrue();
            _recipeRepositoryMock.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
            _identityServiceMock.Verify(service => service.FindUserByIdAsync(It.IsAny<string>()), Times.Once);
            _userRecipeRepositoryMock.Verify(repo => repo.GetOneByPredicateAsync(It.IsAny<Expression<Func<UserRecipe, bool>>>()), Times.Once);
            _userRecipeRepositoryMock.Verify(repo => repo.CreateAsync(It.IsAny<UserRecipe>()), Times.Once);
        }

        [Fact]
        public async Task Should_AddFavoriteRecipe_WhenInvalidUserId_ReturnsUserNotFound()
        {
            var recipe = TestData.GetRecipes().First();

            var request = new AddFavoriteRecipeRequest
            {
                RecipeId = recipe.Id,
                UserId = "InvalidId",
            };

            _identityServiceMock
                .Setup(service => service.FindUserByIdAsync(It.IsAny<string>()))
                .ReturnsAsync((User)null);

            var userService = new UserService(
                _userRecipeRepositoryMock.Object,
                _userIngredientRepositoryMock.Object,
                _recipeRepositoryMock.Object,
                _mapperMock.Object,
                _identityServiceMock.Object
                );

            var result = await userService.AddFavoriteRecipe(request);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().NotBeNull();
            result.Error.Code.Should().Be("404");
            result.Error.Message.Should().Be("Such user doesn't exist");
        }

        [Fact]
        public async Task Should_AddFavoriteRecipe_WhenInvaidRecipeId_ReturnsRecipeNotFound()
        {
            var recipe = TestData.GetRecipes().First();

            var request = new AddFavoriteRecipeRequest
            {
                RecipeId = recipe.Id,
                UserId = _users.FirstOrDefault()!.Id,
            };

            _identityServiceMock
                .Setup(service => service.FindUserByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(_users.FirstOrDefault());

            _recipeRepositoryMock
                .Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync((Recipe)null);

            var userService = new UserService(
                _userRecipeRepositoryMock.Object,
                _userIngredientRepositoryMock.Object,
                _recipeRepositoryMock.Object,
                _mapperMock.Object,
                _identityServiceMock.Object
                );

            var result = await userService.AddFavoriteRecipe(request);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().NotBeNull();
            result.Error.Code.Should().Be("404");
            result.Error.Message.Should().Be("Such recipe doesn't exist");
        }

        [Fact]
        public async Task Should_AddFavoriteRecipe_WhenRecipeAlreadyAdded_ReturnsFailure()
        {
            var recipe = TestData.GetRecipes().First();

            var request = new AddFavoriteRecipeRequest
            {
                RecipeId = recipe.Id,
                UserId = _users.FirstOrDefault()!.Id,
            };

            _identityServiceMock
                .Setup(service => service.FindUserByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(_users.FirstOrDefault());

            _recipeRepositoryMock
                .Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(recipe);

            _userRecipeRepositoryMock
                .Setup(repo => repo.GetOneByPredicateAsync(It.IsAny<Expression<Func<UserRecipe, bool>>>()))
                .ReturnsAsync(new UserRecipe());

            var userService = new UserService(
                _userRecipeRepositoryMock.Object,
                _userIngredientRepositoryMock.Object,
                _recipeRepositoryMock.Object,
                _mapperMock.Object,
                _identityServiceMock.Object
                );

            var result = await userService.AddFavoriteRecipe(request);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().NotBeNull();
            result.Error.Code.Should().Be("400");
            result.Error.Message.Should().Be("The recipe is already added");
        }

        [Fact]
        public async Task Should_DeleteFavoriteRecipe_WhenValidData_DeletesFavoriteRecipes()
        {
            var recipe = TestData.GetRecipes().First();

            var request = new DeleteFavoriteRecipeRequest
            {
                RecipeId = recipe.Id,
                UserId = _users.FirstOrDefault()!.Id,
            };

            _identityServiceMock
                .Setup(service => service.FindUserByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(_users.FirstOrDefault());

            _recipeRepositoryMock
                .Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(recipe);

            _userRecipeRepositoryMock
                .Setup(repo => repo.GetOneByPredicateAsync(It.IsAny<Expression<Func<UserRecipe, bool>>>()))
                .ReturnsAsync(new UserRecipe());

            _userRecipeRepositoryMock
                .Setup(repo => repo.RemoveAsync(It.IsAny<UserRecipe>()))
                .Returns(Task.CompletedTask);

            var userService = new UserService(
                _userRecipeRepositoryMock.Object,
                _userIngredientRepositoryMock.Object,
                _recipeRepositoryMock.Object,
                _mapperMock.Object,
                _identityServiceMock.Object
                );

            var result = await userService.DeleteFavoriteRecipe(request);

            result.IsSuccess.Should().BeTrue();
            _userRecipeRepositoryMock.Verify(repo => repo.RemoveAsync(It.IsAny<UserRecipe>()), Times.Once);
        }

        [Fact]
        public async Task Should_DeleteFavoriteRecipe_WhenInvalidUserId_ReturnsUserNotFound()
        {
            var recipe = TestData.GetRecipes().First();

            var request = new DeleteFavoriteRecipeRequest
            {
                RecipeId = recipe.Id,
                UserId = _users.FirstOrDefault()!.Id,
            };

            _identityServiceMock
                .Setup(service => service.FindUserByIdAsync(It.IsAny<string>()))
                .ReturnsAsync((User)null);

            var userService = new UserService(
                _userRecipeRepositoryMock.Object,
                _userIngredientRepositoryMock.Object,
                _recipeRepositoryMock.Object,
                _mapperMock.Object,
                _identityServiceMock.Object
                );

            var result = await userService.DeleteFavoriteRecipe(request);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().NotBeNull();
            result.Error.Code.Should().Be("404");
            result.Error.Message.Should().Be("Such user doesn't exist");
        }

        [Fact]
        public async Task Should_DeleteFavoriteRecipe_WhenInvaidRecipeId_ReturnsRecipeNotFound()
        {
            var recipe = TestData.GetRecipes().First();

            var request = new DeleteFavoriteRecipeRequest
            {
                RecipeId = recipe.Id,
                UserId = _users.First().Id,
            };

            _identityServiceMock
                .Setup(service => service.FindUserByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(_users.FirstOrDefault());

            _recipeRepositoryMock
                .Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync((Recipe)null);

            var userService = new UserService(
                _userRecipeRepositoryMock.Object,
                _userIngredientRepositoryMock.Object,
                _recipeRepositoryMock.Object,
                _mapperMock.Object,
                _identityServiceMock.Object
                );

            var result = await userService.DeleteFavoriteRecipe(request);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().NotBeNull();
            result.Error.Code.Should().Be("404");
            result.Error.Message.Should().Be("Such recipe doesn't exist");
        }

        [Fact]
        public async Task Should_DeleteFavoriteRecipe_WhenFavoriteRecipeNotAdded_ReturnsFailure()
        {
            var recipe = TestData.GetRecipes().First();

            var request = new DeleteFavoriteRecipeRequest
            {
                RecipeId = recipe.Id,
                UserId = _users.First().Id,
            };

            _identityServiceMock
                .Setup(service => service.FindUserByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(_users.FirstOrDefault());

            _recipeRepositoryMock
                .Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(recipe);

            _userRecipeRepositoryMock
                .Setup(repo => repo.GetOneByPredicateAsync(It.IsAny<Expression<Func<UserRecipe, bool>>>()))
                .ReturnsAsync((UserRecipe)null);

            var userService = new UserService(
                _userRecipeRepositoryMock.Object,
                _userIngredientRepositoryMock.Object,
                _recipeRepositoryMock.Object,
                _mapperMock.Object,
                _identityServiceMock.Object
                );

            var result = await userService.DeleteFavoriteRecipe(request);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().NotBeNull();
            result.Error.Code.Should().Be("404");
            result.Error.Message.Should().Be("Such recipe doesn't belong to the list");
        }

        [Fact]
        public async Task Should_AddRecipeIngredientsToShopList_WhenValidData_AddsIngredients()
        {
            var ingredients = TestData.GetShoppingListIngredients();

            var request = new AddRecipeIngredientsToShopListRequest
            {
                Ingredients = ingredients,
                UserId = _users.FirstOrDefault()!.Id,
            };

            _identityServiceMock
                .Setup(service => service.FindUserByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(_users.FirstOrDefault());

            _userIngredientRepositoryMock
                .Setup(repo => repo.GetManyByPredicateAsync(It.IsAny<Expression<Func<UserIngredient, bool>>>()))
                .ReturnsAsync(new List<UserIngredient>());

            _userIngredientRepositoryMock
                .Setup(repo => repo.CreateManyAsync(It.IsAny<List<UserIngredient>>()))
                .Returns(Task.CompletedTask);

            _userIngredientRepositoryMock
               .Setup(repo => repo.UpdateManyAsync(It.IsAny<List<UserIngredient>>()))
               .Returns(Task.CompletedTask);

            var userService = new UserService(
                _userRecipeRepositoryMock.Object,
                _userIngredientRepositoryMock.Object,
                _recipeRepositoryMock.Object,
                _mapperMock.Object,
                _identityServiceMock.Object
                );

            var result = await userService.AddRecipeIngredientsToShopList(request);

            result.IsSuccess.Should().BeTrue();
            _userIngredientRepositoryMock.Verify(repo => repo.CreateManyAsync(It.IsAny<List<UserIngredient>>()), Times.Once);
            _userIngredientRepositoryMock.Verify(repo => repo.UpdateManyAsync(It.IsAny<List<UserIngredient>>()), Times.Once);
        }

        [Fact]
        public async Task Should_AddRecipeIngredientsToShopList_WhenInvalidUserId_ReturnsUserNotFound()
        {
            var ingredients = TestData.GetShoppingListIngredients();

            var request = new AddRecipeIngredientsToShopListRequest
            {
                Ingredients = ingredients,
                UserId = "InvalidId",
            };

            _identityServiceMock
                .Setup(service => service.FindUserByIdAsync(It.IsAny<string>()))
                .ReturnsAsync((User)null);

            var userService = new UserService(
                _userRecipeRepositoryMock.Object,
                _userIngredientRepositoryMock.Object,
                _recipeRepositoryMock.Object,
                _mapperMock.Object,
                _identityServiceMock.Object
                );

            var result = await userService.AddRecipeIngredientsToShopList(request);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().NotBeNull();
            result.Error.Code.Should().Be("404");
            result.Error.Message.Should().Be("Such user doesn't exist");
        }

        [Fact]
        public async Task Should_UpdateToShopList_WhenValidData_UpdatesIngredients()
        {
            var existingIngredients = TestData.GetShoppingListIngredients();
            var newIngredients = TestData.GetShoppingListIngredients();

            var request = new UpdateShopListRequest
            {
                ExistingIngredients = existingIngredients,
                NewIngredients = newIngredients,
                UserId = _users.First().Id,
            }; 
            
            _identityServiceMock
                .Setup(service => service.FindUserByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(_users.FirstOrDefault());

            _userIngredientRepositoryMock
                .Setup(repo => repo.GetManyByPredicateAsync(It.IsAny<Expression<Func<UserIngredient, bool>>>()))
                .ReturnsAsync(new List<UserIngredient>());

            _userIngredientRepositoryMock
                .Setup(repo => repo.CreateManyAsync(It.IsAny<List<UserIngredient>>()))
                .Returns(Task.CompletedTask);

            _userIngredientRepositoryMock
               .Setup(repo => repo.UpdateManyAsync(It.IsAny<List<UserIngredient>>()))
               .Returns(Task.CompletedTask);

            _userIngredientRepositoryMock
               .Setup(repo => repo.RemoveManyAsync(It.IsAny<List<UserIngredient>>()))
               .Returns(Task.CompletedTask);

            var userService = new UserService(
                _userRecipeRepositoryMock.Object,
                _userIngredientRepositoryMock.Object,
                _recipeRepositoryMock.Object,
                _mapperMock.Object,
                _identityServiceMock.Object
                );

            var result = await userService.UpdateShopList(request);

            result.IsSuccess.Should().BeTrue();
            _userIngredientRepositoryMock.Verify(repo => repo.CreateManyAsync(It.IsAny<List<UserIngredient>>()), Times.Exactly(2));
            _userIngredientRepositoryMock.Verify(repo => repo.UpdateManyAsync(It.IsAny<List<UserIngredient>>()), Times.Once);
            _userIngredientRepositoryMock.Verify(repo => repo.RemoveManyAsync(It.IsAny<List<UserIngredient>>()), Times.Once);
        }

        [Fact]
        public async Task Should_UpdateShopList_WhenInvalidUserId_ReturnsUserNotFound()
        {
            var existingIngredients = TestData.GetShoppingListIngredients();
            var newIngredients = TestData.GetShoppingListIngredients();

            var request = new UpdateShopListRequest
            {
                ExistingIngredients = existingIngredients,
                NewIngredients = newIngredients,
                UserId = "InvalidId",
            };

            _identityServiceMock
                .Setup(service => service.FindUserByIdAsync(It.IsAny<string>()))
                .ReturnsAsync((User)null);

            var userService = new UserService(
                _userRecipeRepositoryMock.Object,
                _userIngredientRepositoryMock.Object,
                _recipeRepositoryMock.Object,
                _mapperMock.Object,
                _identityServiceMock.Object
                );

            var result = await userService.UpdateShopList(request);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().NotBeNull();
            result.Error.Code.Should().Be("404");
            result.Error.Message.Should().Be("Such user doesn't exist");
        }
    }
}