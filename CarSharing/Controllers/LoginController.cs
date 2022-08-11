using Microsoft.AspNetCore.Mvc;
using CarSharing.Models.DataBaseModels;
using CarSharing.Models.Errors;
using CarSharing.Models;
using CarSharing.Models.ViewModels;
using CarSharing.ModelServices;

namespace CarSharing.Controllers
{
    public class LoginController : Controller
    {

        public LoginServices loginServices;
        public CarListServices carListServices;

        public LoginController(LoginServices loginservices)
        {
            this.loginServices = loginservices;
        }
        public IActionResult Login(Customer customer)
        {
            if (loginServices.CheckCookie())
            {
                return View("Logged", customer);
            }
            return View(customer);
        }
        [HttpPost]
        public IActionResult LoginUser(Customer customer)
        {
            if(!loginServices.LoginValidation(customer))
            {
                return View("Login");
            }
            return View("Logged");
        }

        public IActionResult Logout()
        {
            loginServices.RemoveCookie();
            return View("Login");
        }
    }
}
