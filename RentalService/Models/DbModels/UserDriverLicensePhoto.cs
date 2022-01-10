namespace RentalService.Models
{
    public class UserDriverLicensePhoto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public byte[] Photo { get; set; }
        public User User { get; set; }
    }
}
