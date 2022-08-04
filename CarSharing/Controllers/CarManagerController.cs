using Microsoft.AspNetCore.Mvc;
using CarSharing.Models;
using CarSharing.Models.Errors;
using CarSharing.Models.DataBaseModels;

namespace CarSharing.Controllers
{
    public class CarManagerController : Controller
    {

        private CarSharingContext _context;

        public ListOfCars CarList = new ListOfCars();

        public CarManagerController(CarSharingContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            bool allow = true;

            try
            {
                var cookie_Id = Request.Cookies["Session_Id"];
                var cookieString = cookie_Id.ToString();
                var currentUser = _context.Customers.FirstOrDefault(x => x.cookieId == double.Parse(cookieString));
                if (currentUser.adminAccount == false)
                {
                    allow = false;
                }
                
            }
            catch
            {
                allow = false;
            }
            ViewBag.Allow = allow;
            CarList.listOfCars = _context.Cars.ToList();
            List<Reservation> reservationList = new List<Reservation>();
            return View(CarList);
        }

        public IActionResult IndexAddCar(Car car)
        {

            return View(car);
        }

        public IActionResult DeleteReservation(int carId)
        {
            var selectedCar = _context.Cars.FirstOrDefault(x => x.id == carId);
            //selectedCar.rentedStatus = false;
            _context.Update(selectedCar);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        public IActionResult RemoveCar(int carId)
        {
            var selectedCar = _context.Cars.FirstOrDefault(x => x.id == carId);
            _context.Remove(selectedCar);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpPost]
        public IActionResult AddCar(Car car)
        {
            if(!ValidateAddingCar(car))
            {
                return View("IndexAddCar",car);
            }
            //car.nameOfActualCustomer = "none";
            //car.rentedStatus = false;
            //car.rentedFrom = default(DateTime);
            //car.rentedTo = default(DateTime);
            _context.Cars.Add(car);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        public bool ValidateAddingCar(Car car)
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
