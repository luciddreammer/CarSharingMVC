using System.ComponentModel.DataAnnotations;

namespace CarSharing.Models
{
    public class Customer
    {
        [Key]
        public int id { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
        public string city { get; set; }
        public int age { get; set; }
        public DateTime dateOfBirth { get; set; }
        public string login { get; set; }
        public string password { get; set; }
        public bool premiumStatus { get; set; }
        public bool ammountOfReservedCars { get; set; }
        public bool adminAccount { get; set; }
        public double cookieId { get; set; }
        public List<Relation> relations = new List<Relation>();
    }
}
