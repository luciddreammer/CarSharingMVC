using CarSharing.Models;
using CarSharing.Models.Repositories;

namespace CarSharing.Factories
{
    public sealed class CarRepoFactory
    {
        public static CarRepoFactory _instance;

        public CarRepoFactory()
        {
           
        }

        public static CarRepoFactory Instance()
        {
            if(_instance == null)
            {
                _instance = new CarRepoFactory();
            }
            return _instance;
        }

        public ICarRepository Build(CarSharingContext context)
        {
            return new CarRepository(context);
        }
    }
}
