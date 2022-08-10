using CarSharing.Models.DataBaseModels;
using CarSharing.Models.ViewModels;
using CarSharing.Models;
using Microsoft.EntityFrameworkCore;

namespace CarSharing.ModelServices
{
    public class CustomerServices
    {
        private CarSharingContext _context;

        public CustomerServices(CarSharingContext context)
        {
            _context = context;
        }

        public void CustomerAddToDataBase(Customer customer)
        {

            _context.Customers.Add(customer);
            _context.SaveChanges();
        }

        public Customer FindCustomerViaLogin(string login)
        {
            Customer? customer = _context.Customers.FirstOrDefault(c => c.login == login);
            return customer;
        }

        public Customer FindCustomerViaEmail(string email)
        {
            Customer? customer = _context.Customers.FirstOrDefault(c => c.email == email);
            return customer;
        }

        public Customer FindCustomerViaId(int id)
        {
            Customer? customer = _context.Customers.FirstOrDefault(c => c.id == id);
            return customer;
        }

        public Customer FindCustomerViaCookie(double cookieId)
        {
            Customer? customer = _context.Customers.FirstOrDefault(c => c.cookieId == cookieId);
            return customer;
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
