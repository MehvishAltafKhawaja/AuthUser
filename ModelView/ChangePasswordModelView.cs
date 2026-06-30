using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace UserAuth.ModelView
{
    public class ChangePasswordModelView
    {

        

        [EmailAddress(ErrorMessage = "Please Enter a Valid email..")]
        public string Email { get; set; }

        [Required(ErrorMessage ="Please Enter your old password")]
        [DisplayName("Current Password")]
        [DataType(DataType.Password)]
        public string CurrentPassword { get; set; }

        [Required(ErrorMessage = "Please Enter New Password..")]
        [StringLength(40, MinimumLength = 8, ErrorMessage = "The {0}  must be at {2} and at max {1} character long")]
        [Compare("ConfirmPassword", ErrorMessage = "Password Does Not Match!..")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Please Enter Password")]
        public string ConfirmNewPassword { get; set; }
    }
}
