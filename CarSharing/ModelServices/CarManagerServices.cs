using CarSharing.Models;
using System.Diagnostics;
using CarSharing.Models.ViewModels;
using CarSharing.Models.DataBaseModels;
using Microsoft.EntityFrameworkCore;
using CarSharing.Models.Errors;
using CarSharing.Repositories;
using CarSharing.Factories;
using CarSharing.Models.Repositories;

namespace CarSharing.ModelServices
{
    public class CarManagerServices
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private ICustomerRepository _customerRepository;
        private ICarRepository _carRepository;
        private IReservationRepository _reservationRepository;

        public CarManagerServices(IHttpContextAccessor httpContextAccessor, CarSharingContext context)
        {
            CustomerRepoFactory _customerRepoFactory = CustomerRepoFactory.Instance();
            CarRepoFactory _carRepoFactory = CarRepoFactory.Instance();
            ReservationRepoFactory _reservationRepoFactory = ReservationRepoFactory.Instance();
            _reservationRepository = _reservationRepoFactory.Build(context);
            _carRepository = _carRepoFactory.Build(context);
            _customerRepository = _customerRepoFactory.Build(context);
            _httpContextAccessor = httpContextAccessor;
        }

        public bool AdminCheck()
        {
            if (_httpContextAccessor.HttpContext.Request.Cookies["Session_Id"] != null)
            {
                var cookieString = _httpContextAccessor.HttpContext.Request.Cookies["Session_Id"].ToString();
                return _customerRepository.CheckAdminAccount(cookieString);
            }
            else
            {
                return false;
            }
        }

        public List<CarViewModel> GetListOfCars()
        {
            ListOfCars CarList = new ListOfCars();
            List<CarViewModel> carListViewModel = new List<CarViewModel>();
            CarList = _carRepository.GetCars();
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
            _carRepository.RemoveCar(carId);
        }

        public void DeleteReservation(int reservationId)
        {
            _reservationRepository.RemoveReservation(reservationId);
        }

        public void AddCar(CarViewModel car)
        {
            _carRepository.AddCar(car.color, car.engine, car.model, car.brand, car.additionalEquipment);
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
