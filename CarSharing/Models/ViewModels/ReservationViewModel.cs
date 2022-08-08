namespace CarSharing.Models.ViewModels
{
    public class ReservationViewModel
    {
        public int id { get; set; }
        public DateTime rentedFrom { get; set; } = default;
        public DateTime rentedTo { get; set; } = default;
    }
}
