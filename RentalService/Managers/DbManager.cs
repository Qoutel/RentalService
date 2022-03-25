using Microsoft.EntityFrameworkCore;
using RentalService.Interface;
using RentalService.Models;

namespace RentalService.Managers
{
    public class DbManager : IDbManager
    {
        readonly ApplicationDbContext _dbContext;
        public DbManager(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public List<Vehicle> GetVehicle()
        {
            var vehicle = _dbContext.Vehicle.Include(m => m.Brand).Include(m => m.VehicleType)
            .Include(m => m.VehicleClass).Include(m => m.Location).Include(m => m.FuelType).Include(m => m.Photos).ToList();
            return vehicle;
        }
    }
}
