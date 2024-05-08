using Microsoft.AspNetCore.Identity;
using NutritionalRecipeBook.Application.Common.Models;
using NutritionalRecipeBook.Application.Contracts;
using NutritionalRecipeBook.Domain.Entities;
using NutritionalRecipeBook.Domain.Results;
using NutritionalRecipeBook.Infrastructure.Contracts;

namespace NutritionalRecipeBook.Application.Services
{
    public class ReviewService : IReviewService
    {
        private readonly IGenericRepository<Review> _reviewRepository;

        private readonly IIdentityService _identityService;

        public ReviewService(IGenericRepository<Review> reviewRepository, IIdentityService identityService)
        {
            _reviewRepository = reviewRepository;
            _identityService = identityService;
        }

        public async Task<Result> CreateAsync(AddReviewRequest request)
        {
            var user = await _identityService.FindUserByIdAsync(request.UserId);

            if (user == null)
            {
                return Result.Failure(new Error("400", "Such user doesn't exist."));
            }

            var review = new Review(request.Rating, request.Comment, request.UserId, request.RecipeId, user.UserName);
            await _reviewRepository.CreateAsync(review);

            return Result.Success();
        }
    }
}
