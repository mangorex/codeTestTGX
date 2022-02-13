#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using codeTestTgx.Models;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

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
        static string pathHotelsApiResort = "http://www.mocky.io/v2/5e4e43272f00006c0016a52b";

        // GET: api/Hotels
        [HttpGet]
        [Route("/api/hotelList")]
        public async Task<HotelListTGX> GetHotels()
        {
            // Instantiate result variable of hotel list respecting TravelgateX format
            HotelListTGX hotelsTGX = new HotelListTGX();
            hotelsTGX.hotels = new List<HotelTGX>();
            RoomsTGX roomTGX = new RoomsTGX();

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
                    // Loop to save roooms in TravelgateX format
                    foreach (RoomsType roomType in rooms.rooms_type)
                    {
                        foreach(String hotelCode in roomType.hotels)
                        { 
                            if(hotelCode.Equals(hotel.Code))
                            {
                                roomTGX.Name = roomType.name;
                                hotelTGX.Rooms.Add(roomTGX);
                            }
                        }
                    }
                    hotelsTGX.hotels.Add(hotelTGX);
                }
            }

            // Get of hotels list in API Resort format
            HttpResponseMessage responseHotels2 = await client.GetAsync(pathHotelsApiResort);
            if (responseHotels2.IsSuccessStatusCode)
            {
                RoomsApiResort apiResort = await responseHotels2.Content.ReadAsAsync<RoomsApiResort>();

                // Iteration to save room information in TravelgateX format
                foreach (HotelApiResort hotel in apiResort.hotels)
                {
                    HotelTGX hotelTGX = new HotelTGX(hotel.code, hotel.name, hotel.location);

                    foreach (RoomApiResort roomApiResort in hotel.rooms)
                    {
                        roomTGX.Name = roomApiResort.name;
                        hotelTGX.Rooms.Add(roomTGX);
                    }
                    hotelsTGX.hotels.Add(hotelTGX);
                }
            }

            return hotelsTGX;
        }

    }
}
