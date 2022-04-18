using RentalService.Models;
using RentalService.ViewModels;

namespace RentalService.Interface
{
    public interface IDbManager
    {
        public List<Vehicle> GetVehicles();
        public List<VehicleType> GetVehicleTypes();
        public List<VehicleBrand> GetVehicleBrands();
        public List<VehicleClassification> GetVehicleClassifications();
        public List<FuelType> GetFuelTypes();
        public List<Location> GetLocations();
        public List<AdditionalService> GetAdditionalServices();
    }
}
