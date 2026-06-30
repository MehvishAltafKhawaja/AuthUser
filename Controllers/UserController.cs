using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using UserAuth.helper;
using UserAuth.Models;
using UserAuth.ModelView;

namespace UserAuth.Controllers
{
    public class UserController : Controller
    {
        public readonly UserManager<Users> userManager;
        public readonly SignInManager<Users> signInManager;

        public UserController(UserManager<Users> userManager, SignInManager<Users> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegistraViewModel model)
        {
            if(ModelState.IsValid)
            {
                Users u = new Users()
                {
                   Name = model.Name,
                   Email = model.Email,
                   NormalizedEmail = model.Email,
                   UserName = model.Email,
                   NormalizedUserName = model.Email,

                };
                var result = await userManager.CreateAsync(u, model.Password);
                if(result.Succeeded)
                {   SandMail mail = new SandMail();
                    string msg = "Dear" + model.Name + ", <br/><br/> You Have Successfully Registered on ILS Srinagar <br/><b> Your Userid : " + model.Email + "<br/> Password : " + model.Password +"</b><br/><br/> Regards, <br/> <font color='Blue' size='5px'> Ils Srinagar</font>";
                    mail.SendMail(model.Email,"Ils Registeration Completed", msg);
                    TempData["message"] = "Mail Sand Successfully";
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    foreach(var err in result.Errors)
                    {
                        ModelState.AddModelError("", err.Description);
                    }
                }
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await signInManager.PasswordSignInAsync(model.Email, model.Password, model.Rememberme, false);
                if(result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Invalid Email and Password...");
                }
            }
            return View(model);
               
        }

        public async Task<IActionResult> Logout()
        {
            if(signInManager.IsSignedIn(User))
            {
                await signInManager.SignOutAsync();
                return RedirectToAction("Login");
            }
            return NotFound();
        }


        public async Task<IActionResult>ChangePassword()
        {
            if (signInManager.IsSignedIn(User))
            {
                var user = await userManager.Users.FirstOrDefaultAsync(x => x.Email == userManager.GetUserName(User));
                if(user!=null)
                {
                    ViewData["Email"] = user.UserName;
                }
                return View();
            }
            return RedirectToAction("Login");
        }
    }
}

