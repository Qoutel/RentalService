using RentalService.Models;

namespace RentalService.Interface
{
    public interface IDbManager
    {
        public List<Vehicle> GetVehicle();
    }
}
