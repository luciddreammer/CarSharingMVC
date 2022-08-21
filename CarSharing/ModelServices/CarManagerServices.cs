using CarSharing.Models;
using System.Diagnostics;
using CarSharing.Models.ViewModels;
using CarSharing.Models.DataBaseModels;
using Microsoft.EntityFrameworkCore;
using CarSharing.Models.Errors;

namespace CarSharing.ModelServices
{
    public class CarManagerServices
    {
        public List<CarViewModel> carListViewModel = new List<CarViewModel>();
        private CarSharingContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public ListOfCars CarList = new ListOfCars();
        public Car car = new();

        public CarManagerServices(IHttpContextAccessor httpContextAccessor, CarSharingContext context)
        {
            _httpContextAccessor = httpContextAccessor;
            _context = context;
        }

        private bool CheckCookie()
        {
            return _httpContextAccessor.HttpContext.Request.Cookies["Session_Id"] != null;
        }

        public bool AdminCheck()
        {
            if (CheckCookie())
            {
                var cookieString = _httpContextAccessor.HttpContext.Request.Cookies["Session_Id"].ToString();
                var currentUser = _context.Customers.FirstOrDefault(x => x.cookieId == double.Parse(cookieString));
                return currentUser.adminAccount;
            }
            else
            {
                return false;
            }
            return true;
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

        public void RemoveCar(int carId)
        {
            var selectedCar = _context.Cars.FirstOrDefault(x => x.id == carId);
            _context.Remove(selectedCar);
            _context.SaveChanges();
        }

        public void DeleteReservation(int reservationId)
        {
            var selectedCar = _context.Reservations.FirstOrDefault(x => x.id == reservationId);
            _context.Remove(selectedCar);
            _context.SaveChanges();
        }

        public void AddCar(CarViewModel car)
        {
            Car dataBaseCar = new Car();
            dataBaseCar.id = car.id;
            dataBaseCar.model = car.model;
            dataBaseCar.brand = car.brand;
            dataBaseCar.additionalEquipment = car.additionalEquipment;
            dataBaseCar.color = car.color;
            dataBaseCar.engine = car.engine;
            _context.Cars.Add(dataBaseCar);
            _context.SaveChanges();
        }

        public bool ValidateAddingCar(CarViewModel car)
        {
            AddCarErrors.modelError = false;
            AddCarErrors.brandError = false;
            AddCarErrors.engineError = false;
            AddCarErrors.colorError = false;
            AddCarErrors.eqError = false;
            if (string.IsNullOrEmpty(car.brand))
            {
                AddCarErrors.brandError = true;
                return false;
            }
            if (string.IsNullOrEmpty(car.model))
            {
                AddCarErrors.modelError = true;
                return false;
            }
            if (string.IsNullOrEmpty(car.engine))
            {
                AddCarErrors.engineError = true;
                return false;
            }
            if (string.IsNullOrEmpty(car.color))
            {
                AddCarErrors.colorError = true;
                return false;
            }
            if (string.IsNullOrEmpty(car.additionalEquipment))
            {
                AddCarErrors.eqError = true;
                return false;
            }
            return true;
        }





    }
}
