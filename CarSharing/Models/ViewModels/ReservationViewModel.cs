namespace CarSharing.Models.ViewModels
{
    public class ReservationViewModel
    {
        public int carId { get; set; }
        public string brand { get; set; }
        public string model { get; set; }
        public string engine { get; set; }
        public string color { get; set; }
        public string additionalEquipment { get; set; }
        public int reservationId { get; set; }
        public DateTime rentedFrom { get; set; } = default;
        public DateTime rentedTo { get; set; } = default;
        public int customerId { get; set; }
        public List<ReservationViewModel> reservations = new List<ReservationViewModel>();
    }
}
