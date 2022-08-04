using CarSharing.Models.ViewModels;
using CarSharing.Models.Errors;
using CarSharing.ModelServices;
using System.Text.RegularExpressions;


namespace CarSharing.Models.Helper
{
    public class RegisterValidationHelper
    {
        CustomerServices customerServices;
        public RegisterValidationHelper(CustomerServices customerService) //Dependency injection
        {
            this.customerServices = customerService;
        }
        public bool RegisterVerification(RegisterViewModel registerCustomer)
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
            if (string.IsNullOrEmpty(registerCustomer.login) || !registerCustomer.login.All(char.IsLetterOrDigit))
            {
                RegisterErrors.loginError = true;
                isError = true;
            }
            var existingLogin = customerServices.FindCustomerViaLogin(registerCustomer.login);
            if (existingLogin != null)
            {
                RegisterErrors.loginExists = true;
                isError = true;
            }
            if (string.IsNullOrEmpty(registerCustomer.password))
            {
                RegisterErrors.passwordError = true;
                isError = true;
            }
            if (string.IsNullOrEmpty(registerCustomer.email) || !EmailValidation(registerCustomer))
            {
                RegisterErrors.emailError = true;
                isError = true;
            }
            var existingEmail = customerServices.FindCustomerViaEmail(registerCustomer.email);
            if (existingEmail != null)
            {
                RegisterErrors.emailExists = true;
                isError = true;
            }
            if (string.IsNullOrEmpty(registerCustomer.name) || !registerCustomer.name.All(char.IsLetter))
            {
                RegisterErrors.nameError = true;
                isError = true;
            }
            if (string.IsNullOrEmpty(registerCustomer.city) || !registerCustomer.city.All(char.IsLetter))
            {
                RegisterErrors.cityError = true;
                isError = true;
            }
            if (string.IsNullOrEmpty(registerCustomer.phone) || !registerCustomer.phone.All(char.IsDigit))
            {
                RegisterErrors.phoneError = true;
                isError = true;
            }
            if (DateTime.Now.Year - registerCustomer.dateOfBirth.Year < 18 || registerCustomer.dateOfBirth.Year == 0001)
            {
                RegisterErrors.dateOfBirthError = true;
                isError = true;
            }
            if (isError)
            {
                return false;
            }
            return true;
        }
        bool EmailValidation(RegisterViewModel registerCustomer)
        {
            Regex emailRegex = new Regex(@"^((\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*))", RegexOptions.IgnoreCase);

            return emailRegex.IsMatch(registerCustomer.email);
        }

    }
}
