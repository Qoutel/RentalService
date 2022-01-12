using RentalService.Models;

namespace RentalService.ViewModels
{
    public class CabinetViewModel
    {
        public User User { get; set; }
        public List<UserPassportPhoto> PassportPhoto { get; set; } = new List<UserPassportPhoto>();
    }
}
