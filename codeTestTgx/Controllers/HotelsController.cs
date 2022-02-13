#nullable disable
using Microsoft.AspNetCore.Mvc;
using codeTestTgx.Models;


namespace codeTestTgx.Controllers
{
    [Route("api/testtgx")]
    [ApiController]
    public class HotelsController : ControllerBase
    {
        // Declaration of HttpClient global variable
        static HttpClient client = new HttpClient();
        // Declaration of strings path like a global variable
        static string pathHotelsAtalaya = "http://www.mocky.io/v2/5e4a7e4f2f00005d0097d253";
        static string pathRoomsAtalaya = "https://run.mocky.io/v3/132af02e-8beb-438f-ac6e-a9902bc67036";
        static string pathMealPlanAtalaya = "http://www.mocky.io/v2/5e4a7e282f0000490097d252";
        static string pathHotelsApiResort = "http://www.mocky.io/v2/5e4e43272f00006c0016a52b";

        // GET: api/Hotels
        [HttpGet]
        [Route("/api/hotelList")]
        public async Task<HotelListTGX> GetHotels()
        {
            // Instantiate result variable of hotel list respecting TravelgateX format
            HotelListTGX hotelsTGX = new HotelListTGX();

            hotelsTGX = await GetHotelsAtalayaAsync(hotelsTGX);
            hotelsTGX = await GetApiResortAsync(hotelsTGX);

            return hotelsTGX;
        }

        /* In the functions below I decided to use parameters and return of the 
        * same parameter because using ref is not allowed in asynchronous functions
        * It is asynchronous function because I want to use ReadAsAsync 
        */

        #region AtalayaHotels
        /* Function private asynchronous to obtain data of Atalaya Hotels and save information
        * in TravelgateX format Receive HotelListTGX and return hotelListTGX
        */
        private async Task<HotelListTGX> GetHotelsAtalayaAsync(HotelListTGX hotelsTGX)
        {
            // Get of hotels list in Atalaya format
            HttpResponseMessage responseHotels = await client.GetAsync(pathHotelsAtalaya);

            if (responseHotels.IsSuccessStatusCode)
            {
                // Read atalaya hotel list and rooms
                HotelListTGX atalaya = await responseHotels.Content.ReadAsAsync<HotelListTGX>();
                HttpResponseMessage responseRooms = await client.GetAsync(pathRoomsAtalaya);
                RoomsTypes rooms = await responseRooms.Content.ReadAsAsync<RoomsTypes>();

                // Loop atalaya hotels to save the information in hotels list in TravelgateX format
                foreach (HotelTGX hotel in atalaya.hotels)
                {
                    HotelTGX hotelTGX = new HotelTGX(hotel.Code, hotel.Name, hotel.City);
                    hotelTGX = await GetRoomsAtalaya(hotelTGX, hotel, rooms);
                    hotelsTGX.hotels.Add(hotelTGX);
                }

            }

            return hotelsTGX;
        }

        private async Task<HotelTGX> GetRoomsAtalaya(HotelTGX hotelTGX, HotelTGX hotel, RoomsTypes rooms)
        {
            // Loop to save roooms in TravelgateX format
            foreach (RoomsType roomType in rooms.rooms_type)
            {
                foreach (String hotelCode in roomType.hotels)
                {
                    RoomsTGX roomTGX = new RoomsTGX();

                    if (hotelCode.Equals(hotel.Code))
                    {
                        roomTGX.Name = roomType.name;
                        if (roomType.code.Equals(nameof(Room_Type.Suite).ToString().ToLower()))
                        {
                            roomTGX.Room_type = Room_Type.Suite;
                        }
                        else
                        {
                            roomTGX.Room_type = Room_Type.Standard;
                        }
                        hotelTGX.Rooms.Add(roomTGX);
                    }
                }
            }
            return hotelTGX;
        }

        #endregion

        #region ApiResort
        /* Function private asynchronous to obtain data of Api Resort Hotels and save information in TravelgateX format
        *  Receive HotelListTGX and return hotelListTGX
        */
        private async Task<HotelListTGX> GetApiResortAsync(HotelListTGX hotelsTGX)
        {
            // Get of hotels list in API Resort format
            HttpResponseMessage responseHotels = await client.GetAsync(pathHotelsApiResort);
            if (responseHotels.IsSuccessStatusCode)
            {
                RoomsApiResort apiResort = await responseHotels.Content.ReadAsAsync<RoomsApiResort>();

                // Iteration to save room information in TravelgateX format
                foreach (HotelApiResort hotel in apiResort.hotels)
                {
                    HotelTGX hotelTGX = new HotelTGX(hotel.code, hotel.name, hotel.location);
                    hotelTGX = await GetRoomsApiResort(hotelTGX, hotel);
                    hotelsTGX.hotels.Add(hotelTGX);
                }
            }

            return hotelsTGX;
        }

        private async Task<HotelTGX> GetRoomsApiResort(HotelTGX hotelTGX, HotelApiResort hotel)
        {
            foreach (RoomApiResort roomApiResort in hotel.rooms)
            {
                RoomsTGX roomTGX = new RoomsTGX();
                roomTGX.Name = roomApiResort.name;

                // Distinction of the type of room by the name
                if (roomApiResort.name.ToUpper().Equals(nameof(Room_Type.Suite).ToString().ToUpper()))
                {
                    roomTGX.Room_type = Room_Type.Suite;
                }
                else
                {
                    roomTGX.Room_type = Room_Type.Standard;
                }
                hotelTGX.Rooms.Add(roomTGX);
            }

            return hotelTGX;
        }
        #endregion

    }
}
