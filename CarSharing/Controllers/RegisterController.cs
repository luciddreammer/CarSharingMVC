using Microsoft.AspNetCore.Mvc;
using CarSharing.Models;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace CarSharing.Controllers
{
    public class RegisterController : Controller
    {
        private CarSharingContext _context;

        public RegisterController(CarSharingContext context)
        {
            _context = context;
        }
        public IActionResult Register(Customer customer)
        {
            return View(customer);
        }

        public bool EmailValidation(Customer customer)
        {
            Regex emailRegex = new Regex(@"^((\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*))", RegexOptions.IgnoreCase);

            return emailRegex.IsMatch(customer.email);
        }
        public bool RegisterVerification(Customer customer)
        {
            RegisterErrors.nameError = false;
            RegisterErrors.passwordError = false;
            RegisterErrors.loginError = false;
            RegisterErrors.dateOfBirthError = false;
            RegisterErrors.phoneError = false;
            RegisterErrors.emailError = false;
            RegisterErrors.cityError = false;
            RegisterErrors.loginExists = false;
            RegisterErrors.emailExists = false;
            bool isError = false;
            if (string.IsNullOrEmpty(customer.login) || !customer.login.All(char.IsLetterOrDigit))
            {
                RegisterErrors.loginError = true;
                isError = true;
            }
            var existingLogin = _context.Customers.FirstOrDefault(c => c.login == customer.login);
            if(existingLogin != null)
            {
                RegisterErrors.loginExists = true;
                isError = true;
            }
            if (string.IsNullOrEmpty(customer.password))
            {
                RegisterErrors.passwordError = true;
                isError = true;
            }
            if (string.IsNullOrEmpty(customer.email) || !EmailValidation(customer))
            {
                RegisterErrors.emailError = true;
                isError = true;
            }
            var existingEmail = _context.Customers.FirstOrDefault(c => c.email == customer.email);
            if( existingEmail != null)
            {
                RegisterErrors.emailExists = true;
                isError = true;
            }
            if (string.IsNullOrEmpty(customer.name) || !customer.name.All(char.IsLetter))
            {
                RegisterErrors.nameError = true;
                isError = true;
            }
            if (string.IsNullOrEmpty(customer.city) || !customer.city.All(char.IsLetter))
            {
                RegisterErrors.cityError = true;
                isError = true;
            }
            if (string.IsNullOrEmpty(customer.phone) || !customer.phone.All(char.IsDigit))
            {
                RegisterErrors.phoneError = true;
                isError = true;
            }
            if (DateTime.Now.Year-customer.dateOfBirth.Year <18 || customer.dateOfBirth.Year==0001)
            {
                RegisterErrors.dateOfBirthError = true;
                isError = true;
            }
            if(isError)
            {
                return false;
            }
            return true;
        }

        [HttpPost]
        public ActionResult RegisterNewUser(Customer customer)
        {
            if(!RegisterVerification(customer))
            {
                return RedirectToAction("Register", customer);
            }
            customer.age = DateTime.Now.Year - customer.dateOfBirth.Year;
            customer.premiumStatus = false;
            customer.adminAccount = false;
            _context.Customers.Add(customer);
            _context.SaveChanges();
            return RedirectToAction("Register");
        }
    }
}