namespace RentalService.Models
{
    public class VehicleClassification
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public VehicleType VehicleType { get; set; }
    }
}
