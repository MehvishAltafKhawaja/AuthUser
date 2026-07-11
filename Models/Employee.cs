using System.ComponentModel.DataAnnotations;

namespace UserAuth.Models
{
    public class Employee
    {
      
            [Key]
            public int Id { get; set; }

            public string Name { get; set; }

            public string Email { get; set; }

            public string ImageUrl { get; set; }

            public string Address { get; set; }

            public string PhoneNumber { get; set; }

            public string Qualification { get; set; }

    }
}
