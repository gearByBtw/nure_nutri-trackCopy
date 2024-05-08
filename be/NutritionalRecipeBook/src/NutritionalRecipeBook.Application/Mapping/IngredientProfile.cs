using AutoMapper;
using NutritionalRecipeBook.Application.Common.Models;
using NutritionalRecipeBook.Domain.Entities;

namespace NutritionalRecipeBook.Application.Mapping
{
    public class IngredientProfile : Profile
    {
        public IngredientProfile()
        {
            CreateMap<UserIngredient, ShoppingListIngredientModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Ingredient.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Ingredient.Name))
                .ReverseMap();
        }
    }
}
