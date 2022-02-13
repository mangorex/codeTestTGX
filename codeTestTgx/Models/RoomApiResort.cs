namespace codeTestTgx.Models
{
    // Model to get json of hotelList in RoomApiResort
    public class RoomApiResort
    {
        public string code { get; set; }
        public string name { get; set; }
    }

    public class HotelApiResort
    {
        public string code { get; set; }
        public string name { get; set; }
        public string location { get; set; }
        public List<RoomApiResort> rooms { get; set; }
    }

    public class RoomsApiResort
    {
        public List<HotelApiResort> hotels { get; set; }
    }
}
