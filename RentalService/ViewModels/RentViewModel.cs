using RentalService.Models;
using System.ComponentModel.DataAnnotations;

namespace RentalService.ViewModels
{
    public class RentViewModel
    {
        [Required]
        [Display(Name = "Submission date")]
        [DataType(DataType.Date)]
        public DateTime SubmissionDate { get; set; } = DateTime.Now;
        [Required]
        [Display(Name = "Return date")]
        [DataType(DataType.Date)]
        public DateTime ReturnDate { get; set; } = DateTime.Now;
        public int VehicleId { get; set; }
        public decimal RentAmount { get; set; }
        public List<AdditionalService> AdditionalServices { get; set; } = new List<AdditionalService>();
    }
}
