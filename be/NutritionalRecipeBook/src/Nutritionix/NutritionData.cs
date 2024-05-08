using Newtonsoft.Json;

namespace Nutritionix
{
    public class NutritionData
    {
        [JsonProperty("foods")]
        public List<Food> Foods { get; set; }
    }
}
