using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace codeTestTgx.Models
{
    // public Codes CodeStatusEnum { get; set; }
    //Could also be
    //public string CodeStatus { get { return CodeStatusEnum.ToString(); } }
    public enum Room_Type
    {
        Standard,
        Suite
    }

    public class Rooms
    {
        public string Name { get; set; }
        // public Room_Type Room_type { get; set; }
        public string Room_type { get; set; }
        public List<string> Meals_plan { get; set; }
        public string Price { get; set; }

        //public static implicit operator Rooms(List<Rooms> v)
        //{
        //    throw new NotImplementedException();
        //}
    }

    public class Hotel
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string City { get; set; }
        public List<Rooms> Rooms { get; set; }
    }

    public class Hotels
    {
        public List<Hotel> hotels { get; set; }

        //public static implicit operator Hotels(List<Hotel> v)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
