using RentalService.Models;
namespace RentalService.ViewModels
{
    public class HomePageViewModel
    {
        public IEnumerable<Vehicle> Vehicles { get; set; }
        public PageViewModel PageViewModel { get; set; }
        public List<VehicleBrand> VehicleBrands { get; set; }
        public bool BMW { get; set; }
    }
}
