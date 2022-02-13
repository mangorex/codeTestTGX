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

                // Loop atalaya hotels to save the information in hotels list in TravelgateX format
                foreach (HotelTGX hotel in atalaya.hotels)
                {
                    HotelTGX hotelTGX = new HotelTGX(hotel.Code, hotel.Name, hotel.City);
                    await GetRoomsAtalaya(hotelTGX, hotel, rooms);
                    hotelsTGX.hotels.Add(hotelTGX);
                }
            }
        }

        // Function private to get Rooms of atalaya
        // This functions calls to GetMealPlansInRooms to get Meal plans and duplicate rooms correctly
        // private async Task<HotelTGX> GetRoomsAtalaya(HotelTGX hotelTGX, HotelTGX hotel, RoomsTypes rooms)
        private async Task GetRoomsAtalaya(HotelTGX hotelTGX, HotelTGX hotel, RoomsTypes rooms)
        {
            // Loop to save roooms in TravelgateX format
            foreach (RoomsType roomType in rooms.rooms_type)
            {
                foreach (String hotelCode in roomType.hotels)
                {
                    RoomTGX roomTGX = new RoomTGX(roomType);

                    if (hotelCode.Equals(hotel.Code))
                    {
                        HttpResponseMessage responseMealPlan =
                            await client.GetAsync(pathMealPlanAtalaya);

                        if (responseMealPlan.IsSuccessStatusCode)
                        {
                            MealPlanAtalaya mealPlanAtalaya = await
                            responseMealPlan.Content.ReadAsAsync<MealPlanAtalaya>();
                            GetMealPlansInRooms(hotelTGX, hotel, roomTGX,
                                mealPlanAtalaya, roomType);
                        }
                    }
                }
            }
        }

        // This functin get meal plans and duplicate rooms saving code mealplan and price
        // private async Task<HotelTGX> GetMealPlansInRooms( HotelTGX hotelTGX, HotelTGX hotel, RoomTGX roomTGX, 
        private void GetMealPlansInRooms(HotelTGX hotelTGX, HotelTGX hotel, RoomTGX roomTGX,
            MealPlanAtalaya mealPlanAtalaya, RoomsType roomType)
        {
            foreach (MealPlan mealPlan in mealPlanAtalaya.meal_plans)
            {
                if (nameof(mealPlan.hotel.acs).Equals(hotel.Code))
                {
                    Hotel hotelMealPlan = mealPlan.hotel;
                    Acs acs = (Acs)hotelMealPlan.acs.FirstOrDefault();
                    roomTGX.Price = acs.price;
                    roomTGX.Meals_plan = mealPlan.code;
                    hotelTGX.Rooms.Add(roomTGX);

                    for (int i = 1; i < hotelMealPlan.acs.Count; i++)
                    {
                        roomTGX = new RoomTGX(roomType);
                        acs = hotelMealPlan.acs[i];
                        roomTGX.Price = acs.price;
                        roomTGX.Meals_plan = mealPlan.code;
                        hotelTGX.Rooms.Add(roomTGX);
                    }

                    roomTGX = new RoomTGX(roomType);
                }

                else if (nameof(mealPlan.hotel.ave).Equals(hotel.Code))
                {
                    Hotel hotelMealPlan = mealPlan.hotel;
                    Ave ave = (Ave)hotelMealPlan.ave.FirstOrDefault();
                    roomTGX.Price = ave.price;
                    roomTGX.Meals_plan = mealPlan.code;
                    hotelTGX.Rooms.Add(roomTGX);

                    for (int i = 1; i < hotelMealPlan.ave.Count; i++)
                    {
                        roomTGX = new RoomTGX(roomType);
                        ave = hotelMealPlan.ave[i];
                        roomTGX.Price = ave.price;
                        roomTGX.Meals_plan = mealPlan.code;
                        hotelTGX.Rooms.Add(roomTGX);
                    }

                    roomTGX = new RoomTGX(roomType);
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
            if (responseHotels.IsSuccessStatusCode)
            {
                RoomsApiResort apiResort = await responseHotels.Content.ReadAsAsync<RoomsApiResort>();

                // Iteration to save room information in TravelgateX format
                foreach (HotelApiResort hotel in apiResort.hotels)
                {
                    HotelTGX hotelTGX = new HotelTGX(hotel.code, hotel.name, hotel.location);
                    GetRoomsApiResort(hotelTGX, hotel);
                    hotelsTGX.hotels.Add(hotelTGX);
                }
            }

        }

        private void GetRoomsApiResort(HotelTGX hotelTGX, HotelApiResort hotel)
        {
            foreach (RoomApiResort roomApiResort in hotel.rooms)
            {
                RoomTGX roomTGX = new RoomTGX(roomApiResort);
                hotelTGX.Rooms.Add(roomTGX);
            }
        }

        #endregion

    }
}
