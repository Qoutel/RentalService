using Microsoft.AspNetCore.Mvc.Rendering;
using RentalService.Models;

namespace RentalService.ViewModels
{
    public class VehicleClassListViewModel
    {
        public IEnumerable<VehicleClassification> VehicleClasses { get; set; }
        public IEnumerable<VehicleType> VehicleTypes { get; set; }
        public SelectList? VehicleTypesFilter { get; set; }
    }
}
