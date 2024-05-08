using FluentAssertions;
using Moq;
using NutritionalRecipeBook.Application.Services;
using NutritionalRecipeBook.Domain.Entities;
using NutritionalRecipeBook.Infrastructure.Contracts;

namespace NutritionalRecipeBook.Application.UnitTests
{
    public class IngredientServiceUnitTests
    {
        private readonly Mock<IGenericRepository<Ingredient>> _ingredientRepository;

        public IngredientServiceUnitTests()
        {
            _ingredientRepository = new(MockBehavior.Strict);
        }

        [Fact]
        public async Task Should_ReturnAllIngredients()
        {
            var ingredients = TestData.GetIngredients();

            _ingredientRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(ingredients);

            var ingredientService = new IngredientService(_ingredientRepository.Object);
            var result = await ingredientService.GetAllAsync();

            result.Should().NotBeNull();
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().NotBeNull();
            result.Value.Should().HaveCount(ingredients.Count);
            result.Value.Should().BeEquivalentTo(ingredients);
        }
    }
}
