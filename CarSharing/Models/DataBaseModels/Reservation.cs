using System.ComponentModel.DataAnnotations;

namespace CarSharing.Models.DataBaseModels
{
    public class Reservation
    {
        [Key]
        public int id { get; set; }
        public DateTime rentedFrom { get; set; } = default;
        public DateTime rentedTo { get; set; } = default;
        public Relation relation { get; set; }
        //public int carId { get; set; }
        //public Car car { get; set; }
        //public int customerId { get; set; }
        //public Customer customer { get; set; }
        //public Reservation(int carId)
        //{
        //    this.carId = carId;
        //}

        //public Reservation()
        //{

        //}
    }
}
