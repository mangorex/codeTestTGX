// Model of Room in Atalaya format

namespace codeTestTgx.Models
{
    // Class of room in Atalaya Format
    public class RoomsType
    {
        public List<string> hotels { get; set; }
        public string code { get; set; }
        public string name { get; set; }
    }

    // Class of list of rooms in Atalaya Format
    public class RoomsTypes
    {
        public List<RoomsType> rooms_type { get; set; }
    }
}
