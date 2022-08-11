using Microsoft.AspNetCore.Mvc;
using CarSharing.Models.DataBaseModels;
using CarSharing.Models.Errors;
using CarSharing.Models;
using CarSharing.Models.ViewModels;


namespace CarSharing.ModelServices
{
    public class LoginServices
    {
        private CarSharingContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public LoginServices(IHttpContextAccessor httpContextAccessor, CarSharingContext context)
        {
            _httpContextAccessor = httpContextAccessor;
            _context = context;
        }

        public bool CheckCookie()
        {
            return _httpContextAccessor.HttpContext.Request.Cookies["Session_Id"] != null;
        }

        public bool LoginValidation(Customer customer)
        {
            LoginFlags.loginError = false;
            LoginFlags.passwordError = false;
            var user = _context.Customers.FirstOrDefault(x => x.login == customer.login);
            if(user == null)
            {
                LoginFlags.loginError = true;
                return false;
            }
            if(user.password != customer.password)
            {
                LoginFlags.passwordError = true;
                return false;
            }
            CreateCookie(user);
            return true;
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
            _context.Customers.Update(user);
            _context.SaveChanges();
            _httpContextAccessor.HttpContext.Response.Cookies.Append("Session_Id", randomNumber.ToString(), cookieOptions);
        }

        public void RemoveCookie()
        {
            var cookie_Id = _httpContextAccessor.HttpContext.Request.Cookies["Session_Id"];
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
                _httpContextAccessor.HttpContext.Response.Cookies.Delete("Session_Id");
            }
            _httpContextAccessor.HttpContext.Response.Cookies.Delete("Session_Id");

        }
    }
}
