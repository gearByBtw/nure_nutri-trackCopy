using Microsoft.AspNetCore.Mvc;
using WebApi.Data.Interfaces;
using WebApi.Dto;
using WebApi.Models;

namespace WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class CalorieNotesController : Controller
{
    private readonly IRepository<CalorieNote> _calorieNoteRepository;
    private readonly IRepository<Recepie> _recepieRepository;

    public CalorieNotesController(IRepository<CalorieNote> calorieNoteRepository, IRepository<Recepie> _recepieRepository)
    {
        _calorieNoteRepository = calorieNoteRepository;
        this._recepieRepository = _recepieRepository;
    }

    [HttpGet]
    public async Task<IActionResult> GetCalorieNotes()
    {
        var calorieNotes = await _calorieNoteRepository.GetListAsync();
        return Ok(calorieNotes);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetCalorieNote(Guid id)
    {
        var calorieNote = await _calorieNoteRepository.GetByIdAsync(id);
        if (calorieNote == null)
        {
            return NotFound();
        }
        return Ok(calorieNote);
    }

    [HttpPost]
    public async Task<IActionResult> AddCalorieNote(CalorieNoteDto model)
    {
        var recepie = await _recepieRepository.GetByIdAsync(model.RecepieId);
        if (recepie == null)
        {
            return NotFound("Recepie not found");
        }
        
        var calorieNote = new CalorieNote()
        {
            Calorie = model.Calorie,
            CreatedAt = DateTime.Now,
            RecepieId = Guid.NewGuid(),
            RecepieName = recepie.Name,
            UserId = model.UserId
        };

        recepie.CalorieNotes ??= new List<CalorieNote>();
        recepie.CalorieNotes.Add(calorieNote);
        
        
        await _recepieRepository.UpdateAsync(model.RecepieId, recepie);
        
        return CreatedAtAction(nameof(GetCalorieNote), new { id = calorieNote.Id }, calorieNote);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateCalorieNote(Guid id, CalorieNote calorieNote)
    {
        await _calorieNoteRepository.UpdateAsync(id, calorieNote);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCalorieNote(Guid id)
    {
        var calorieNote = await _calorieNoteRepository.GetByIdAsync(id);
        if (calorieNote == null)
        {
            return NotFound();
        }
        await _calorieNoteRepository.DeleteAsync(calorieNote);
        return NoContent();
    }
}