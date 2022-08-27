using CarSharing.Models;
using CarSharing.Models.DataBaseModels;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Plugins;

namespace CarSharing.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {

        private CarSharingContext _context;

        public CustomerRepository( CarSharingContext context)
        {
            _context = context;
        }

        bool ICustomerRepository.CheckAdminAccount(string cookie)
        {
            return _context.Customers.FirstOrDefault(x => x.cookieId == double.Parse(cookie)).adminAccount;
        }

        Customer? ICustomerRepository.GetCustomer(string cookie)
        {
           return _context.Customers.FirstOrDefault(x => x.cookieId == double.Parse(cookie));
        }

        Customer? ICustomerRepository.FindCustomerWithLogin(string login)
        {
            return _context.Customers.FirstOrDefault(x => x.login == login);
        }

        Customer? ICustomerRepository.FindCustomerViaEmail(string email)
        {
            return _context.Customers.FirstOrDefault(x => x.email == email);
        }

        Customer? ICustomerRepository.FindCustomerViaId(int id)
        {
            return _context.Customers.FirstOrDefault(x => x.id == id);
        }

        void ICustomerRepository.CreateCookie(Customer customer)
        {
            _context.Customers.Update(customer);
            _context.SaveChanges();
        }

        void ICustomerRepository.AddUser(Customer customer)
        {
            _context.Customers.Add(customer);
            _context.SaveChanges();
        }


    }
}
