using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using UserAuth.Models;

namespace UserAuth.Data
{
    public class UserdbContext:IdentityDbContext<Users>
    {
        public UserdbContext(DbContextOptions<UserdbContext> options)
    : base(options)
        {

        }
    }
}
