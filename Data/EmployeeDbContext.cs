using Microsoft.EntityFrameworkCore;
using UserAuth.Models;

namespace UserAuth.Data
{

    public class EmployeeDbContext:DbContext
    {

     public EmployeeDbContext(DbContextOptions<EmployeeDbContext> options)
        : base(options)
        {

        }

     public DbSet<Employee> Employees
        {
            get; set;

        }
    }

}
