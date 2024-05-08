using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NutritionalRecipeBook.Application.Common.Models;
using NutritionalRecipeBook.Application.Common.Models.Recipe;
using NutritionalRecipeBook.Application.Contracts;


namespace NutritionalRecipeBook.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class RecipesController : ControllerBase
    {
        private readonly IRecipeService _recipeService;

        private readonly IValidator<CreateRecipeRequest> _createRecipeValidator;

        private readonly IValidator<UpdateRecipeRequest> _updateRecipeValidator;

        private readonly IValidator<SearchParams> _searchParamsValidator;

        public RecipesController(IRecipeService recipeService,
            IValidator<UpdateRecipeRequest> updateRecipeValidator,
            IValidator<CreateRecipeRequest> createRecipeValidator,
            IValidator<SearchParams> searchParamsValidator)
        {
            _recipeService = recipeService;
            _createRecipeValidator = createRecipeValidator;
            _updateRecipeValidator = updateRecipeValidator;
            _searchParamsValidator = searchParamsValidator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] SearchParams parameters)
        {
            var validation = _searchParamsValidator.Validate(parameters);

            if (!validation.IsValid)
            {
                return BadRequest(validation.Errors);
            }

            var result = await _recipeService.GetAllAsync(parameters);

            if (result.IsSuccess)
            {
                var recipes = result.Value;

                var metadata = new
                {
                    recipes.TotalCount,
                    recipes.PageSize,
                    recipes.CurrentPage,
                    recipes.TotalPages,
                    recipes.HasNext,
                    recipes.HasPrevious
                };

                Response.Headers.Append("X-Pagination", JsonConvert.SerializeObject(metadata));
                Response.Headers.Append("Access-Control-Expose-Headers", "X-Pagination");
            }

            return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error.Message);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _recipeService.GetByIdWithRelationsAsync(id);

            return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error.Message);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateRecipeRequest request)
        {
            var validation = _createRecipeValidator.Validate(request);

            if (!validation.IsValid)
            {
                return BadRequest(validation.Errors);
            }

            var result = await _recipeService.CreateAsync(request);

            return result.IsSuccess ? Ok("Succesfully created") : BadRequest(result.Error.Message);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateRecipeRequest request)
        {
            var validation = _updateRecipeValidator.Validate(request);

            if (!validation.IsValid)
            {
                return BadRequest(validation.Errors);
            }

            var result = await _recipeService.UpdateAsync(request);

            return result.IsSuccess ? Ok("Succesfully updated") : BadRequest(result.Error.Message);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id, [FromQuery] string userId)
        {
            var result = await _recipeService.DeleteByIdAsync(id, userId);

            return result.IsSuccess ? Ok($"Recipe with id ({id}) deleted") : BadRequest(result.Error.Message);
        }
    }
}
