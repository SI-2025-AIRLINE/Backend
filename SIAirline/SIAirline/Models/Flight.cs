using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

    public class Flight
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(20)]
        public string FlightNumber { get; set; }

        [Required]
        public int AircraftId { get; set; }

        [Required]
        public int DepartureDestinationId { get; set; }

        [Required]
        public int ArrivalDestinationId { get; set; }

        [Required]
        public DateTime DepartureTime { get; set; }

        [Required]
        public DateTime ArrivalTime { get; set; }

        [Required]
        public string Status { get; set; } // Use enum if needed

        [Required]
        public float EconomyPrice { get; set; }

        [Required]
        public float BusinessPrice { get; set; }

        [Required]
        public float FirstClassPrice { get; set; }

        [Required]
        public int Capacity { get; set; }

        [Required]
        public int AvailableSeats { get; set; }

        // Navigation properties
        [ForeignKey("AircraftId")]
        public Aircraft Aircraft { get; set; }

        [ForeignKey("DepartureDestinationId")]
        public Destination DepartureDestination { get; set; }

        [ForeignKey("ArrivalDestinationId")]
        public Destination ArrivalDestination { get; set; }
    }
