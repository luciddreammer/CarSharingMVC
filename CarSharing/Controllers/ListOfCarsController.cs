using Microsoft.AspNetCore.Mvc;
using CarSharing.Models;
using Microsoft.EntityFrameworkCore;


namespace CarSharing.Controllers
{
    public class ListOfCarsController : Controller
    {

        private CarSharingContext _context;

        public ListOfCars CarList = new ListOfCars();

        public ListOfCarsController(CarSharingContext context)
        {
            _context = context;
        }
        public IActionResult ListOfCars()
        {
            List<Reservation> listOfReservations = new List<Reservation>();
            CarList.listOfCars = _context.Cars.ToList();
            Car car = new Car();
            car.relations.AddRange(_context.Relations
                .Include(c => c.car)
                .Include(r => r.reservation)
                .Include(cu => cu.customer));
            

            var sessionId = HttpContext.Request.Cookies["Session_Id"];
            if (sessionId != null)
            {
                return View("Logged", CarList);
            }
            return View("NotLogged", CarList);
        }

    }
}
