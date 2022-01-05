using Microsoft.AspNetCore.Identity;
namespace RentalService.Models
{
    public class User: IdentityUser
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public int Year { get; set; }
        public string? Adress { get; set; }
        public bool IsEmailConfirmed { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public List<Rent>? CurrentLeases { get; set; }
        public List<RentsHistory>? PastLeases { get; set; }
    }
}
