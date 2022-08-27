using CarSharing.Models.ViewModels;
using CarSharing.Models;
using CarSharing.Models.Errors;
using CarSharing.Models.DataBaseModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CarSharing.Factories;
using CarSharing.Repositories;
using CarSharing.Models.Repositories;

namespace CarSharing.ModelServices
{
    public class ReservationServices
    {
        CarViewModel carViewModel = new();
        private readonly IHttpContextAccessor _httpContextAccessor;
        private IReservationRepository _reservationRepository;
        private ICarRepository _carRepository;
        private ICustomerRepository _customerRepository;

        public ReservationServices(CarSharingContext context, IHttpContextAccessor http)
        {
            ReservationRepoFactory _reservationRepoFactory = ReservationRepoFactory.Instance();
            CarRepoFactory _carRepoFactory = CarRepoFactory.Instance();
            CustomerRepoFactory _customerRepoFactory = CustomerRepoFactory.Instance();
            _customerRepository = _customerRepoFactory.Build(context);
            _reservationRepository = _reservationRepoFactory.Build(context);
            _carRepository = _carRepoFactory.Build(context);
            _httpContextAccessor = http;
        }

        public Car FindCar(int carId)
        {
            return _carRepository.FindCar(carId);
        }

        public CarViewModel CarToViewModelTransfer(Car car)
        {
            //Tu inny sposob
            //List<Car> selectedCar = _context.Cars.Include(x => x.relations).Where(x => x.id==car.id).ToList();
            Car selectedCar = _carRepository.FindCarWithRelations(car.id);
            foreach (var rel in selectedCar.relations)
            {
                carViewModel.reservations.Add(new ReservationViewModel
                {
                    reservationId = rel.reservation.id,
                    rentedFrom = rel.reservation.rentedFrom,
                    rentedTo = rel.reservation.rentedTo
                });
            }
            carViewModel.id = car.id;
            carViewModel.brand = car.brand;
            carViewModel.color = car.color;
            carViewModel.model = car.model;
            carViewModel.additionalEquipment = car.additionalEquipment;
            carViewModel.engine = car.engine;
            return carViewModel;
        }

        public bool VerifyDates(ReservationViewModel reservation)
        {
            ReserveErrors.MaxLengthError = false;
            ReserveErrors.PastError = false;
            ReserveErrors.ToNullError = false;
            ReserveErrors.FromNullError = false;
            ReserveErrors.AnotherReservationError = false;
            if (reservation.rentedFrom == default(DateTime))
            {
                ReserveErrors.FromNullError = true;
                return false;
            }
            if (reservation.rentedTo == default(DateTime))
            {
                ReserveErrors.ToNullError = true;
                return false;
            }
            if ((reservation.rentedFrom - DateTime.Now).Days < 0)
            {
                ReserveErrors.PastError = true;
                return false;
            }
            if ((reservation.rentedFrom - reservation.rentedTo).Days > 21 || (reservation.rentedTo - reservation.rentedFrom).Days < 1)
            {
                ReserveErrors.MaxLengthError = true;
                return false;
            }//year must be the same
            Car selectedCar = _carRepository.FindCarWithRelations(reservation.carId);
            foreach (var res in selectedCar.relations)
            {
                bool conditionOne = (reservation.rentedFrom - res.reservation.rentedFrom).Days < 0 || (reservation.rentedFrom - res.reservation.rentedTo).Days > 0;
                bool conditionTwo = (reservation.rentedTo - res.reservation.rentedFrom).Days < 0 || (reservation.rentedTo - res.reservation.rentedTo).Days > 0;
                if (!(conditionOne && conditionTwo))
                {
                    ReserveErrors.AnotherReservationError = true;
                    return false;
                }
            }
            return true;
        }
        public void DataBaseSetUp(ReservationViewModel reservationViewModel)
        {
            var selectedCar = FindCar(reservationViewModel.carId);
            var cookieString = _httpContextAccessor.HttpContext.Request.Cookies["Session_Id"];
            Customer currentUser = _customerRepository.GetCustomer(cookieString);
            var newReservation = new Reservation { rentedFrom = reservationViewModel.rentedFrom, rentedTo = reservationViewModel.rentedTo};
            _reservationRepository.AddReservation(newReservation);
            var newRelation = new Relation { carId = reservationViewModel.carId, customerId = currentUser.id, reservationId = newReservation.id };
            _reservationRepository.AddRelation(newRelation);
        }
    }
}
