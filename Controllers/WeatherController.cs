using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Nancy.Json;
using Newtonsoft.Json;
using Weather_Deatils.Models;

namespace Weather_Deatils.Controllers
{
   
    public class WeatherController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly string _apiKey = "e925a4bd77f92d591f34b720ca5bf6e9";

        public WeatherController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        // GET: /Weather/Search
        public IActionResult Search()
        {
            ViewBag.IsSearched = false;
            return View();
        }

        // POST: /Weather/Search
        [HttpPost]
        public async Task<IActionResult> Search(string city,string userid)
        {
          
            ViewBag.userid = userid;
            ViewBag.IsSearched = false;
            if (!string.IsNullOrEmpty(city))
            {
                // Make request to OpenWeatherMap API
                var client = _httpClientFactory.CreateClient();
                var response = await client.GetAsync($"https://api.openweathermap.org/data/2.5/weather?q={city}&appid={_apiKey}&units=metric");

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var weatherData = JsonConvert.DeserializeObject<object>(json);
                    RootObject weatherInfo = (new JavaScriptSerializer()).Deserialize<RootObject>(json);
                    ResultViewModel rslt = new ResultViewModel();
                    
                    rslt.Country = weatherInfo.sys.country;
                    rslt.City = weatherInfo.name;
                    rslt.Lat = Convert.ToString(weatherInfo.coord.lat);
                    rslt.Lon = Convert.ToString(weatherInfo.coord.lon);
                    rslt.Description = weatherInfo.weather[0].description;
                    rslt.Humidity = Convert.ToString(weatherInfo.main.humidity);
                    rslt.Temp = Convert.ToString(weatherInfo.main.temp);
                    rslt.TempFeelsLike = Convert.ToString(weatherInfo.main.feels_like);
                    rslt.TempMax = Convert.ToString(weatherInfo.main.temp_max);
                    rslt.TempMin = Convert.ToString(weatherInfo.main.temp_min);
                    rslt.wind = Convert.ToString(weatherInfo.wind.speed);
                    rslt.WeatherIcon = weatherInfo.weather[0].icon;

                        //Converting OBJECT to JSON String   
                        //jsonstring = new JavaScriptSerializer().Serialize(rslt);

                        ViewBag.weatherData = rslt;
                    ViewBag.IsSearched = true;
                    // Pass weather data to the view

                    return View(rslt);
                }
            }
          


            ModelState.AddModelError(string.Empty, "City is required");
            return View();
        }
    }
    }

