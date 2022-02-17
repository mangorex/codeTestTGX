namespace codeTestTgx.Models
{
    public class Regimene
    {
        public string code { get; set; }
        public string name { get; set; }
        public string hotel { get; set; }
        public string room_type { get; set; }
        public int price { get; set; }
    }

    public class MealPlansApiResort
    {
        public List<Regimene> regimenes { get; set; }
    }



}
