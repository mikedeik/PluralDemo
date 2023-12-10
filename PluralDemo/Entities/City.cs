using PluralDemo.Models;
using System.ComponentModel.DataAnnotations;

namespace PluralDemo.Entities {
    public class City {

        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }

        public ICollection<PointOfInterest> PointsOfInterest { get; set; }
            = new List<PointOfInterest>();

        public City(string name) {
            Name = name;
        }
    }
}
