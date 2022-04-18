namespace RentalService.Models
{
    public class Rent
    {
        public int Id { get; set; }
        public string CustomerId { get; set; }
        public Vehicle Vehicle { get; set; }
        public DateTime SubmissionDate { get; set; }
        public DateTime ReturnDate { get; set; }
        public decimal RentAmount { get; set; }
        public ICollection<AdditionalService> AdditionalServices { get; set; }
        public Rent()
        {
            AdditionalServices = new List<AdditionalService>();
        }
    }
}
