using Microsoft.EntityFrameworkCore;

namespace RentalService.Models
{
    public class ApplicationDbContext: DbContext
    {
        public DbSet<UserPassportPhoto>? UserPassportPhoto { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }
    }
}

