using RentalService.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace RentalService.ViewModels
{
    public class EditVehicleViewModel
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public int YearOfManufactured { get; set; }
        [Required]
        public int Mileage { get; set; }
        [Required]
        public decimal PricePerDay { get; set; }
        public bool IsAvailable { get; set; } = true;
        public int FuelTypeId { get; set; }
        public int VehicleTypeId { get; set; }
        [Required]
        public int NumberOfSeats { get; set; }
        public bool AutomaticTransmission { get; set; }
        public int LocationId { get; set; }
        public int VehicleClassId { get; set; }
        public int BrandId { get; set; }
        public IFormFile? Photo { get; set; }
    }
}
