using Microsoft.AspNetCore.Mvc;
using CarSharing.Models;
using CarSharing.Models.Errors;
using CarSharing.Models.DataBaseModels;
using CarSharing.ModelServices;
using CarSharing.Models.ViewModels;

namespace CarSharing.Controllers
{
    public class CarManagerController : Controller
    {
        public CarManagerServices carManagerServices;
        public ListOfCarsViewModel listOfCars = new();

        public CarManagerController(CarManagerServices carManagerServices)
        {
            this.carManagerServices = carManagerServices;
        }

        public IActionResult Index()
        {
            ViewBag.Allow = carManagerServices.AdminCheck();
            listOfCars.listOfCars = carManagerServices.GetListOfCars();
            return View(listOfCars);
        }

        public IActionResult IndexAddCar(Car car)
        {
            return View(car);
        }

        public IActionResult DeleteReservation(int carId)
        {
            carManagerServices.DeleteReservation(carId);
            return RedirectToAction("Index");
        }
        public IActionResult RemoveCar(int carId)
        {
            carManagerServices.RemoveCar(carId);
            return RedirectToAction("Index");
        }
        [HttpPost]
        public IActionResult AddCar(CarViewModel car)
        {
            if (!carManagerServices.ValidateAddingCar(car))
            {
                return View("IndexAddCar", car);
            }
            carManagerServices.AddCar(car);
            return RedirectToAction("Index");
        }
    }
}
