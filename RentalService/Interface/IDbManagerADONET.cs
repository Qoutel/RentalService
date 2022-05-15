using RentalService.Models;
using RentalService.ViewModels;

namespace RentalService.Interface
{
    public interface IDbManagerADONET
    {
        public Task<List<User>> GetUsers();
        public Task<List<Vehicle>> GetVehicles();
        public Task<List<VehicleType>> GetVehicleTypes();
        public Task<List<VehicleBrand>> GetVehicleBrands();
        public Task<List<VehicleClassification>> GetVehicleClassifications();
        public Task<List<FuelType>> GetFuelTypes();
        public Task<List<Location>> GetLocations();
        public Task<List<AdditionalService>> GetAdditionalServices();
        public Task<List<Rent>> GetRents();
        public Task<User> GetUserById(int userId);
        public Task<Vehicle?> GetVehicleById(int vehicleId);
        public Task<Rent?> GetRentById(int rentId);
        public Task<FuelType?> GetFuelTypeById(int fuelTypeId);
        public Task<Location?> GetLocationById(int locationId);
        public Task<AdditionalService?> GetAdditionalServiceById(int additionalServiceId);
        public Task<VehicleType?> GetVehicleTypeById(int vehicleTypeId);
        public Task<VehicleClassification?> GetVehicleClassificationById(int vehicleClassId);
        public Task<VehicleBrand?> GetVehicleBrandById(int vehicleBrandId);
        public Task<bool> AddRent(Rent rent);
        public Task<bool> AddVehicle(Vehicle vehicle);
        public Task<bool> AddVehicleBrand(VehicleBrand vehicleBrand);
        public Task<bool> AddVehicleClassification(VehicleClassification vehicleClassification);
        public Task<bool> AddAdditionalService(AdditionalService additionalService);
        public Task<bool> AddLocation(Location location);
        public Task<bool> AddFuelType(FuelType fuelType);
        public Task<bool> RemoveVehicle(Vehicle vehicle);
        public Task<bool> UpdateVehicle(Vehicle vehicle);
        public Task<bool> UpdateAdditionalService(AdditionalService additionalService);
    }
}
