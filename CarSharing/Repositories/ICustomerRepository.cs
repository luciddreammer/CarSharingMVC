using CarSharing.Models.DataBaseModels;
using Microsoft.EntityFrameworkCore;

namespace CarSharing.Repositories
{
    public interface ICustomerRepository
    {
        bool CheckAdminAccount(string cookie);
        Customer GetCustomer(string cookie);
        Customer FindCustomerWithLogin(string login);
        void CreateCookie(Customer customer);
        void AddUser(Customer customer);
        Customer FindCustomerViaEmail(string email);
        Customer FindCustomerViaId(int id);
     


    }
}
