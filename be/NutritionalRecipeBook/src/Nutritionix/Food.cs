using Newtonsoft.Json;

namespace Nutritionix
{
    public class Food
    {
        [JsonProperty("food_name")]
        public string Name { get; set; }

        [JsonProperty("nf_calories")]
        public double Calories { get; set; }
    }
}
