using CarSharing.Models.DataBaseModels;
using CarSharing.Models.ViewModels;
using CarSharing.Models;
using Microsoft.EntityFrameworkCore;


namespace CarSharing.ModelServices
{
    public class CarListService
    {
        public List<CarViewModel> carListViewModel = new List<CarViewModel>();
        public ListOfCars CarList = new ListOfCars();
        private CarSharingContext _context;

        public CarListService(CarSharingContext context)
        {
            _context = context;
        }

        public List<CarViewModel> GetListOfCars()
        {
            CarList.listOfCars = _context.Cars.ToList();
            Car car = new Car();
            car.relations.AddRange(_context.Relations
                .Include(c => c.car)
                .Include(r => r.reservation)
                .Include(cu => cu.customer));
            foreach (var singleCar in CarList.listOfCars)
            {
                CarViewModel singleCarViewModel = new();
                foreach (var rel in singleCar.relations)
                {
                    singleCarViewModel.reservations.Add(new ReservationViewModel
                    {
                        reservationId = rel.reservation.id,
                        rentedFrom = rel.reservation.rentedFrom,
                        rentedTo = rel.reservation.rentedTo
                    });
                }
                singleCarViewModel.id = singleCar.id;
                singleCarViewModel.color = singleCar.color;
                singleCarViewModel.model = singleCar.model;
                singleCarViewModel.brand = singleCar.brand;
                singleCarViewModel.engine = singleCar.engine;
                singleCarViewModel.additionalEquipment = singleCar.additionalEquipment;
                carListViewModel.Add(singleCarViewModel);
            }
            return carListViewModel;
        }
    }
}
