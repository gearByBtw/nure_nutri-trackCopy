namespace WebApi.Models;

public class CalorieNote : BaseModel
{
    public Guid? UserId { get; set; }
    public DateTime CreatedAt { get; set; }
    public int Calorie { get; set; }
    public Guid? RecepieId { get; set; }
    public string RecepieName { get; set; }

    public virtual Recepie Recepie { get; set; }
}