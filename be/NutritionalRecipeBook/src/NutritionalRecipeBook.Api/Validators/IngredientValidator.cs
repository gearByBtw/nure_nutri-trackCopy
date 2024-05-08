using FluentValidation;
using NutritionalRecipeBook.Domain.Entities;
using System.Text.RegularExpressions;

namespace NutritionalRecipeBook.Api.Validators
{
    public class IngredientValidator : AbstractValidator<Ingredient>
    {
        public IngredientValidator()
        {
            RuleFor(i => i.Name)
                .NotEmpty()
                .WithMessage("{PropertyName} cannot be empty.")
                .Length(2, 128)
                .WithMessage("Length ({TotalLength}) of {PropertyName} is invalid.");
        }
    }
}
