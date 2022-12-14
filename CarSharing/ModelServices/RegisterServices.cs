using CarSharing.Models.DataBaseModels;
using CarSharing.Models.ViewModels;
using CarSharing.Models;
using CarSharing.Models.Errors;
using System.Text.RegularExpressions;
using CarSharing.Repositories;
using CarSharing.Factories;


namespace CarSharing.ModelServices
{
    public class RegisterServices
    {
        private ICustomerRepository _customerRepository;

        public RegisterServices(CarSharingContext context)
        {
            CustomerRepoFactory customerRepoFactory = CustomerRepoFactory.Instance();
            _customerRepository = customerRepoFactory.Build(context);
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
            if (string.IsNullOrEmpty(registerCustomer.login) || !registerCustomer.login.All(char.IsLetterOrDigit))
            {
                RegisterErrors.loginError = true;
                return false;
            }
            if (_customerRepository.FindCustomerWithLogin(registerCustomer.login) != null)
            {
                RegisterErrors.loginExists = true;
                return false;
            }
            if (string.IsNullOrEmpty(registerCustomer.password))
            {
                RegisterErrors.passwordError = true;
                return false;
            }
            if (string.IsNullOrEmpty(registerCustomer.email) || !EmailValidation(registerCustomer))
            {
                RegisterErrors.emailError = true;
                return false;
            }
            if (_customerRepository.FindCustomerViaEmail(registerCustomer.email) != null)
            {
                RegisterErrors.emailExists = true;
                return false;
            }
            if (string.IsNullOrEmpty(registerCustomer.name) || !registerCustomer.name.All(char.IsLetter))
            {
                RegisterErrors.nameError = true;
                return false;
            }
            if (string.IsNullOrEmpty(registerCustomer.city) || !registerCustomer.city.All(char.IsLetter))
            {
                RegisterErrors.cityError = true;
                return false;
            }
            if (string.IsNullOrEmpty(registerCustomer.phone) || !registerCustomer.phone.All(char.IsDigit))
            {
                RegisterErrors.phoneError = true;
                return false;
            }
            if (DateTime.Now.Year - registerCustomer.dateOfBirth.Year < 18 || registerCustomer.dateOfBirth.Year == 0001)
            {
                RegisterErrors.dateOfBirthError = true;
                return false;
            }
            return true;
        }
        bool EmailValidation(RegisterViewModel registerCustomer)
        {
            Regex emailRegex = new Regex(@"^((\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*))", RegexOptions.IgnoreCase);

            return emailRegex.IsMatch(registerCustomer.email);
        }

        public void CustomerAddToDataBase(Customer customer)
        {
            _customerRepository.AddUser(customer);
        }

        public Customer RegisterViewModelToCustomerTransfer(RegisterViewModel registerCustomer)
        {
            Customer customer = new();
            customer.id = registerCustomer.Id;
            customer.name = registerCustomer.name;
            customer.email = registerCustomer.email;
            customer.phone = registerCustomer.phone;
            customer.city = registerCustomer.city;
            customer.age = registerCustomer.age;
            customer.dateOfBirth = registerCustomer.dateOfBirth;
            customer.login = registerCustomer.login;
            customer.password = registerCustomer.password;
            customer.premiumStatus = registerCustomer.premiumStatus;
            customer.ammountOfReservedCars = registerCustomer.ammountOfReservedCars;
            customer.adminAccount = registerCustomer.adminAccount;
            customer.cookieId = registerCustomer.cookieId;
            return customer;
        }
    }
}
