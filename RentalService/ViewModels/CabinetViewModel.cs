using RentalService.Models;

namespace RentalService.ViewModels
{
    public class CabinetViewModel
    {
        public User User { get; set; }
        public List<UserPassportPhoto> PassportPhoto { get; set; } = new List<UserPassportPhoto>();
        public List<UserDriverLicensePhoto> DriversLicensePhoto { get; set; } = new List<UserDriverLicensePhoto>();
        public List<UserIdentificationCodePhoto> IdentificationCodePhoto { get; set; } = new List<UserIdentificationCodePhoto>();
    }
}
