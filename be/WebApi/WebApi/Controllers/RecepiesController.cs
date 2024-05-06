using Microsoft.AspNetCore.Mvc;
using WebApi.Data.Interfaces;
using WebApi.Dto;
using WebApi.Models;

namespace WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class RecepiesController : Controller
{
    private readonly IRepository<Recepie> _recepieRepository;
    private readonly IRepository<Ingredient> _ingredientRepository;

    public RecepiesController(IRepository<Recepie> recepieRepository, IRepository<Ingredient> ingredientRepository)
    {
        _recepieRepository = recepieRepository;
        _ingredientRepository = ingredientRepository;
    }

    [HttpGet]
    public async Task<IActionResult> GetRecepies()
    {
        var recepies = await _recepieRepository.GetListAsync();
        
        return Ok(recepies);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetRecepie(Guid id)
    {
        var recepie = await _recepieRepository.GetByIdAsync(id);
        if (recepie == null)
        {
            return NotFound();
        }
        return Ok(recepie);
    }

    [HttpPost]
    public async Task<IActionResult> AddRecepie(RecepieDto model)
    {
        var recepie = new Recepie()
        {
            CalorieNotes = new List<CalorieNote>(),
            Calories = model.Calories,
            Description = model.Description,
            Ingredients = new List<Ingredient>(),
            IsPremium = model.IsPremium,
            Name = model.Name,
            Votes = model.Votes
        };
        
        await _recepieRepository.AddAsync(recepie);
        
        return CreatedAtAction(nameof(GetRecepie), new { id = recepie.Id }, recepie);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateRecepie(Guid id, Recepie recepie)
    {
        if (id != recepie.Id)
        {
            return BadRequest();
        }
        
        await _recepieRepository.UpdateAsync(id, recepie);
        
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteRecepie(Guid id)
    {
        var recepie = await _recepieRepository.GetByIdAsync(id);
        
        if (recepie == null)
        {
            return NotFound();
        }
        
        await _recepieRepository.DeleteAsync(recepie);
        
        return NoContent();
    }
}