// Model of Mealplan in Atalaya format

namespace codeTestTgx.Models
{
    // I think that with Ave and Acs it should be used inheritance (herencia)

    // Class hotel of mealplan in atalaya format
    public class Ave
    {
        public string room { get; set; }
        public int price { get; set; }
    }

    // Class hotel of mealplan in atalaya format
    public class Acs
    {
        public string room { get; set; }
        public int price { get; set; }
    }

    // Class hotellist of mealplan in atalaya format
    public class Hotel
    {
        public List<Ave> ave { get; set; }
        public List<Acs> acs { get; set; }
    }

    // Class of mealplan in atalaya format
    public class MealPlan
    {
        public string code { get; set; }
        public string name { get; set; }
        public Hotel hotel { get; set; }
    }

    // Class of list mealplans in atalaya format
    public class MealPlanAtalaya
    {
        public List<MealPlan> meal_plans { get; set; }
    }


}
