using System.ComponentModel.DataAnnotations;

namespace RentalService.ViewModels
{
    public class ChangePasswordViewModel
    {
        public string Id { get; set; }
        public string Email { get; set; }
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }
        [DataType(DataType.Password)]
        public string OldPassword { get; set; }
    }
}
