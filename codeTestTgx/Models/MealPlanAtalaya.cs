namespace codeTestTgx.Models
{
    public class Ave
    {
        public string room { get; set; }
        public int price { get; set; }
    }

    public class Acs
    {
        public string room { get; set; }
        public int price { get; set; }
    }

    public class Hotel
    {
        public List<Ave> ave { get; set; }
        public List<Acs> acs { get; set; }
    }

    public class MealPlan
    {
        public string code { get; set; }
        public string name { get; set; }
        public Hotel hotel { get; set; }
    }

    public class MealPlanAtalaya
    {
        public List<MealPlan> meal_plans { get; set; }
    }


}
