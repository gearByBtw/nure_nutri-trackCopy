using FluentAssertions;
using Moq;
using NutritionalRecipeBook.Application.Services;
using NutritionalRecipeBook.Domain.Entities;
using NutritionalRecipeBook.Infrastructure.Contracts;

namespace NutritionalRecipeBook.Application.UnitTests
{
    public class CategoryServiceUnitTests
    {
        private readonly Mock<IGenericRepository<Category>> _categoryRepository;

        public CategoryServiceUnitTests()
        {
            _categoryRepository = new(MockBehavior.Strict);
        }

        [Fact]
        public async Task Should_ReturnAllCategories()
        {
            var categories = TestData.GetCategories();

            _categoryRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(categories);

            var categoryService = new CategoryService(_categoryRepository.Object);
            var result = await categoryService.GetAllAsync();

            result.Should().NotBeNull();
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().NotBeNull();
            result.Value.Should().HaveCount(categories.Count);
            result.Value.Should().BeEquivalentTo(categories);
        }
    }
}