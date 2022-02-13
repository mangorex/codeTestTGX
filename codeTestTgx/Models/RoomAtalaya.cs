namespace codeTestTgx.Models
{
    // Model to get json of hotelList in RoomApiResort
    public class RoomsType
    {
        public List<string> hotels { get; set; }
        public string code { get; set; }
        public string name { get; set; }
    }

    public class RoomsTypes
    {
        public List<RoomsType> rooms_type { get; set; }
    }
}
