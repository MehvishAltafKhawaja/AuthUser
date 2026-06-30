using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace UserAuth.ModelView
{
    public class LoginViewModel
    {
        [EmailAddress(ErrorMessage ="Please enter email!")]
        [DisplayName("Email Id")]
        public string Email { get; set;}
        [Required(ErrorMessage ="Please entry your password")]
        [DataType(DataType.Password)]
        public string Password { get; set;}

        [DisplayName("Remember Me")]
        public bool Rememberme { get; set;}

    }
}
