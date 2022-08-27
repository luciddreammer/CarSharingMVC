using CarSharing.Models;
using CarSharing.Repositories;

namespace CarSharing.Factories
{
    public sealed class CustomerRepoFactory
    {
        public static CustomerRepoFactory _instance;

        CustomerRepoFactory()
        {
        }

        public static CustomerRepoFactory Instance()
        {
            if(_instance == null)
            {
                _instance = new CustomerRepoFactory();
            }
            return _instance;
        }

        public ICustomerRepository Build(CarSharingContext context)
        {
            return new CustomerRepository(context);
        }
    }
}
