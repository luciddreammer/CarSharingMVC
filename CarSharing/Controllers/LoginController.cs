using Microsoft.AspNetCore.Mvc;
using CarSharing.Models;

namespace CarSharing.Controllers
{
    public class LoginController : Controller
    {

        private CarSharingContext _context;

        public LoginController(CarSharingContext context)
        {
            _context = context;
        }
        public IActionResult Login(Customer customer)
        {
            var sessionId = HttpContext.Request.Cookies["Session_Id"];
            if(sessionId != null)
            {
                return View("Logged", customer);
            }
            return View(customer);
        }

        public void CreateCookie(Customer user)
        {
            var rnd = new Random();
            CookieOptions cookieOptions = new CookieOptions();
            cookieOptions.Secure = true;
            cookieOptions.Expires = new DateTimeOffset(DateTime.Now.AddDays(7));
            double randomNumber;
            do
            {
                randomNumber = rnd.Next(10, 100000000);
            } while (_context.Customers.FirstOrDefault(x => x.cookieId == randomNumber) != null);
            user.cookieId = randomNumber;
            _context.SaveChanges();
            HttpContext.Response.Cookies.Append("Session_Id", randomNumber.ToString(), cookieOptions);
        }


        public IActionResult LoginUser(Customer customer)
        {
            LoginFlags.loginError = false;
            LoginFlags.passwordError = false;
            var user = _context.Customers.FirstOrDefault(x => x.login == customer.login);
            var conditionOne = user != null;
            if(!conditionOne)
            {
                LoginFlags.loginError = true;
                return View("Login");
            }
            var conditionTwo = user.password == customer.password;
            if (!conditionTwo)
            {
                LoginFlags.passwordError = true;
                return View("Login");
            }
            CreateCookie(user);
            return View("Logged");
        }

        public void RemoveCookie()
        {
            var cookie_Id = Request.Cookies["Session_Id"];
            var cookieString = cookie_Id.ToString();
            try
            {
                var currentUser = _context.Customers.FirstOrDefault(x => x.cookieId == double.Parse(cookieString));
                currentUser.cookieId = 0;
                _context.Customers.Update(currentUser);
                _context.SaveChanges();
            }
            catch
            {
                HttpContext.Response.Cookies.Delete("Session_Id");
            }
            HttpContext.Response.Cookies.Delete("Session_Id");

        }

        public IActionResult Logout()
        {
            RemoveCookie();
            return View("Login");
        }
    }
}
