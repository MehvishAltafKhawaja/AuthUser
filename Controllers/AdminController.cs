using Microsoft.AspNetCore.Authorization;
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
         
        public AdminController(EmployeeDbContext context, IWebHostEnvironment env)
        {
            this.context = context;
            this.env = env;
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
       

     


    }
}
