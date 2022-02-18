// Model of Mealplan in ApiResort format

namespace codeTestTgx.Models
{
    // Class of Room in ApiResortFormat
    public class RoomApiResort
    {
        public string code { get; set; }
        public string name { get; set; }
    }

    // Class of list Room in Api Resort format
    public class HotelApiResort
    {
        public string code { get; set; }
        public string name { get; set; }
        public string location { get; set; }
        public List<RoomApiResort> rooms { get; set; }
    }

    // Class of hotel lists and Rooms in Api Resort format
    public class RoomsApiResort
    {
        public List<HotelApiResort> hotels { get; set; }
    }
}
