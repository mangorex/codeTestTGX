namespace codeTestTgx.Models
{
    // Model HotelListTGX respecting TravelgateX format

    // public Codes CodeStatusEnum { get; set; }
    //Could also be
    //public string CodeStatus { get { return CodeStatusEnum.ToString(); } }
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

    public class RoomTGX
    {
        public string Name { get; set; }
        public Room_Type Room_type { get; set; }
        public string Meals_plan { get; set; }
        public int Price { get; set; }

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

    public class HotelTGX
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string City { get; set; }
        public List<RoomTGX> Rooms { get; set; }

        public HotelTGX(string Code, string Name, string City)
        {
            this.Code = Code;
            this.Name = Name;
            this.City = City;
            this.Rooms = new List<RoomTGX>();
        }
    }

    public class HotelListTGX
    {
        public List<HotelTGX> hotels { get; set; }
        public HotelListTGX()
        {
            this.hotels = new List<HotelTGX>();
        }
    }

}
