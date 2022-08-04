using Microsoft.AspNetCore.Mvc;
using CarSharing.Models.ViewModels;
using System.Diagnostics;
using CarSharing.ModelServices;
using CarSharing.Models.Errors;
using CarSharing.Models.DataBaseModels;
using CarSharing.Models;
using CarSharing.Models.Helper;


namespace CarSharing.Controllers
{
    public class RegisterController : Controller
    {
        RegisterValidationHelper validationHelper;
        CustomerServices customerServices;

        public RegisterController(CustomerServices customerService) //Dependency injection
        {
            this.customerServices = customerService;
        }

        public IActionResult Register(RegisterViewModel registerCustomer)
        {
            return View(registerCustomer);
        }

        [HttpPost]
        public ActionResult RegisterNewUser(RegisterViewModel registerCustomer)
        {
            Customer customer = new Customer();
            if (!validationHelper.RegisterVerification(registerCustomer))
            {
                return RedirectToAction("Register", registerCustomer);
            }
            customer = customerServices.RegisterViewModelToCustomerTransfer(registerCustomer);
            customerServices.CustomerAddToDataBase(customer);
            return RedirectToAction("Register");
        }
    }
}