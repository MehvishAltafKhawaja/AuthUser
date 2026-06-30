using System.ComponentModel.DataAnnotations;

namespace UserAuth.ModelView
{
    public class RegistraViewModel
    {
        [Required(ErrorMessage ="Please Enter Your Name..")]
        public string Name { get; set; }

        [EmailAddress(ErrorMessage ="Please Enter a Valid email..")]
        public string Email { get; set; }

        [Required(ErrorMessage="Please Enter Password..")]
        [StringLength(40,MinimumLength =8 , ErrorMessage ="The {0}  must be at {2} and at max {1} character long")]
        [Compare("ConfirmPassword",ErrorMessage ="Password Does Not Match!..")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage ="Please Enter Password")]
        public string ConfirmPassword { get; set;}
    }
}
