using Microsoft.EntityFrameworkCore;
namespace SIAirline.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Aircraft> Aircrafts { get; set; }
        public DbSet<SeatingConfig> SeatingConfigs { get; set; }
        public DbSet<Destination> Destinations { get; set; }
        public DbSet<Flight> Flights { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure the one-to-one relationship between SeatingConfig and Aircraft
            modelBuilder.Entity<SeatingConfig>()
                .HasOne(sc => sc.Aircraft) // Each SeatingConfig belongs to one Aircraft
                .WithMany()               // Aircraft does not have a collection of SeatingConfigs
                .HasForeignKey(sc => sc.AircraftId) // Foreign key in SeatingConfig
                .OnDelete(DeleteBehavior.Cascade); // Cascade delete: if Aircraft is deleted, delete its SeatingConfig
        
        // Relationship: Flight -> Aircraft (One-to-Many)
        modelBuilder.Entity<Flight>()
                .HasOne(f => f.Aircraft) // Each Flight belongs to one Aircraft
                .WithMany(a => a.Flights) // Each Aircraft can have many Flights
                .HasForeignKey(f => f.AircraftId) // Foreign key in Flight
                .OnDelete(DeleteBehavior.Cascade); // Cascade delete: if Aircraft is deleted, delete its Flights

            // Relationship: Flight -> Destination (DepartureDestination, One-to-Many)
            modelBuilder.Entity<Flight>()
                .HasOne(f => f.DepartureDestination) // Each Flight has one DepartureDestination
                .WithMany(d => d.DepartureFlights) // Each Destination can be the departure for many Flights
                .HasForeignKey(f => f.DepartureDestinationId) // Foreign key in Flight
                .OnDelete(DeleteBehavior.Cascade); // Cascade delete: if Destination is deleted, delete its DepartureFlights

            // Relationship: Flight -> Destination (ArrivalDestination, One-to-Many)
            modelBuilder.Entity<Flight>()
                .HasOne(f => f.ArrivalDestination) // Each Flight has one ArrivalDestination
                .WithMany(d => d.ArrivalFlights) // Each Destination can be the arrival for many Flights
                .HasForeignKey(f => f.ArrivalDestinationId) // Foreign key in Flight
                .OnDelete(DeleteBehavior.Cascade); // Cascade delete: if Destination is deleted, delete its ArrivalFlights
        }
    }
}