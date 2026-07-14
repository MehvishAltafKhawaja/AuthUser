using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using UserAuth.Data;
using UserAuth.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<UserdbContext>(x => x.UseSqlServer(builder.Configuration.GetConnectionString("constr")));
builder.Services.AddDbContext<StudentSearchContext>(x => x.UseSqlServer(builder.Configuration.GetConnectionString("constr2")));
builder.Services.AddDbContext<EmployeeDbContext>(x => x.UseSqlServer(builder.Configuration.GetConnectionString("constr3")));
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
builder.Services.Configure<DataProtectionTokenProviderOptions>(options =>
{
    options.TokenLifespan = TimeSpan.FromSeconds(40);
});
builder.Services.PostConfigure<DataProtectionTokenProviderOptions>(options =>
{
    Console.WriteLine($"Configured Token Lifetime: {options.TokenLifespan}");
});
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
app.UseAuthentication();
app.UseAuthorization();
using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider
        .GetRequiredService<RoleManager<IdentityRole>>();

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

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
