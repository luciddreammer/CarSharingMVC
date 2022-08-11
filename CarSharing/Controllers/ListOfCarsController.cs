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
        CarListServices carListService;
        public ListOfCars CarList = new ListOfCars();

        public ListOfCarsController(CarListServices carListService)
        {
            this.carListService = carListService;
        }

        public IActionResult ListOfCars()
        {
            carListService.carListViewModel = carListService.GetListOfCars();
            if (carListService.CheckCookie())
            {
                return View("Logged", carListService);
            }
            return View("NotLogged", carListService);
        }

    }
}
