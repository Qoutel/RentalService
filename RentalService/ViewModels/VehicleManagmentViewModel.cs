using Microsoft.AspNetCore.Mvc.Rendering;
using RentalService.Models;
using System.Collections.Generic;

namespace RentalService.ViewModels
{
    public class VehicleManagmentViewModel
    {
        public IEnumerable<Vehicle> Vehicles { get; set; }
        public SelectList FuelTypes { get; set; }
        public SelectList VehicleTypes { get; set; }
        public SelectList Locations { get; set; }
        public SelectList VehicleBrands { get; set; }
    }
}
