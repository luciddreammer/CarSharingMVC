using CarSharing.Models.DataBaseModels;
using CarSharing.Models.ViewModels;
using CarSharing.Models;
using Microsoft.EntityFrameworkCore;
using CarSharing.Models.Repositories;
using CarSharing.Factories;

namespace CarSharing.ModelServices
{
    public class CarListServices
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private ICarRepository _carRepository;

        public CarListServices(CarSharingContext context, IHttpContextAccessor httpContextAccessor)
        {
            CarRepoFactory _carRepoFactory = CarRepoFactory.Instance();
            _httpContextAccessor = httpContextAccessor;
            _carRepository = _carRepoFactory.Build(context);
        }

        public bool CheckCookie()
        {
            return _httpContextAccessor.HttpContext.Request.Cookies["Session_Id"] != null;
        }

        public List<CarViewModel> GetListOfCars()
        {
            List<CarViewModel> carListViewModel = new List<CarViewModel>();
            ListOfCars CarList = new ListOfCars();
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
    }
}
