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
        public List<Rent> GetRents();
        public List<RentsHistory> GetRentsHistory();
        public Vehicle? GetVehicleById(int vehicleId);
        public Rent? GetRentById(int rentId);
        public FuelType? GetFuelTypeById(int fuelTypeId);
        public Location? GetLocationById(int locationId);
        public AdditionalService? GetAdditionalServiceById(int additionalServiceId);
        public VehicleType? GetVehicleTypeById(int vehicleTypeId);
        public VehicleClassification? GetVehicleClassificationById(int vehicleClassId);
        public VehicleBrand? GetVehicleBrandById(int vehicleBrandId);
        public VehiclePhoto? GetVehiclePhotoById(int vehiclePhotoId);
        public void AddRent(Rent rent);
        public void AddVehicle(Vehicle vehicle);
        public void AddVehicleBrand(VehicleBrand vehicleBrand);
        public void AddVehicleClassification(VehicleClassification vehicleClassification);
        public void AddAdditionalService(AdditionalService additionalService);
        public void AddLocation(Location location);
        public void AddFuelType(FuelType fuelType);
        public void RemoveVehicle(Vehicle vehicle);
        public void RemoveVehiclePhoto(VehiclePhoto vehiclePhoto);
        public void UpdateVehicle(Vehicle vehicle);
        public void UpdateAdditionalService(AdditionalService additionalService);
    }
}
