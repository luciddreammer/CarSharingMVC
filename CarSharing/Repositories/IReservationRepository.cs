using CarSharing.Models.DataBaseModels;

namespace CarSharing.Repositories
{
    public interface IReservationRepository
    {
        void RemoveReservation(int id);
        void AddReservation(Reservation reservation);
        void AddRelation(Relation relation);
    }
}
