using Microsoft.AspNetCore.Identity;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Http;
using Weather_Deatils.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace Weather_Deatils.Controllers
{   
    public class UserController : Controller
    {
        private readonly WeatherDeatilsContext _context;
		private readonly IConfiguration _config;

		public UserController(WeatherDeatilsContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
			
		}
        public IActionResult Register()
        {
            return View(new UserCity());
        }
        [HttpPost]
        public IActionResult Register(UserCity user)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.UserCities.Add(user);
                    _context.SaveChanges();
                    return RedirectToAction("Login");
                }
                catch (DbUpdateException ex)
                {
                    // Log the exception or handle it appropriately
                    var errorMessage = ex.InnerException?.Message;
                    ModelState.AddModelError("", "An error occurred while saving the data. Please try again later.");
                   
                }
            }
            return View(user);
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [Authorize]
        [HttpGet]
        public IActionResult GetDashboard() {
			int userId = 0;
			if (User.Identity.IsAuthenticated)
			{
				var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "UserId");
				if (userIdClaim != null)
				{
					userId = int.Parse(userIdClaim.Value);
				}
			}
			ViewBag.UserId = userId;
			return Json(new { result = "Redirect", url = "/Weather/Search?userId="+userId, userid=ViewBag.UserId});

		}

        [Authorize]
        [HttpGet]
        public IActionResult Test() {
            return Json(new { login = "success"});
        }

        [HttpPost]
		[AllowAnonymous]
        public IActionResult Login1([FromBody] UserCity user)
        {
            var existingUser = _context.UserCities.FirstOrDefault(u => u.UserName == user.UserName && u.Password == user.Password);
            if (existingUser != null)
            {
                ViewBag.userid = existingUser.UserId;
                var token = GenerateToken(existingUser.UserName,existingUser.UserId);
				return Json(new { result = "Success", jwt = token, userid = existingUser.UserId });

			}

            ModelState.AddModelError(string.Empty, "Invalid email or password");
            return View(user);
        }
      
        [Httpost]
        public IActionResult SaveCitySearch(string cityName,  string userid)
        {
            if (!string.IsNullOrEmpty(cityName))
            {
                
                var existingUser = _context.UserCities.FirstOrDefault(u => u.UserId == Convert.ToInt32(userid));
                City objCity = new City();
                objCity.CityName = cityName;    
                objCity.UserId = Convert.ToInt32(userid);
                existingUser.Cities.Add(objCity);
                _context.UserCities.Update(existingUser);
                _context.SaveChanges();
            }

            return Json("Success"); 
        }
        [HttpGet]
        public IActionResult FavoriteCities(string userid)
        {
            var favoriteCities = _context.UserCities.Where(u => u.UserId == Convert.ToInt32(userid)).Select(u => u.Cities).ToList();

            // If no favorite cities found for the user, return an empty list
            if (!favoriteCities.Any())
            {
                return Json(new List<City>());
            }

            // Return the list of favorite cities as JSON
            return View(favoriteCities.SelectMany(c => c));
        }
       
        public string GenerateToken(string UserName, int UserId)
        {

            Jwtcs _jwtcs = new Jwtcs(_config);
            var token = _jwtcs.GenerateToken(UserName,UserId);
            return token;
        }


        public IActionResult Index()
            {
            return View();
            }
        }
    }

