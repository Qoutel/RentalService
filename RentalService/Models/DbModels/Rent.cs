namespace RentalService.Models
{
    public class Rent
    {
        public int Id { get; set; }
        public User? Customer { get; set; }
        public Vehicle Vehicle { get; set; }
        public DateTime RentStartDay { get; set; }
        public DateTime RentEndDay { get; set; }
        public decimal RentAmount { get; set; }
        public List<AdditionalService> AdditionalServices { get; set; } = new List<AdditionalService>();
    }
}
