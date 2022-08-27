using Microsoft.AspNetCore.Mvc;
using CarSharing.Models;
using Microsoft.EntityFrameworkCore;
using CarSharing.Models.DataBaseModels;
using CarSharing.Models.ViewModels;
using CarSharing.ModelServices;


namespace CarSharing.Controllers
{
    public class ListOfCarsController : Controller
    {
        public CarListServices carListService;
        public ListOfCars CarList = new ListOfCars();

        public ListOfCarsController(CarListServices carListService)
        {
            this.carListService = carListService;
        }

        public IActionResult ListOfCars()
        {
            List<CarViewModel> carList = new List<CarViewModel>();
            carList = carListService.GetListOfCars();
            if (carListService.CheckCookie())
            {
                return View("Logged", carList);
            }
            return View("NotLogged", carList);
        }

    }
}
