using NutritionalRecipeBook.Application.Common.Models;
using NutritionalRecipeBook.Domain.Results;

namespace NutritionalRecipeBook.Application.Contracts
{
    public interface IReviewService
    {
        public Task<Result> CreateAsync(AddReviewRequest request);
    }
}
