using FluentValidation;
using NutritionalRecipeBook.Application.Common.Models;

namespace NutritionalRecipeBook.Api.Validators
{
    public class SearchParamsValidator : AbstractValidator<SearchParams>
    {
        public SearchParamsValidator()
        {
            RuleFor(sp => sp.PageSize)
                .Must(ps => ps <= 30)
                .WithMessage("Max page size is 30");

            RuleFor(sp => sp)
                .Custom((searchParams, context) =>
                {
                    if (searchParams.MinCalories.HasValue && searchParams.MaxCalories.HasValue &&
                        searchParams.MinCalories > searchParams.MaxCalories)
                    {
                        context.AddFailure(nameof(searchParams.MinCalories), "MinCalories cannot be greater than MaxCalories.");
                    }
                });
        }
    }
}