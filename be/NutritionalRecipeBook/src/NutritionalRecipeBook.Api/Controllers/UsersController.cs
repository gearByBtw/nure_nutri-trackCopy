using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NutritionalRecipeBook.Application.Common.Models.User;
using NutritionalRecipeBook.Application.Contracts;

namespace NutritionalRecipeBook.Api.Controllers
{
    [Authorize]
    [Route("[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var result = await _userService.GetById(id);

            return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error.Message);
        }

        [HttpPost("add-favorite-recipe")]
        public async Task<IActionResult> AddFavoriteRecipe([FromBody] AddFavoriteRecipeRequest request)
        {
            var result = await _userService.AddFavoriteRecipe(request);

            return result.IsSuccess ? Ok("Succesfully added") : BadRequest(result.Error.Message);
        }

        [HttpPost("add-recipe-ingredients-to-shopping-list")]
        public async Task<IActionResult> AddRecipeIngredientsToShopList([FromBody] AddRecipeIngredientsToShopListRequest request)
        {
            var result = await _userService.AddRecipeIngredientsToShopList(request);

            return result.IsSuccess ? Ok("List succesfully updated") : BadRequest(result.Error.Message);
        }

        [HttpPost("update-shopping-list")]
        public async Task<IActionResult> UpdateShopList([FromBody] UpdateShopListRequest request)
        {
            var result = await _userService.UpdateShopList(request);

            return result.IsSuccess ? Ok("List succesfully updated") : BadRequest(result.Error.Message);
        }

        [HttpDelete("delete-favorite-recipe")]
        public async Task<IActionResult> DeleteFavoriteRecipe([FromQuery] DeleteFavoriteRecipeRequest request)
        {
            var result = await _userService.DeleteFavoriteRecipe(request);

            return result.IsSuccess ? Ok("Succesfully deleted") : BadRequest(result.Error.Message);
        }
    }
}
