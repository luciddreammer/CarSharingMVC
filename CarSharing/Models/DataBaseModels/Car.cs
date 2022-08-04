using System.ComponentModel.DataAnnotations;


namespace CarSharing.Models.DataBaseModels
{
    public class Car
    {
        [Key]
        public int id { get; set; }
        public string brand { get; set; }
        public string model { get; set; }
        public string engine { get; set; }
        public string color { get; set; }
        public string additionalEquipment { get; set; }
        public List<Relation> relations = new List<Relation>();

    }
}
