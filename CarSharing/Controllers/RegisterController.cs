using Microsoft.AspNetCore.Mvc;
using CarSharing.Models.ViewModels;
using CarSharing.ModelServices;
using CarSharing.Models.DataBaseModels;


namespace CarSharing.Controllers
{
    public class RegisterController : Controller
    {
        RegisterServices registerServices;

        public RegisterController(RegisterServices customerService) //Dependency injection
        {
            this.registerServices = customerService;
        }

        public IActionResult Register(RegisterViewModel registerCustomer)
        {
            return View(registerCustomer);
        }

        [HttpPost]
        public ActionResult RegisterNewUser(RegisterViewModel registerCustomer)
        {
            if (!registerServices.RegisterVerification(registerCustomer))
            {
                return RedirectToAction("Register", registerCustomer);
            }
            Customer customer = registerServices.RegisterViewModelToCustomerTransfer(registerCustomer);
            registerServices.CustomerAddToDataBase(customer);
            return RedirectToAction("Register");
        }
    }
}