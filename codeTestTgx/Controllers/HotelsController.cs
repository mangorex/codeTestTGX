#nullable disable
using Microsoft.AspNetCore.Mvc;
using codeTestTgx.Models;
using System.Diagnostics;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace codeTestTgx.Controllers
{
    [Route("api/testtgx")]
    [ApiController]
    public class HotelsController : ControllerBase
    {
        #region Variables
        // Declaration of HttpClient global variable
        static HttpClient client = new HttpClient();
        // Declaration of strings path like a global variable
        static string pathHotelsAtalaya = "http://www.mocky.io/v2/5e4a7e4f2f00005d0097d253";
        static string pathRoomsAtalaya = "https://run.mocky.io/v3/132af02e-8beb-438f-ac6e-a9902bc67036";
        static string pathMPAtalaya = "http://www.mocky.io/v2/5e4a7e282f0000490097d252";
        static string pathHotelsApiResort = "http://www.mocky.io/v2/5e4e43272f00006c0016a52b";
        static string pathRoomMPApiResort = "http://www.mocky.io/v2/5e4a7dd02f0000290097d24b";
        static string pathApiHotelsTGX = "https://localhost:7296/api/hotelList";
        #endregion

        #region Endpoints

        // GET: api/hotelList
        // First endpoint of the test
        // Hotel list
        [HttpGet]
        [Route("/api/hotelList")]
        public async Task<HotelListTGX> GetHotels()
        {
            // Instantiate result variable of hotel list respecting TravelgateX format
            HotelListTGX hotelsTGX = new HotelListTGX();

            // Asynchonous function to obtains hotel and rooms of Atalaya provider
            await GetHotelsAtalayaAsync(hotelsTGX);
            // Asynchonous function to obtains hotel and rooms of ApiResort provider
            await GetApiResortAsync(hotelsTGX);

            return hotelsTGX;
        }

        // GET: api/Hotels
        // Second endpoint
        // Get itinerary cancun
        // This function call first endpoint and return the itinerary Cancun and Malaga
        [HttpGet]
        [Route("/api/itineraryCancun")]
        public async Task<HotelListTGX> GetIteneraryCancun()
        {
            HotelListTGX hotelsTGXOrig = new HotelListTGX();
            HotelListTGX hotelsTGXResul = new HotelListTGX();
            RoomTGX roomTGX = new RoomTGX();

            // Call first endpoint
            HttpResponseMessage responseHotels = await client.GetAsync(pathApiHotelsTGX);
            hotelsTGXOrig = await responseHotels.Content.ReadAsAsync<HotelListTGX>();

            // Serialize hotel json
            string hotelJson = JsonConvert.SerializeObject(hotelsTGXOrig);

            // Filter hotels by Malaga and Cancun cities
            List<HotelTGX> hotelsTGX = hotelsTGXOrig.Hotels.Where(
                h => h.City.Equals("Malaga") || h.City.Equals("Cancun")).ToList();


            foreach ( HotelTGX hotelTGX in hotelsTGX )
            {
                HotelTGX hotelTGXResul = new HotelTGX(hotelTGX.Code, hotelTGX.Name, hotelTGX.City);

                // foreach to filter hotels Malaga with Pension Completa and room standard
                // and filter Cancun with alojamiento y desayuno and Room type standard
                foreach ( RoomTGX room in hotelTGX.Rooms )
                {
                    if(
                        hotelTGX.City.Equals("Malaga") && 
                        room.Meal_plan.Equals("pc") &&
                        room.Room_type.Equals(Room_Type.Standard)
                    ) {
                        room.Nights = 3;
                        room.Price = (int)(room.Price * room.Nights) * 2;
                        hotelTGXResul.Rooms.Add(room);
                    }

                    if (
                        hotelTGX.City.Equals("Cancun") &&
                        room.Meal_plan.Equals("ad") &&
                        room.Room_type.Equals(Room_Type.Standard)
                    ) {
                        room.Nights = 5;
                        room.Price = (int)(room.Price * room.Nights) * 2;
                        hotelTGXResul.Rooms.Add(room);
                    }
                }

                hotelsTGXResul.Hotels.Add(hotelTGXResul);
            }

            return hotelsTGXResul;
        }

        #endregion

        #region AtalayaHotels

        /* Function private asynchronous to obtain data of Atalaya Hotels and save information
        * in TravelgateX format.
        * Receive HotelListTGX hotelsTGX
        */
        private async Task GetHotelsAtalayaAsync( HotelListTGX hotelsTGX )
        {
            // Get of hotels list in Atalaya format
            HttpResponseMessage responseHotels = await client.GetAsync(pathHotelsAtalaya);

            if (responseHotels.IsSuccessStatusCode)
            {
                // Read atalaya hotel list and rooms
                HotelListTGX atalaya = await responseHotels.Content.ReadAsAsync<HotelListTGX>();
                HttpResponseMessage responseRooms = await client.GetAsync(pathRoomsAtalaya);
                RoomsTypes rooms = await responseRooms.Content.ReadAsAsync<RoomsTypes>();
                HttpResponseMessage responseMPAta = await client.GetAsync(pathMPAtalaya);

                MealPlanAtalaya mealPlanAtalaya =
                    await responseMPAta.Content.ReadAsAsync<MealPlanAtalaya>();

                if (responseMPAta.IsSuccessStatusCode)
                {
                    // Loop atalaya hotels to save the information in hotels list in TravelgateX format (hotelsTGX)
                    foreach (HotelTGX hotel in atalaya.Hotels)
                    {
                        HotelTGX hotelTGX = new HotelTGX(hotel.Code, hotel.Name, hotel.City);
                        // Obtain hotel travelgatex hotelTGX
                        GetRoomsAtalaya(hotelTGX, hotel, rooms, mealPlanAtalaya);
                        // Add hotelTGX to hotelsTGX result
                        hotelsTGX.Hotels.Add(hotelTGX);
                    }

                }
            }
        }

        // Function private to get Rooms of atalaya
        // This functions calls to GetMealPlansInRooms to get Meal plans and duplicate rooms correctly
        // private async Task<HotelTGX> GetRoomsAtalaya(HotelTGX hotelTGX, HotelTGX hotel, RoomsTypes rooms)
        private void GetRoomsAtalaya(HotelTGX hotelTGX, HotelTGX hotel,
            RoomsTypes rooms, MealPlanAtalaya mealPlanAtalaya
        ) {
            // Loop to save roooms in TravelgateX format
            foreach (RoomsType roomType in rooms.rooms_type)
            {
                foreach (String hotelCode in roomType.hotels)
                {
                    if (hotelCode.Equals(hotel.Code))
                    {
                        // Get MealPlamns and rooms of atalaya hotels
                        GetMealPlansInRoomsAtalaya(hotelTGX, hotel,
                            mealPlanAtalaya, roomType);
                    }
                }
            }
        }

        // This function get meal plans and duplicate rooms saving
        // code mealplan and price in TravelgateX format
        private void GetMealPlansInRoomsAtalaya( HotelTGX hotelTGX, HotelTGX hotel,
            MealPlanAtalaya mealPlanAtalaya, RoomsType roomType 
        ) {
            RoomTGX roomTGX = null;

            // foreach to get mealplans of atalaya
            foreach (MealPlan mealPlan in mealPlanAtalaya.meal_plans)
            {
                if (nameof(mealPlan.hotel.acs).Equals(hotel.Code))
                {
                    string codeMealPlan = mealPlan.code;

                    // For to get mealplans of hotel acs
                    for (int i = 0; i < mealPlan.hotel.acs.Count(); i++)
                    {
                        if (roomType.code.Equals(mealPlan.hotel.acs[i].room))
                        {
                            // Instantiate room tgx
                            roomTGX = new RoomTGX(roomType);
                            // Save mealplans in TravelgateX format. In the roomTGX
                            Acs acs = mealPlan.hotel.acs[i];
                            roomTGX.Price = acs.price;
                            roomTGX.Meal_plan = codeMealPlan;
                            // Save type of room
                            roomTGX.Room_type = acs.room.ToUpper().Equals(
                                Room_Type.Suite.ToString().ToUpper())
                                ? Room_Type.Suite : Room_Type.Standard;

                            // Add room in hotelTGX. Each mealplan create a copy of the rooom
                            hotelTGX.Rooms.Add(roomTGX);
                        }
                    }
                }

                // The same as with acs, but with hotel ave. I think it showld be refactored, but I do not have enough time 
                else if (nameof(mealPlan.hotel.ave).Equals(hotel.Code) &&
                    roomType.code.Equals(mealPlan.hotel.ave.FirstOrDefault().room) 
                ) {
                    roomTGX = new RoomTGX(roomType);
                    Ave ave = (Ave)mealPlan.hotel.ave.FirstOrDefault();
                    roomTGX.Price = ave.price;
                    roomTGX.Meal_plan = mealPlan.code;
                    roomTGX.Room_type = ave.room.Equals(Room_Type.Suite) ? Room_Type.Suite : Room_Type.Standard;
                    hotelTGX.Rooms.Add(roomTGX);
                }

            }
        }

        #endregion

        #region ApiResort

        /* Function private asynchronous to obtain data of Api Resort Hotels and save information in TravelgateX format
        *  Receive HotelListTGX
        */
        private async Task GetApiResortAsync(HotelListTGX hotelsTGX)
        {
            // Get of hotels list in API Resort format
            HttpResponseMessage responseHotels = await client.GetAsync(pathHotelsApiResort);
            HttpResponseMessage responseRoomMPApiResort = await client.GetAsync(pathRoomMPApiResort);

            if (responseHotels.IsSuccessStatusCode && responseRoomMPApiResort.IsSuccessStatusCode)
            {
                RoomsApiResort apiResort = await responseHotels.Content.ReadAsAsync<RoomsApiResort>();
                MealPlansApiResort mealPlansAR = await responseRoomMPApiResort.Content.ReadAsAsync<MealPlansApiResort>();

                // Iteration to save room information in TravelgateX format
                foreach (HotelApiResort hotel in apiResort.hotels)
                {
                    HotelTGX hotelTGX = new HotelTGX(hotel.code, hotel.name, hotel.location);
                    // Get rooms and meal plan of ApiResort and add hotel to hotelsTGX
                    GetRoomsApiResort(hotelTGX, hotel, mealPlansAR);
                    hotelsTGX.Hotels.Add(hotelTGX);
                }
            }

        }

        /* Function to get Rooms and mealplans of Api Resorts
         * Receive hotelTGX (hotel object in TravelgateX format), 
         * hotel (hotel in format of ApiResort) 
         * and mealPlansAR( mealplans in format Api Resort 
        */
        private void GetRoomsApiResort(HotelTGX hotelTGX, HotelApiResort hotel, MealPlansApiResort mealPlansAR)
        {
            foreach (RoomApiResort roomApiResort in hotel.rooms)
            {
                foreach (MPApiResort mealPlanAr in mealPlansAR.regimenes)
                {
                    if (hotel.code.Equals(mealPlanAr.hotel) &&
                        roomApiResort.code.Equals(mealPlanAr.room_type)
                    )
                    {
                        RoomTGX roomTGX = new RoomTGX(roomApiResort);
                        roomTGX.Price = mealPlanAr.price;
                        roomTGX.Meal_plan = mealPlanAr.code;
                        roomTGX.Room_type = mealPlanAr.room_type.Equals("su")
                            ? Room_Type.Suite : Room_Type.Standard;
                        hotelTGX.Rooms.Add(roomTGX);
                    }
                }
            }
        }

        #endregion

    }
}
