using System.ComponentModel.DataAnnotations;


namespace CarSharing.Models

{
    public class Relation
    {
        [Key]
        public int Id { get; set; }
        public int carId { get; set; }
        public Car car { get; set; }
        public int reservationId { get; set; }
        public Reservation reservation { get; set; }
        public int customerId { get; set; }
        public Customer customer { get; set; }

        public Relation(int carId)
        {
            this.carId = carId;
        }
        public Relation()
        {

        }

    }
}
