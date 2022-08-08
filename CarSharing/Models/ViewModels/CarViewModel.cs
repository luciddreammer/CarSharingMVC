namespace CarSharing.Models.ViewModels
{
    public class CarViewModel
    {
        public int id { get; set; }
        public string brand { get; set; }
        public string model { get; set; }
        public string engine { get; set; }
        public string color { get; set; }
        public string additionalEquipment { get; set; }
        public List<ReservationViewModel> reservations = new List<ReservationViewModel>();
    }
}
