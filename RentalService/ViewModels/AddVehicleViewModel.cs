using RentalService.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace RentalService.ViewModels
{
    public class AddVehicleViewModel
    {
        public Vehicle Vehicle { get; set; }
        public SelectList VehicleType { get; set; }
        public VehicleClassification VehicleClassification { get; set; }
        public Location Location { get; set; }
        public VehicleBrand VehicleBrand { get; set; }
        public FuelType FuelType { get; set; }
    }
}
