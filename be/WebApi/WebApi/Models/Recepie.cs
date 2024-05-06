namespace WebApi.Models;

public class Recepie : BaseModel
{
    public string Name { get; set; }
    public virtual List<Ingredient> Ingredients { get; set; }
    public int Calories { get; set; }
    public string Description { get; set; }
    public string Votes { get; set; }
    public bool IsPremium { get; set; }

    public virtual ICollection<CalorieNote> CalorieNotes { get; set; }
}