// Model of Mealplan in ApiResort format

namespace codeTestTgx.Models
{
    // Class of mealplan in Api Resort format
    public class MPApiResort
    {
        public string code { get; set; }
        public string name { get; set; }
        public string hotel { get; set; }
        public string room_type { get; set; }
        public int price { get; set; }
    }

    // Class of list mealplans in Api Resort format
    public class MealPlansApiResort
    {
        public List<MPApiResort> regimenes { get; set; }
    }



}
