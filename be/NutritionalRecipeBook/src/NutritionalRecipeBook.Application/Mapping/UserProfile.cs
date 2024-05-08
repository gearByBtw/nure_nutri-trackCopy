using AutoMapper;
using NutritionalRecipeBook.Application.Common.Models.User;
using NutritionalRecipeBook.Domain.Entities;

namespace NutritionalRecipeBook.Application.Mapping
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, GetUserResonse>()
                .ForMember(dest => dest.FavoriteRecipes, opt => opt.MapFrom(src => src.FavoriteRecipes.Select(ri => ri.Recipe)))
                .ReverseMap();
        }
    }
}
