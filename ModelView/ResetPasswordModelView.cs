using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace UserAuth.ModelView
{
    public class ResetPasswordModelView
    {

        [Required(ErrorMessage = "Email address is required. ")]
        [EmailAddress(ErrorMessage = "Please enter a valid email.. ")]
        public string Email { get; set; }


        [Required(ErrorMessage = "Password is required. ")]
        [DataType(DataType.Password, ErrorMessage = "Invalid Password format. ")]
        public string Password { get; set; }



        [Required(ErrorMessage = "Password is required. ")]
        [DataType(DataType.Password, ErrorMessage = "Invalid Password format. ")]
        [DisplayName("Confirm Password")]
        [Compare("Password", ErrorMessage = "Password must Match.")]
        public string ConfirmPassword { get; set; }


        [Required(ErrorMessage = "The password reset token is required..")]
        public string Token { get; set; }
    }
}
