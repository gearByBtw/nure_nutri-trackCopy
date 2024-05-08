using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Moq;
using NutritionalRecipeBook.Application.Common.Models;
using NutritionalRecipeBook.Application.Contracts;
using NutritionalRecipeBook.Application.Services;
using NutritionalRecipeBook.Domain.Entities;
using NutritionalRecipeBook.Infrastructure.Contracts;

namespace NutritionalRecipeBook.Application.UnitTests
{
    public class ReviewServiceUnitTests
    {
        private Mock<IGenericRepository<Review>> _reviewRepositoryMock;

        private Mock<IIdentityService> _identityServiceMock;

        public ReviewServiceUnitTests()
        {
            _reviewRepositoryMock = new(MockBehavior.Strict);
            _identityServiceMock = new();
        }

        [Fact]
        public async Task Should_CreateReview()
        {
            var users = TestData.GetUsers();

            var request = new AddReviewRequest
            {
                UserId = users.FirstOrDefault()!.Id,
                Rating = 5,
                Comment = "Great recipe!",
                RecipeId = Guid.NewGuid()
            };

            _identityServiceMock
                .Setup(service => service.FindUserByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(users.FirstOrDefault());

            _reviewRepositoryMock.Setup(repo => repo.CreateAsync(It.IsAny<Review>())).Returns(Task.CompletedTask);

            var reviewService = new ReviewService(_reviewRepositoryMock.Object, _identityServiceMock.Object);
            var result = await reviewService.CreateAsync(request);

            result.IsSuccess.Should().BeTrue();
        }

        [Fact]
        public async Task Should_ReturnFailureResult_UserNotFound()
        {
            var request = new AddReviewRequest
            {
                UserId = Guid.NewGuid().ToString(),
                Rating = 5,
                Comment = "Great recipe!",
                RecipeId = Guid.NewGuid()
            };

            _identityServiceMock
                .Setup(service => service.FindUserByIdAsync(It.IsAny<string>()))
                .ReturnsAsync((User)null);

            _reviewRepositoryMock.Setup(repo => repo.CreateAsync(It.IsAny<Review>())).Returns(Task.CompletedTask);

            var reviewService = new ReviewService(_reviewRepositoryMock.Object, _identityServiceMock.Object);
            var result = await reviewService.CreateAsync(request);

            result.IsSuccess.Should().BeFalse();
            result.Error.Code.Should().Be("400");
            result.Error.Message.Should().Be("Such user doesn't exist.");
        }
    }
}
