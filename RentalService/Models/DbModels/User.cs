using Microsoft.AspNetCore.Identity;

namespace RentalService.Models
{
    public class User: IdentityUser
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string? Adress { get; set; }
        public bool IsEmailConfirmed { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public List<Rent> CurrentLeases { get; set; } = new List<Rent>();
        public List<RentsHistory> PastLeases { get; set; } = new List<RentsHistory>();
        public List<UserPassportPhoto> PassportPhotos { get; set; } = new List<UserPassportPhoto>();
        public List<UserDriverLicensePhoto> DriverLicensePhotos { get; set; } = new List<UserDriverLicensePhoto>();
        public List<UserIdentificationCodePhoto> IdentificationCodePhoto { get; set; } = new List<UserIdentificationCodePhoto>();
    }
}
