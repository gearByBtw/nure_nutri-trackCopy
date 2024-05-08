using AutoMapper;
using NutritionalRecipeBook.Application.Common.Models.Recipe;
using NutritionalRecipeBook.Domain.Entities;

namespace NutritionalRecipeBook.Application.Mapping
{
    public class RecipeProfile : Profile
    {
        public RecipeProfile()
        {
            CreateMap<Recipe, GetRecipeResponse>()
                .ForMember(dest => dest.Ingredients, opt => opt.MapFrom(src => src.Ingredients.Select(ri => ri.Ingredient)))
                .ReverseMap();
        }
    }
}