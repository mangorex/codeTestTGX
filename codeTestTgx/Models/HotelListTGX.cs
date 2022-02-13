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

    public class RoomsTGX
    {
        public string Name { get; set; }
        public Room_Type Room_type { get; set; }
        public List<string> Meals_plan { get; set; }
        public string Price { get; set; }
      
    }

    public class HotelTGX
    {
        public HotelTGX(string Code, string Name, string City)
        {
            this.Code = Code;
            this.Name = Name;
            this.City = City;
            this.Rooms = new List<RoomsTGX>();
        }

        public string Code { get; set; }
        public string Name { get; set; }
        public string City { get; set; }
        public List<RoomsTGX> Rooms { get; set; }
    }

    public class HotelListTGX
    {
        public List<HotelTGX> hotels { get; set; }
    }

}
