using CarSharing.Models.DataBaseModels;
using CarSharing.Models.Errors;
using CarSharing.Models;
using CarSharing.Factories;
using CarSharing.Repositories;


namespace CarSharing.ModelServices
{
    public class LoginServices
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private ICustomerRepository _customerRepository;

        public LoginServices(IHttpContextAccessor httpContextAccessor, CarSharingContext context)
        {
            _httpContextAccessor = httpContextAccessor;
            CustomerRepoFactory _customerRepoFactory = CustomerRepoFactory.Instance();
            _customerRepository = _customerRepoFactory.Build(context);
        }

        public bool CheckCookie()
        {
            return _httpContextAccessor.HttpContext.Request.Cookies["Session_Id"] != null;
        }

        public bool LoginValidation(Customer customer)
        {
            LoginFlags.loginError = false;
            LoginFlags.passwordError = false;
            var user = _customerRepository.FindCustomerWithLogin(customer.login);
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
            } while (_customerRepository.GetCustomer(randomNumber.ToString()) != null);
            user.cookieId = randomNumber;
            _customerRepository.CreateCookie(user);
            _httpContextAccessor.HttpContext.Response.Cookies.Append("Session_Id", randomNumber.ToString(), cookieOptions);
        }

        public void RemoveCookie()
        {
            var cookie_Id = _httpContextAccessor.HttpContext.Request.Cookies["Session_Id"];
            var cookieString = cookie_Id.ToString();
            try
            {
                var currentUser = _customerRepository.GetCustomer(cookieString);
                currentUser.cookieId = 0;
                _customerRepository.CreateCookie(currentUser);
            }
            catch
            {
                _httpContextAccessor.HttpContext.Response.Cookies.Delete("Session_Id");
            }
            _httpContextAccessor.HttpContext.Response.Cookies.Delete("Session_Id");

        }
    }
}
