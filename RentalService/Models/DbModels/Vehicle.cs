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
        public bool IsAvailable { get; set; } = true;
        public FuelType FuelType { get; set; } = new FuelType();
        public VehicleType VehicleType { get; set; } =  new VehicleType();
        public int NumberOfSeats { get; set; }
        public bool AutomaticTransmission { get; set; }
        public Location Location { get; set; } = new Location();
        public VehicleClassification VehicleClass { get; set; } = new VehicleClassification();
        public VehicleBrand Brand { get; set; } = new VehicleBrand();
        public List<VehiclePhoto> Photos { get; set; } = new List<VehiclePhoto>();
    }
}
