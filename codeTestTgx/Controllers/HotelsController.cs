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

        // GET: api/Hotels
        [HttpGet]
        [Route("/api/hotelList")]
        public async Task<HotelListTGX> GetHotels()
        {
            // Instantiate result variable of hotel list respecting TravelgateX format
            HotelListTGX hotelsTGX = new HotelListTGX();

            await GetHotelsAtalayaAsync(hotelsTGX);
            await GetApiResortAsync(hotelsTGX);

            return hotelsTGX;
        }

        // GET: api/Hotels
        [HttpGet]
        [Route("/api/itineraryCancun")]
        public async Task<HotelListTGX> GetIteneraryCancun()
        {
            HotelListTGX hotelsTGXOrig = new HotelListTGX();
            HttpResponseMessage responseHotels = await client.GetAsync(pathApiHotelsTGX);
            hotelsTGXOrig = await responseHotels.Content.ReadAsAsync<HotelListTGX>();

            string hotelJson = JsonConvert.SerializeObject(hotelsTGXOrig);

            JObject hotelJobj = JObject.Parse(hotelJson);

            List<HotelTGX> hotelsTGX = hotelsTGXOrig.hotels.Where(
                h => h.City.Equals("Malaga") || h.City.Equals("Cancun")).ToList();

            HotelListTGX hotelsTGXResul = new HotelListTGX();
            RoomTGX roomTGX = new RoomTGX();
            foreach (HotelTGX hotelTGX in hotelsTGX)
            {
                HotelTGX hotelTGXResul = new HotelTGX(hotelTGX.Code, hotelTGX.Name, hotelTGX.City);

                foreach (RoomTGX room in hotelTGX.Rooms)
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
                    )
                    {
                        room.Nights = 5;
                        room.Price = (int)(room.Price * room.Nights) * 2;
                        hotelTGXResul.Rooms.Add(room);
                    }
                }

                hotelsTGXResul.hotels.Add(hotelTGXResul);
            }

            return hotelsTGXResul;
        }

        #endregion

        #region AtalayaHotels

        /* Function private asynchronous to obtain data of Atalaya Hotels and save information
        * in TravelgateX format Receive HotelListTGX and return hotelListTGX
        */
        private async Task GetHotelsAtalayaAsync(HotelListTGX hotelsTGX)
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

                // FAIL IS HERE. In this foreach We are calling 2 times to here
                MealPlanAtalaya mealPlanAtalaya =
                    await responseMPAta.Content.ReadAsAsync<MealPlanAtalaya>();

                if (responseMPAta.IsSuccessStatusCode)
                {
                    // Loop atalaya hotels to save the information in hotels list in TravelgateX format
                    foreach (HotelTGX hotel in atalaya.hotels)
                    {
                        HotelTGX hotelTGX = new HotelTGX(hotel.Code, hotel.Name, hotel.City);
                        GetRoomsAtalaya(hotelTGX, hotel, rooms, mealPlanAtalaya);
                        hotelsTGX.hotels.Add(hotelTGX);
                    }

                    // FAIL HERE. Probablemente haya que llamar aqui al meal plan
                }
            }
        }

        // Function private to get Rooms of atalaya
        // This functions calls to GetMealPlansInRooms to get Meal plans and duplicate rooms correctly
        // private async Task<HotelTGX> GetRoomsAtalaya(HotelTGX hotelTGX, HotelTGX hotel, RoomsTypes rooms)
        private void GetRoomsAtalaya(HotelTGX hotelTGX, HotelTGX hotel,
            RoomsTypes rooms, MealPlanAtalaya mealPlanAtalaya)
        {
            // Loop to save roooms in TravelgateX format
            foreach (RoomsType roomType in rooms.rooms_type)
            {
                foreach (String hotelCode in roomType.hotels)
                {
                    if (hotelCode.Equals(hotel.Code))
                    {
                        GetMealPlansInRoomsAtalaya(hotelTGX, hotel, // roomTGX,
                            mealPlanAtalaya, roomType);
                    }
                }

            }
        }

        // This functin get meal plans and duplicate rooms saving code mealplan and price
        private void GetMealPlansInRoomsAtalaya(HotelTGX hotelTGX, HotelTGX hotel, // RoomTGX roomTGX,
            MealPlanAtalaya mealPlanAtalaya, RoomsType roomType)
        {
            RoomTGX roomTGX = null;

            foreach (MealPlan mealPlan in mealPlanAtalaya.meal_plans)
            {
                if (nameof(mealPlan.hotel.acs).Equals(hotel.Code))
                {
                    string codeMealPlan = mealPlan.code;
                    for (int i = 0; i < mealPlan.hotel.acs.Count(); i++)
                    {
                        if (roomType.code.Equals(mealPlan.hotel.acs[i].room))
                        {
                            roomTGX = new RoomTGX(roomType);
                            Acs acs = mealPlan.hotel.acs[i];
                            roomTGX.Price = acs.price;
                            roomTGX.Meal_plan = codeMealPlan;
                            roomTGX.Room_type = acs.room.ToUpper().Equals(
                                Room_Type.Suite.ToString().ToUpper())
                                ? Room_Type.Suite : Room_Type.Standard;
                            hotelTGX.Rooms.Add(roomTGX);
                        }
                    }
                }

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
        *  Receive HotelListTGX and return hotelListTGX
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
                    GetRoomsApiResort(hotelTGX, hotel, mealPlansAR);
                    hotelsTGX.hotels.Add(hotelTGX);
                }
            }

        }

        private void GetRoomsApiResort(HotelTGX hotelTGX, HotelApiResort hotel, MealPlansApiResort mealPlansAR)
        {
            foreach (RoomApiResort roomApiResort in hotel.rooms)
            {
                foreach (Regimene mealPlanAr in mealPlansAR.regimenes)
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
