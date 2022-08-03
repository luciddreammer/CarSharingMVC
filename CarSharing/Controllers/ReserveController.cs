using Microsoft.AspNetCore.Mvc;
using CarSharing.Models;

namespace CarSharing.Controllers
{
    public class ReserveController : Controller
    {
        CarSharingContext _context;

        public ReserveController(CarSharingContext context)
        {
            ReserveErrors.MaxLengthError = false;
            ReserveErrors.PastError = false;
            ReserveErrors.ToNullError = false;
            ReserveErrors.FromNullError = false;
            ReserveErrors.AnotherReservationError = false;
            _context = context;
        }

        public IActionResult Reserve(int carId)
        {
            var car = _context.Cars.FirstOrDefault(x => x.id == carId);
            //ViewBag.CarViewBag = car.id;
            return View(car);
        }

        public bool VerifyDates(Reservation reservation, Car car)
        {
            

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
            }//Next day is min
            //if ((reservation.rentedFrom - DateTime.Now).TotalDays > 1)
            //{
            //    ReserveErrors.PastError = true;
            //    return false;
            //}//Next day is min

            if ((reservation.rentedFrom - reservation.rentedTo).Days > 21 || (reservation.rentedTo - reservation.rentedFrom).Days < 1)
            {
                ReserveErrors.MaxLengthError = true;
                return false;
            }//year must be the same

            //foreach(var res in car.reservations) zakomentowane do naprawienia bzy danych
            //{
            //    bool conditionOne = (reservation.rentedFrom - res.rentedFrom).Days < 0 || (reservation.rentedFrom - res.rentedTo).Days > 0;
            //    bool conditionTwo = (reservation.rentedTo - res.rentedFrom).Days < 0 || (reservation.rentedTo - res.rentedTo).Days > 0;
            //    if(!(conditionOne && conditionTwo))
            //    {
            //        ReserveErrors.AnotherReservationError = true;
            //        return false;
            //    }
            //}

            return true;
        }

        public void DataBaseSetUp(Car car)
        {
            var selectedCar = _context.Cars.FirstOrDefault(x => x.id == car.id);
            var cookie_Id = Request.Cookies["Session_Id"];
            var cookieString = cookie_Id.ToString();
            var currentUser = _context.Customers.FirstOrDefault(x => x.cookieId == double.Parse(cookieString)).name;
           // var newReservation = new Reservation { nameOfCustomer = currentUser, rentedFrom = };
            //selectedCar.rentedFrom = car.rentedFrom;
            //selectedCar.rentedTo = car.rentedTo;
            //selectedCar.rentedStatus = true;
            
            //selectedCar.nameOfActualCustomer = currentUser;
            //_context.Cars.Update(selectedCar);
            _context.SaveChanges();
        }

        [HttpPost]
        public IActionResult NewReservation(Relation relation)
        {
            bool reserved = false;
            Car car = _context.Cars.FirstOrDefault(x => x.id == relation.carId);

            //car = _context.Cars.Single(x => x.id == reservation.carId); // zakomentowane do naprawienia bazy danych
                
            //if (!VerifyDates(reservation, car))
            //{
            //    return View("Reserve", car);
            //}
            //DataBaseSetUp(car);
            //reserved = true;

            return View("ReservationComplete", relation);
        }
    }
}
