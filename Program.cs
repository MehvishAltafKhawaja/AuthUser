using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using UserAuth.Data;
using UserAuth.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<UserdbContext>(x => x.UseSqlServer(builder.Configuration.GetConnectionString("constr")));
builder.Services.AddIdentity<Users, IdentityRole>(option =>
{
    option.Password.RequiredLength = 8;
    option.Password.RequireUppercase = true;
    option.Password.RequireNonAlphanumeric = true;
    option.Password.RequireLowercase = true;
    option.Password.RequireDigit = true;
    option.User.RequireUniqueEmail = true;
    option.SignIn.RequireConfirmedEmail = false;
    option.SignIn.RequireConfirmedPhoneNumber = false;
    option.SignIn.RequireConfirmedAccount = false;
    
    }).AddEntityFrameworkStores<UserdbContext>().AddDefaultTokenProviders();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
