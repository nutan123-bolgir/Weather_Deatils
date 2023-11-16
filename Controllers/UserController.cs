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
                    // You can also add the specific error message to the ModelState if needed.
                    // ModelState.AddModelError("", errorMessage);
                }
            }
            return View(user);
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
		[Authorize]
        public IActionResult Login1(UserCity user)
        {
            var existingUser = _context.UserCities.FirstOrDefault(u => u.UserName == user.UserName && u.Password == user.Password);

            if (existingUser != null)
            {
               
                ViewBag.userid = existingUser.UserId;

				return View(@"~/Views/Weather/Search.cshtml");
            }

            ModelState.AddModelError(string.Empty, "Invalid email or password");
            return View(user);
        }
        
        [Httpost]
        public IActionResult SaveCitySearch(string cityName, string userid)
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

        [HttpGet("GenerateToken")]
        public async Task<IActionResult> GenerateToken(string UserName , string Password)
        {

            Jwtcs _jwtcs = new Jwtcs(_config);
		    var token = await _jwtcs.GenerateToken( UserName).ConfigureAwait(false);
            return Json(token);
        }


        public IActionResult Index()
            {
            return View();
            }
        }
    }

