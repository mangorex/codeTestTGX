using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace codeTestTgx.Models
{
    public class Rooms
    {
        [Key]
        public string name { get; set; }
        public string room_type { get; set; }
        public List<string> meals_plan { get; set; }
        public string price { get; set; }
    }

    public class Hotel
    {
        [Key]
        public string code { get; set; }
        public string name { get; set; }
        public string city { get; set; }
        public Rooms rooms { get; set; }
    }

    public class Hotels
    {
        [Key]
        public int Id { get; set; }
        public List<Hotel> hotels { get; set; }
    }
}
