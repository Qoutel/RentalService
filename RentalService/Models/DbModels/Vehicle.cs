using System.ComponentModel.DataAnnotations;
namespace RentalService.Models
{
    public class Vehicle
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int YearOfManufactured { get; set; }
        public int Mileage { get; set; }
        public decimal PricePerDay { get; set; }
        public bool IsAvailable { get; set; }
        public VehicleType? VehicleType { get; set; }
        public FuelType? FuelType { get; set; }
        public int NumberOfSeats { get; set; }
        public bool AutomaticTransmission  { get; set; }
        public Location Location { get; set; }
        public VehicleClassification VehicleClass { get; set; }
    }
}
