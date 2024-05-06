using WebApi.Models;

namespace WebApi.Dto;

public class RecepieDto
{
    public string Name { get; set; }
    public int Calories { get; set; }
    public string Description { get; set; }
    public string Votes { get; set; }
    public bool IsPremium { get; set; }
}