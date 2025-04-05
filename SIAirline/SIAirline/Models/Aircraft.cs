
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


    public class Aircraft
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(255)]
        public string Model { get; set; }
        
        [Required]
        [MaxLength(255)]
        public string Description { get; set; }

        [Required]
        [MaxLength(255)]
        public string RegistrationNumber { get; set; }

        // Navigation property for Flights (one-to-many relationship)
        public ICollection<Flight> Flights { get; set; }
    }
