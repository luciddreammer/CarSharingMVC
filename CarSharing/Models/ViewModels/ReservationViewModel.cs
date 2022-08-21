namespace CarSharing.Models.ViewModels
{
    public class ReservationViewModel
    {
        public int carId { get; set; }
        public int reservationId { get; set; }
        public DateTime rentedFrom { get; set; } = default;
        public DateTime rentedTo { get; set; } = default;
        public int customerId { get; set; }
    }
}
