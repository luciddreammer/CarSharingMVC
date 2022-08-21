using Microsoft.AspNetCore.Mvc;
using CarSharing.Models.ViewModels;
using CarSharing.Models.Errors;
using CarSharing.Models.DataBaseModels;
using CarSharing.ModelServices;

namespace CarSharing.Controllers
{
    public class ReserveController : Controller
    {
        ReservationServices reservationService;

        public ReserveController(ReservationServices reservationservice)
        {
            this.reservationService = reservationservice;
        }

        public IActionResult Reserve(CarViewModel carViewModel)
        {
            Car car = reservationService.FindCar(carViewModel.id);
            carViewModel = reservationService.CarToViewModelTransfer(car);
            return View(carViewModel);
        }

        public IActionResult ReservationComplete()
        {
            return View("ReservationComplete");
        }

        [HttpPost]
        public IActionResult NewReservation(ReservationViewModel reservationViewModel)
        {
            if (!reservationService.VerifyDates(reservationViewModel))
            {
                CarViewModel carViewModel = new();
                carViewModel.id = reservationViewModel.carId;
                 return RedirectToAction("Reserve", carViewModel);
            }
            reservationService.DataBaseSetUp(reservationViewModel);
            return RedirectToAction("ReservationComplete");
        }
    }
}
