using System.ComponentModel.DataAnnotations;

namespace UserAuth.ModelView
{
    public class EmployeeModelView
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage ="Please Enter Your Name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Please Enter Your Email")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Upload Your Photo")]
        public IFormFile ImageUrl { get; set; }

        [Required(ErrorMessage = "Please Enter Address")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Please Enter Your Contact Number")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "We need your qualification to register your account!")]
        public string Qualification { get; set; }
    }
}
