using CarSharing.Models.DataBaseModels;
using CarSharing.Models.ViewModels;

namespace CarSharing.Models.Repositories
{
    public interface ICarRepository
    {
        ListOfCars GetCars();
        void AddCar(string carColor, string carEngine, string carModel, string carBrand, string carAdditionalEq);
        void RemoveCar(int id);
        Car FindCar(int id);
        Car FindCarWithRelations(int id);
    }
}
