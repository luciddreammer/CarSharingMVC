using CarSharing.Models.ViewModels;
using CarSharing.Models;
using CarSharing.Models.Errors;
using CarSharing.Models.DataBaseModels;
using Microsoft.AspNetCore.Mvc;
using CarSharing.Models.Helper;
using Microsoft.EntityFrameworkCore;


namespace CarSharing.ModelServices
{
    public class ReservationServices
    {
        ReservationViewModel reservationViewModel = new();
        CarSharingContext _context;
        Car car = new();
        CookieHelper cookieHelper = new();

        public ReservationServices(CarSharingContext context)
        {
            _context = context;
        }

        public Car FindCar(int carId)
        {
            car = _context.Cars.FirstOrDefault(x => x.id == carId);
            return car;
        }

        public ReservationViewModel CarToViewModelTransfer(Car car)
        {
            reservationViewModel.carId = car.id;
            reservationViewModel.brand = car.brand;
            reservationViewModel.color = car.color;
            reservationViewModel.model = car.model;
            reservationViewModel.additionalEquipment = car.additionalEquipment;
            reservationViewModel.engine = car.engine;
            car = new Car();
            car.relations.AddRange(_context.Relations
                .Include(c => c.car)
                .Include(r => r.reservation)
                .Include(cu => cu.customer));

            foreach (var rel in car.relations)
            {
                reservationViewModel.reservations.Add(new ReservationViewModel
                {
                    reservationId = rel.reservation.id,
                    rentedFrom = rel.reservation.rentedFrom,
                    rentedTo = rel.reservation.rentedTo
                });
            }
            return reservationViewModel;
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

            foreach (var res in _context.Reservations.ToList())
            {
                bool conditionOne = (reservation.rentedFrom - res.rentedFrom).Days < 0 || (reservation.rentedFrom - res.rentedTo).Days > 0;
                bool conditionTwo = (reservation.rentedTo - res.rentedFrom).Days < 0 || (reservation.rentedTo - res.rentedTo).Days > 0;
                if (!(conditionOne && conditionTwo))
                {
                    ReserveErrors.AnotherReservationError = true;
                    return false;
                }
            }
            return true;
        }

        public void DataBaseSetUp(ReservationViewModel reservationViewModel, string cookie_Id)
        {
            var selectedCar = FindCar(reservationViewModel.carId);
            var cookieString = cookie_Id.ToString();
            Customer currentUser = _context.Customers.FirstOrDefault(x => x.cookieId == double.Parse(cookieString));
            var newReservation = new Reservation { rentedFrom = reservationViewModel.rentedFrom, rentedTo = reservationViewModel.rentedTo};
            _context.Reservations.Add(newReservation);
            _context.SaveChanges();
            var newRelation = new Relation { carId = reservationViewModel.carId, customerId = currentUser.id, reservationId = newReservation.id };
            _context.Relations.Add(newRelation);
            _context.SaveChanges();
        }
    }
}
