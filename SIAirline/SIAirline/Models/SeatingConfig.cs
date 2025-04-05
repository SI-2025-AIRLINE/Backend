using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public enum SeatClass
{
    Economy,
    Business,
    First
}
public class SeatingConfig
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int AircraftId { get; set; }

    [Required]
    public SeatClass SeatClass { get; set; } = SeatClass.Economy;

        [Required]
        public int RowCount { get; set; }

        [Required]
        public int SeatsPerRow { get; set; }

        // Navigation property for Aircraft
        [ForeignKey("AircraftId")]
        public Aircraft Aircraft { get; set; }
    }
