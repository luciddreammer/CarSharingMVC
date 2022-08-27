using CarSharing.Models.DataBaseModels;
using CarSharing.Models.ViewModels;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace CarSharing.Models.Repositories
{
    public class CarRepository : ICarRepository
    {
        private CarSharingContext _context;
        private ListOfCars CarList = new ListOfCars();

        public CarRepository(CarSharingContext context)
        {
            _context = context;
        }

        ListOfCars ICarRepository.GetCars()
        {
            CarList.listOfCars = _context.Cars.ToList();
            Car car = new Car();
            car.relations.AddRange(_context.Relations
                .Include(c => c.car)
                .Include(r => r.reservation)
                .Include(cu => cu.customer));
            return CarList;
        }
        
        void ICarRepository.AddCar(string carColor, string carEngine, string carModel, string carBrand, string carAdditionalEq)
        {
            Car dataBaseCar = new Car();
            dataBaseCar.model = carModel;
            dataBaseCar.brand = carBrand;
            dataBaseCar.additionalEquipment = carAdditionalEq;
            dataBaseCar.color = carColor;
            dataBaseCar.engine = carEngine;
            _context.Cars.Add(dataBaseCar);
            _context.SaveChanges();
        }
        void ICarRepository.RemoveCar(int carId)
        {
            Car selectedCar = _context.Cars.FirstOrDefault(x => x.id == carId);
            _context.Remove(selectedCar);
            _context.SaveChanges();
        }

        Car ICarRepository.FindCar(int id)
        {
            Car car = _context.Cars.FirstOrDefault(x => x.id == id);
            return car;
        }

        Car ICarRepository.FindCarWithRelations(int id)
        {
            return _context.Cars.Include(x => x.relations).ThenInclude(x => x.reservation).FirstOrDefault(x => x.id == id);
        }
    }


}

