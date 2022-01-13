using Microsoft.EntityFrameworkCore;

namespace RentalService.Models
{
    public class ApplicationDbContext: DbContext
    {
        public DbSet<UserPassportPhoto>? UserPassportPhoto { get; set; }
        public DbSet<UserDriverLicensePhoto>? UserDriverLicensePhoto { get; set;}
        public DbSet<UserIdentificationCodePhoto>? UserIdentificationCodePhoto { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }
    }
}

