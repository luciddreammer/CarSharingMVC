using CarSharing.Models;
using CarSharing.Repositories;

namespace CarSharing.Factories
{
    public sealed class ReservationRepoFactory
    {
        public static ReservationRepoFactory _instance;

        ReservationRepoFactory()
        {
        }

        public static ReservationRepoFactory Instance()
        {
            if (_instance == null)
            {
                _instance = new ReservationRepoFactory();
            }
            return _instance;
        }

        public IReservationRepository Build(CarSharingContext context)
        {
            return new ReservationRepository(context);
        }
    }
}
