# codeTestTGX
## AUTHOR: Manuel Antonio Gómez Angulo
## START DATE: 12/02/2022
## END DATE: 18/02/2022

It is about a development in C# with .net 6 of a web api to do integrations hotels 

### Architecture and technologies used
* **Windows 10**
* **Net core 6**
* Visual studio 2022 community Edition
* Newtonsoft.Json

### Points completed
* **Code in english with so love and not so bad style of code (I think)**
* Web api with two endpoints
* First endpoint is hotels list: https://localhost:7296/api/hotelList
    Return hotel list in TravelgateX format
    Result in Results/endpoint1.json
    
* Second endpoint is itinerary Cancun and Malaga cities: https://localhost:7296/api/itineraryCancun
    Return itinerary Cancun and Malaga in TravelgateX format
    Result in Results/endpoint2.json
    
### Things to improve:
* **Code with the best style of code (I think)**
* Use of inheritance in MealPlanAtalaya.cs with Acs and Ave. I think
* A better refactoring in GetMealPlansInRoomsAtalaya with Acs and Ave hotels
* Quit nights of endpoint 1. I wrote
       [JsonProperty("Nights", NullValueHandling = NullValueHandling.Ignore)]
        public int? Nights {  get; set; }
    to avoid impression of nights with null value, but did not work. Bad for me :( 
* Fix [JsonConverter(typeof(StringEnumConverter))] in Room_Type enum to print key like a string (Standard or Suite)
* Put Enum in RoomTGX in Mealplan

So **many thanks to the people** who have given me the test **to prove myself**
