
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
public enum DestinationType
{
    NeZnam,
    Znam
}
    public class Destination
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(255)]
        public string Name { get; set; }

        [Required]
        [MaxLength(255)]
        public string CityCode { get; set; }

        [Required]
        [MaxLength(3)]
        public string AirportCode { get; set; }

    [Required]
    public DestinationType Status { get; set; } = DestinationType.NeZnam;

        // Navigation properties for related Flights
        public ICollection<Flight> DepartureFlights { get; set; }
        public ICollection<Flight> ArrivalFlights { get; set; }
    }
