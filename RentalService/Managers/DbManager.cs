using Microsoft.EntityFrameworkCore;
using RentalService.Interface;
using RentalService.Models;
using RentalService.ViewModels;

namespace RentalService.Managers
{
    public class DbManager : IDbManager
    {
        readonly ApplicationDbContext _dbContext;
        public DbManager(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public List<Vehicle> GetVehicles()
        {
            var vehicles = _dbContext.Vehicle.Include(m => m.Brand).Include(m => m.VehicleType)
            .Include(m => m.VehicleClass).Include(m => m.Location).Include(m => m.FuelType).Include(m => m.Photos).ToList();
            return vehicles;
        }
        public List<FuelType> GetFuelTypes()
        {
            var fuelTypes = _dbContext.FuelType.ToList();
            return fuelTypes;
        }
        public List<VehicleType> GetVehicleTypes()
        {
            var vehicleTypes = _dbContext.VehicleType.ToList();
            return vehicleTypes;
        }
        public List<VehicleBrand> GetVehicleBrands()
        {
            var vehicleBrands = _dbContext.VehicleBrand.ToList();
            return vehicleBrands;
        }
        public List<VehicleClassification> GetVehicleClassifications()
        {
            var vehicleClass = _dbContext.VehicleClassification.ToList();
            return vehicleClass;
        }
        public List<Location> GetLocations()
        {
            var locations = _dbContext.Location.ToList();
            return locations;
        }
        public List<AdditionalService> GetAdditionalServices()
        {
            return _dbContext.AdditionalService.ToList();
        }
    }
}
