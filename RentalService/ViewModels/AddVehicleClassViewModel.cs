using Microsoft.AspNetCore.Mvc.Rendering;
using RentalService.Models;
using System.ComponentModel.DataAnnotations;

namespace RentalService.ViewModels
{
    public class AddVehicleClassViewModel
    {
        public int VehicleTypeId { get; set; }
        [Required]
        public string ClassName { get; set; }
    }
}
