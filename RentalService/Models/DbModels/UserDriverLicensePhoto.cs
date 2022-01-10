namespace RentalService.Models
{
    public class UserDriverLicensePhoto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public User User { get; set; }
    }
}
