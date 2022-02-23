using RentalService.Models;
namespace RentalService.ViewModels
{
    public class HomePageViewModel
    {
        public IEnumerable<Vehicle> Vehicles { get; set; }
        public PageViewModel PageViewModel { get; set; }
        public List<VehicleBrand> VehicleBrands { get; set; } = new List<VehicleBrand>();
        public List<FuelType> FuelTypes { get; set; } = new List<FuelType>();
        public List<Location> Locations { get; set; } = new List<Location>();
        public List<VehicleType> VehicleTypes { get; set; } = new List<VehicleType>();
    }
}
