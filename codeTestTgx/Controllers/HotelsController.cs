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
        static HttpClient client = new HttpClient();
        static string pathHotelsAtalaya = "http://www.mocky.io/v2/5e4a7e4f2f00005d0097d253";
        static string pathRooms = "https://run.mocky.io/v3/132af02e-8beb-438f-ac6e-a9902bc67036";
        static string pathHotelsApiResort = "http://www.mocky.io/v2/5e4e43272f00006c0016a52b";

        /*private readonly TodoContext _context;
        public HotelsController(TodoContext context)
        {
            _context = context;
        }*/

        // GET: api/Hotels
        [HttpGet]
        [Route("/api/hotelList")]
        public async Task<Hotels> GetHotels()
        {
            Hotels hotelsResul = new Hotels();
            hotelsResul.hotels = new List<Hotel>();
            Rooms roomResul = new Rooms();

            HttpResponseMessage responseHotels = await client.GetAsync(pathHotelsAtalaya);
            if (responseHotels.IsSuccessStatusCode)
            {
                Hotels atalaya = await responseHotels.Content.ReadAsAsync<Hotels>();

                HttpResponseMessage responseRooms = await client.GetAsync(pathRooms);
                RoomsTypes rooms = await responseRooms.Content.ReadAsAsync<RoomsTypes>();

                // foreach (HotelAt hotel in atalaya.hotels)
                foreach (Hotel hotel in atalaya.hotels)
                {
                    Hotel hotelResul = new Hotel();
                    hotelResul.Rooms = new List<Rooms>();
                    hotelResul.Code = hotel.Code;
                    hotelResul.Name = hotel.Name;
                    hotelResul.City = hotel.City;
                    
                    foreach (RoomsType roomType in rooms.rooms_type)
                    {
                        foreach(String hotelCode in roomType.hotels)
                        { 
                            if(hotelCode.Equals(hotel.Code))
                            {
                                roomResul.Name = roomType.name;
                                hotelResul.Rooms.Add(roomResul);
                            }
                        }
                    }
                    hotelsResul.hotels.Add(hotelResul);
                }

            }
            return hotelsResul;
        }

       /*
       // GET api/<HotelsController>/5
       [HttpGet("{id}")]
       public async Task<ActionResult<Hotels>> GetHotel(long id)
       {
           var hotelsItems = await _context.hotelsItems.FindAsync(id);

           if (hotelsItems == null)
           {
               return NotFound();
           }

           return hotelsItems;
       }

       // POST api/<HotelsController>
      [HttpPost]
       public async Task<ActionResult<Hotels>> PostTodoItem(Hotels hotels)
       {
           _context.hotelsItems.Add(hotels);
           //await _context.SaveChangesAsync();

           return CreatedAtAction(nameof(GetHotel), new { id = hotels.Id }, hotels);
       }

       // PUT api/<HotelsController>/5
       [HttpPut("{id}")]
       public async Task<IActionResult> PutHotel(long id, Hotels hotels)
       {
           if (id != hotels.Id)
           {
               return BadRequest();
           }

           _context.Entry(hotels).State = EntityState.Modified;

           try
           {
               await _context.SaveChangesAsync();
           }
           catch (DbUpdateConcurrencyException)
           {
               if (!HotelsExists(id))
               {
                   return NotFound();
               }
               else
               {
                   throw;
               }
           }

           return NoContent();
       }

       // DELETE api/<HotelsController>/5
       [HttpDelete("{id}")]
       public async Task<IActionResult> DeleteHotels(long id)
       {
           var hotel = await _context.hotelsItems.FindAsync(id);
           if (hotel == null)
           {
               return NotFound();
           }

           _context.hotelsItems.Remove(hotel);
           await _context.SaveChangesAsync();

           return NoContent();
       }*/

        /*private bool HotelsExists(long id)
        {
            return _context.hotelsItems.Any(e => e.Id == id);
        }*/
    }
}
