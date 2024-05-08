

namespace Nutritionix
{
    public interface INutritionixClient
    {
        public Task<NutritionData?> GetNutritionData(string query);
    }
}
