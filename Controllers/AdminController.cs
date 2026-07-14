using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UserAuth.Data;
using UserAuth.Models;
using UserAuth.ModelView;


namespace UserAuth.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {   private readonly EmployeeDbContext context;
        private readonly IWebHostEnvironment env;
        private readonly UserManager<Users> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        public AdminController(
            EmployeeDbContext context,
            UserManager<Users> userManager,
            RoleManager<IdentityRole> roleManager,
            IWebHostEnvironment env)
        {
            this.context = context;
            this.env = env;
            this.userManager = userManager;
            this.roleManager = roleManager;
        }

        public async Task<IActionResult> EmployeeRecord()
        {
            List<Employee> emp = await context.Employees.ToListAsync();
            return View(emp);
        }


        //Register Employee
        [HttpGet]
        public IActionResult AddEmployee()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddEmployee(EmployeeModelView model)
        {
            if(ModelState.IsValid)
            {
                if(model.ImageUrl !=null)
                {
                    string fn = Guid.NewGuid().ToString() + "_" + model.ImageUrl.FileName;
                    string folder = Path.Combine(env.WebRootPath, "EmployeeImage");
                    string imagePath = Path.Combine(folder, fn);
                    await model.ImageUrl.CopyToAsync(new FileStream(imagePath, FileMode.Create));

                    Employee emp = new Employee()
                    {
                        Name = model.Name,
                        Email = model.Email,
                        Address = model.Address,
                        PhoneNumber = model.PhoneNumber,
                        Qualification = model.Qualification,
                        ImageUrl = fn
                    };
                    await context.Employees.AddAsync(emp);
                    context.SaveChanges();
                    TempData["success"] = "Employee Record Added Successfully";
                    return RedirectToAction("EmployeeRecord");
                }
                else
                {
                    TempData["error"] = "Please Select Image";
                    return RedirectToAction("Index", "Home");
                }

                
            }
            return View(model);
        }


        //Changing role of  users
        public async Task<IActionResult> Users()
        {
            var users = userManager.Users.ToList();

            return View(users);
        }

        public async Task<IActionResult> ChangeRole(string id)
        {

            var user = await userManager.FindByIdAsync(id);


            if (user == null)
            {
                return NotFound();
            }


            ViewBag.Roles = roleManager.Roles.ToList();


            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> ChangeRole(
          string UserId,
          string Role)
        {

            var user = await userManager.FindByIdAsync(UserId);


            if (user != null)
            {

                var oldRoles = await userManager.GetRolesAsync(user);


                await userManager.RemoveFromRolesAsync(
                    user,
                    oldRoles
                );


                await userManager.AddToRoleAsync(
                    user,
                    Role
                );

            }


            return RedirectToAction("Users");

        }

    }
}
