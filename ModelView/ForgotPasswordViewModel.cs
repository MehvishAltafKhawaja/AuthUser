using System.ComponentModel.DataAnnotations;

namespace UserAuth.ModelView
{
    public class ForgotPasswordViewModel
    {
        [Required(ErrorMessage = "Please Enter your Email")]
        [EmailAddress(ErrorMessage = "Please Enter a Valid email..")]
        public string Email { get; set; }
    }
}
