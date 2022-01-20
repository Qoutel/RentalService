using Microsoft.EntityFrameworkCore;

namespace RentalService.Models
{
    public class ApplicationDbContext: DbContext
    {
        public DbSet<UserPassportPhoto>? UserPassportPhoto { get; set; }
        public DbSet<UserDriverLicensePhoto>? UserDriverLicensePhoto { get; set;}
        public DbSet<UserIdentificationCodePhoto>? UserIdentificationCodePhoto { get; set; }
        public DbSet<AdditionalService> AdditionalService { get; set; }
        public DbSet<FuelType> FuelType { get; set; }
        public DbSet<Location> Location { get; set; }
        public DbSet<Rent> Rent { get; set; }
        public DbSet<RentsHistory> RentsHistory { get; set; }
        public DbSet<Vehicle> Vehicle { get; set; }
        public DbSet<VehicleClassification> VehicleClassification { get; set; }
        public DbSet<VehiclePhoto> VehiclePhoto { get; set;}
        public DbSet<VehicleType> VehicleType { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
    }
}

