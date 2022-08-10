﻿using Microsoft.AspNetCore.Mvc;
using CarSharing.Models.ViewModels;
using CarSharing.Models.Errors;
using CarSharing.Models.DataBaseModels;
using CarSharing.ModelServices;

namespace CarSharing.Controllers
{
    public class ReserveController : Controller
    {
        ReservationServices reservationService;
        ReservationViewModel reservationViewModel;
        Car car = new();

        public ReserveController(ReservationServices reservationservice)
        {
            this.reservationService = reservationservice;
        }

        public IActionResult Reserve(ReservationViewModel reservationViewModel)
        {
            car = reservationService.FindCar(reservationViewModel.carId);
            reservationViewModel = reservationService.CarToViewModelTransfer(car);
            return View(reservationViewModel);
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
                   return RedirectToAction("Reserve", reservationViewModel);
            }
            var cookie_Id = Request.Cookies["Session_Id"];
            reservationService.DataBaseSetUp(reservationViewModel, cookie_Id);
            return RedirectToAction("ReservationComplete");
        }
    }
}
