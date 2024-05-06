using WebApi.Models;

namespace WebApi.Dto;

public class CalorieNoteDto
{
    public Guid? UserId { get; set; }
    public int Calorie { get; set; }
    public Guid RecepieId { get; set; }
    public string RecepieName { get; set; }
}