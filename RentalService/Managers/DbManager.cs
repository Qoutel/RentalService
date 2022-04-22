using Microsoft.AspNetCore.Identity;
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
        public List<Rent> GetRents()
        {
            return _dbContext.Rent.Include(r => r.AdditionalServices)
                .Include(r => r.Vehicle).ThenInclude(v => v.FuelType)
                .Include(r => r.Vehicle).ThenInclude(v => v.VehicleType)
                .Include(r => r.Vehicle).ThenInclude(v => v.Location)
                .Include(r => r.Vehicle).ThenInclude(v => v.VehicleClass)
                .Include(r => r.Vehicle).ThenInclude(v => v.Brand)
                .Include(r => r.Vehicle).ThenInclude(v => v.Photos).ToList();
        }
        public List<RentsHistory> GetRentsHistory()
        {
            return _dbContext.RentsHistory.Include(r => r.AdditionalServices)
                .Include(r => r.Vehicle).ThenInclude(v => v.FuelType)
                .Include(r => r.Vehicle).ThenInclude(v => v.VehicleType)
                .Include(r => r.Vehicle).ThenInclude(v => v.Location)
                .Include(r => r.Vehicle).ThenInclude(v => v.VehicleClass)
                .Include(r => r.Vehicle).ThenInclude(v => v.Brand)
                .Include(r => r.Vehicle).ThenInclude(v => v.Photos).ToList();
        }
        public Vehicle? GetVehicleById(int vehicleId)
        {
            Vehicle? vehicle = _dbContext.Vehicle.Include(m => m.Brand).Include(m => m.VehicleType)
            .Include(m => m.VehicleClass).Include(m => m.Location).Include(m => m.FuelType).Include(m => m.Photos)
            .Where(v => v.Id == vehicleId).FirstOrDefault();
            return vehicle;
        }
        public Rent? GetRentById(int rentId)
        {
            Rent? rent = _dbContext.Rent.Include(r => r.AdditionalServices)
                .Include(r => r.Vehicle).ThenInclude(v => v.FuelType)
                .Include(r => r.Vehicle).ThenInclude(v => v.VehicleType)
                .Include(r => r.Vehicle).ThenInclude(v => v.Location)
                .Include(r => r.Vehicle).ThenInclude(v => v.VehicleClass)
                .Include(r => r.Vehicle).ThenInclude(v => v.Brand)
                .Include(r => r.Vehicle).ThenInclude(v => v.Photos).Where(r => r.Id == rentId).FirstOrDefault();
            return rent;
        }
        public Location? GetLocationById(int locationId)
        {
            Location? location = _dbContext.Location.Where(l => l.Id == locationId).FirstOrDefault();
            return location;
        }
        public AdditionalService? GetAdditionalServiceById(int additionalServiceId)
        {
            AdditionalService? service = _dbContext.AdditionalService.Where(ad => ad.Id == additionalServiceId).FirstOrDefault();
            return service;
        }
        public VehicleType? GetVehicleTypeById(int vehicleTypeId)
        {
            VehicleType? vehicleType = _dbContext.VehicleType.Where(vt => vt.Id == vehicleTypeId).FirstOrDefault();
            return vehicleType;
        }
        public VehicleClassification? GetVehicleClassificationById(int vehicleClassId)
        {
            VehicleClassification vehicleClassification = _dbContext.VehicleClassification.Where(vc => vc.Id == vehicleClassId).FirstOrDefault();
            return vehicleClassification;
        }
        public VehicleBrand? GetVehicleBrandById(int vehicleBrandId)
        {
            VehicleBrand? vehicleBrand = _dbContext.VehicleBrand.Where(vb => vb.Id == vehicleBrandId).FirstOrDefault();
            return vehicleBrand;
        }
        public FuelType? GetFuelTypeById(int fuelTypeId)
        {
            FuelType? fuelType = _dbContext.FuelType.Where(ft => ft.Id == fuelTypeId).FirstOrDefault();
            return fuelType;
        }
        public VehiclePhoto? GetVehiclePhotoById(int vehiclePhotoId)
        {
            VehiclePhoto? vehiclePhoto = _dbContext.VehiclePhoto.Where(p => p.Id == vehiclePhotoId).FirstOrDefault();
            return vehiclePhoto;
        }
        public void AddRent(Rent rent)
        {
            if (rent != null)
            {
                _dbContext.Rent.Add(rent);
                _dbContext.SaveChanges();
            }
        }
        public void AddVehicle(Vehicle vehicle)
        {
            if (vehicle != null)
            {
                _dbContext.Vehicle.Add(vehicle);
                _dbContext.SaveChanges();
            }
        }
        public void AddVehicleBrand(VehicleBrand vehicleBrand)
        {
            if (vehicleBrand != null)
            {
                _dbContext.VehicleBrand.Add(vehicleBrand);
                _dbContext.SaveChanges();
            }
        }
        public void AddVehicleClassification(VehicleClassification vehicleClassification)
        {
            if (vehicleClassification != null)
            {
                _dbContext.VehicleClassification.Add(vehicleClassification);
                _dbContext.SaveChanges();
            }
        }
        public void AddAdditionalService(AdditionalService additionalService)
        {
            if (additionalService != null)
            {
                _dbContext.AdditionalService.Add(additionalService);
                _dbContext.SaveChanges();
            }
        }
        public void AddLocation(Location location)
        {
            if (location != null)
            {
                _dbContext.Location.Add(location);
                _dbContext.SaveChanges();
            }
        }
        public void AddFuelType(FuelType fuelType)
        {
            if (fuelType != null)
            {
                _dbContext.FuelType.Add(fuelType);
                _dbContext.SaveChanges();
            }
        }
        public void RemoveVehicle(Vehicle vehicle)
        {
            if (vehicle != null)
            {
                _dbContext.Vehicle.Remove(vehicle);
                _dbContext.SaveChanges();
            }
        }
        public void RemoveVehiclePhoto(VehiclePhoto vehiclePhoto)
        {
            if (vehiclePhoto != null)
            {
                _dbContext.VehiclePhoto.Remove(vehiclePhoto);
                _dbContext.SaveChanges();
            }
        }
        public void UpdateVehicle(Vehicle vehicle)
        {
            if (vehicle != null)
            {
                _dbContext.Vehicle.Update(vehicle);
                _dbContext.SaveChanges();
            }
        }
        public void UpdateAdditionalService(AdditionalService additionalService)
        {
            if (additionalService != null)
            {
                _dbContext.AdditionalService.Update(additionalService);
                _dbContext.SaveChanges();
            }
        }
    }
}
