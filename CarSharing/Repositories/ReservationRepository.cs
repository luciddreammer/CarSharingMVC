using CarSharing.Models;
using CarSharing.Models.DataBaseModels;

namespace CarSharing.Repositories
{
    public class ReservationRepository : IReservationRepository
    {
        private CarSharingContext _context;
        public ReservationRepository(CarSharingContext context)
        {
            _context = context;
        }

        void IReservationRepository.RemoveReservation(int reservationId)
        {
            var selectedCar = _context.Reservations.FirstOrDefault(x => x.id == reservationId);
            _context.Remove(selectedCar);
            _context.SaveChanges();
        }

        void IReservationRepository.AddRelation(Relation relation)
        {
            _context.Relations.Add(relation);
            _context.SaveChanges();
        }

        void IReservationRepository.AddReservation(Reservation reservation)
        {
            _context.Reservations.Add(reservation);
            _context.SaveChanges();
        }
    }
}
