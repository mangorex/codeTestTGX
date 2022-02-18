using Newtonsoft.Json;

// Model HotelListTGX respecting TravelgateX format

namespace codeTestTgx.Models
{

    public enum Room_Type
    {
        Standard,
        Suite
    }

    public enum Meal_plan
    {
        Pc,
        Mp,
        Sa,
        Ad
    }

    // Class of one room in TravelgateX format
    public class RoomTGX
    {
        public string Name { get; set; }
        public Room_Type Room_type { get; set; }
        public string Meal_plan { get; set; }
        public int Price { get; set; }

        // I want to not show nights when nights is null. This is supposed to work, but no :( 
        [JsonProperty("Nights", NullValueHandling = NullValueHandling.Ignore)]
        public int? Nights {  get; set; }

        public RoomTGX()
        {

        }

        public RoomTGX(RoomsType roomType)
        {
            this.Name = roomType.name;
            if (roomType.code.Equals(
                    nameof(Room_Type.Suite).ToString().ToLower())
                )
            {
                this.Room_type = Room_Type.Suite;
            }
            else
            {
                this.Room_type = Room_Type.Standard;
            }

            this.Nights = null;
        }

        public RoomTGX(RoomApiResort roomApiResort)
        {
            this.Name = roomApiResort.name;

            // Distinction of the type of room by the name
            if (roomApiResort.name.ToUpper().Equals(nameof(Room_Type.Suite).ToString().ToUpper()))
            {
                this.Room_type = Room_Type.Suite;
            }
            else
            {
                this.Room_type = Room_Type.Standard;
            }
        }

    }

    // Class of one hotel in TravelgateX format
    public class HotelTGX
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string City { get; set; }
        public List<RoomTGX> Rooms { get; set; }

        public HotelTGX()
        {
            this.Rooms = new List<RoomTGX>();
        }

        public HotelTGX(string Code, string Name, string City)
        {
            this.Code = Code;
            this.Name = Name;
            this.City = City;
            this.Rooms = new List<RoomTGX>();
        }
    }

    // Class of hotel list in TravelgateX format
    public class HotelListTGX
    {
        public List<HotelTGX> Hotels { get; set; }
        public HotelListTGX()
        {
            this.Hotels = new List<HotelTGX>();
        }
    }

}
