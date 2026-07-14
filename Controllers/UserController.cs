using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using Microsoft.SqlServer.Server;
using System.Net;
using System.Text.Encodings.Web;
using UserAuth.helper;
using UserAuth.Models;
using UserAuth.ModelView;
using Microsoft.Extensions.Options;

namespace UserAuth.Controllers
{
    public class UserController : Controller
    {
        public readonly UserManager<Users> userManager;
        public readonly SignInManager<Users> signInManager;
        public readonly RoleManager<IdentityRole> roleManager;

        public UserController(UserManager<Users> userManager, SignInManager<Users> signInManager, RoleManager<IdentityRole> roleManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.roleManager = roleManager;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegistraViewModel model)
        {
            if (ModelState.IsValid)
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
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(u, "Student");
                    SandMail mail = new SandMail();
                    string msg = "Dear" + model.Name + ", <br/><br/> You Have Successfully Registered on ILS Srinagar <br/><b> Your Userid : " + model.Email + "<br/> Password : " + model.Password + "</b><br/><br/> Regards, <br/> <font color='Blue' size='5px'> Ils Srinagar</font>";
                    mail.SendMail(model.Email, "Ils Registeration Completed", msg);
                    TempData["message"] = "Mail Sand Successfully";
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    foreach (var err in result.Errors)
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
                if (result.Succeeded)
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
            if (signInManager.IsSignedIn(User))
            {
                await signInManager.SignOutAsync();
                return RedirectToAction("Login");
            }
            return NotFound();
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> ChangePassword()
        {
            if (signInManager.IsSignedIn(User))
            {
                var user = await userManager.Users.FirstOrDefaultAsync(x => x.Email == userManager.GetUserName(User));
                if (user != null)
                {
                    ViewData["Email"] = user.UserName;
                }
                return View();
            }
            return RedirectToAction("Login");
        }
        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordModelView model)
        {
            var user = await userManager.Users.FirstOrDefaultAsync(x => x.Email == userManager.GetUserName(User));
            if (user != null)
            {
                ViewData["Email"] = user.UserName;
            }
            if (ModelState.IsValid)
            {
                var result = await userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
                if (result.Succeeded)
                {
                    TempData["CCMessage"] = "Password Changed Successfully";
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ViewData["Message"] = "Incorrect Old Password";
                    return View();
                }

            }

            return View(model);
        }




        //---------------------------Forgot Password---------------------

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPassword()
        {
            return View();
        }




        public async Task SendForgotPasswordEmail(string? email, Users? user)
        {

            // generate token for password reset
            var provider = userManager.Options.Tokens.PasswordResetTokenProvider;
            Console.WriteLine($"Password Reset Provider: {provider}");

            var token = await userManager.GeneratePasswordResetTokenAsync(user);

            // create a password reset link
            var passwordResetLink = Url.Action("ResetPassword", "User",
            new { Email = email, Token = token }, protocol: HttpContext.Request.Scheme);

            // Encode the link to prevent XSS attacks
            var safeLink = HtmlEncoder.Default.Encode(passwordResetLink);


            var subject = "Reset Your Password";

            var messageBody = $@"
               <div style=""font-family: Arial, Helvetica, sans-serif; font-size:16px; color: #333;line-height:1.5;padding:20px;""><h2 style="" color: #007bff; text-align: center;"">
                  Reset Password Request </h2>

               <p style="" margin-bottom: 20px; ""> Hi {user.Name},</p>
               <p>We received a request to reset your password for your <strong> Dot Net Tutorials</strong> account. If you mase this request,please click the button below to reset your password: </p>

               <div style=""text-align: center; margin: 20px 0;""> 
                <a href = ""{safeLink}""
                   style = ""background-color: #007bff; color #fff; padding: 10px 20px; text-decoration: none; font-weight: bold; border-radius:5px display: inline-block;"">
                   Reset Password
                </a>
               </div>

               <p>If the button above doesn't work, copy and past the following URL into your browser:</p>
                  <p style=""background-color:#f8f9fa; padding: 10px; border: 1px solid #ddd; border-radius: 5px;"">
                  <a href=""{safeLink}"" style=""color: #007bff; text-decoration: none; "">{safeLink}</a>
               </p>   

               <p> If you did not request to reset your password, please ignore this email or contact suppory if you have concerns. </p>

               <p style=""margin-top: 30px; "">Thank you, <br/> Ils Srinager </p>
            </div>";

            SandMail sand = new SandMail();
            sand.SendMail(email, subject, messageBody);

        }



        //Forgot password
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            // Check if the model passess all the validation rules
            if (ModelState.IsValid)
            {
                // Attempt to find the database whoese email address match the one entered by the user
                var user = await userManager.FindByEmailAsync(model.Email);

                if (user != null)
                {
                    await SendForgotPasswordEmail(user.Email, user);
                    return RedirectToAction("ForgotPasswordConfirmation");
                }
                else
                {
                    ViewData["Message"] = "Email Id does not exists...";
                    return RedirectToAction("ForgotPasswordConfirmation", "User");
                }
            }
            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPasswordConfirmation()
        {
            return View();
        }


        [HttpGet]
        [AllowAnonymous]

        public IActionResult ResetPassword(string Token, string Email)
        {
            if (Token == null || Email == null)
            {
                ViewBag.ErrorTitle = "Invalid Password reset Token";
                ViewBag.ErrorMessage = "Token Expired";
                return View("Error");
            }
            else
            {
                ResetPasswordModelView model = new ResetPasswordModelView();
                model.Token = Token;
                model.Email = Email;
                return View(model);
            }
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword(ResetPasswordModelView model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await userManager.FindByEmailAsync(model.Email);

            if (user == null)
            {
                ModelState.AddModelError("", "Invalid user.");
                return View(model);
            }

            var result = await userManager.ResetPasswordAsync(user, model.Token, model.Password);

            if (result.Succeeded)
            {
                return RedirectToAction("ResetPasswordConfirmation");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            return View(model);
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        public async Task CreateRoles()
        {
            string[] roles =
            {
               "Admin",
               "Employee",
               "Student"
            };


            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }
        }

        public async Task<IActionResult> CreateAdmin()
        {
            Users admin = new Users()
            {
                Name = "Admin",
                Email = "admin@gmail.com",
                UserName = "admin@gmail.com"
            };


            var result = await userManager.CreateAsync(
                admin,
                "Admin@123"
            );


            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(admin, "Admin");
            }


            return Content("Admin Created Successfully");
        }
    }
}


