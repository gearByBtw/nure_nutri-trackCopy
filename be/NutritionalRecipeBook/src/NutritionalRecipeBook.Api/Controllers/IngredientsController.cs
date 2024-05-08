using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NutritionalRecipeBook.Application.Contracts;


namespace NutritionalRecipeBook.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class IngredientsController : ControllerBase
    {
        private readonly IIngredientService _ingredientService;

        public IngredientsController(IIngredientService ingredientService)
        {
            _ingredientService = ingredientService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _ingredientService.GetAllAsync();

            return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error.Message);
        }
    }
}
