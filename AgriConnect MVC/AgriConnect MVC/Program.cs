using AgriConnect_MVC.Controllers;
using AgriConnect_MVC.Data;
using AgriConnect_MVC.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days.
    // You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

//adds roles to the program
using(var scope = app.Services.CreateScope())
{
    var roleManger=
        scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

    var roles = new[] {"Employee", "Farmer"};

    foreach (var role in roles)
    {
        if(!await roleManger.RoleExistsAsync(role))
        {
            await roleManger.CreateAsync(new IdentityRole(role));
        }
    }
}

//creates the first default admin user.
using (var scope = app.Services.CreateScope())
{
    var userManger =
        scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

    string email = "employee@employee.com";
    string password = "Admin1234#";

    if (await userManger.FindByEmailAsync(email) == null)
    {
        var user = new IdentityUser { UserName = email, Email = email, EmailConfirmed = true };

        await userManger.CreateAsync(user,password);

        await userManger.AddToRoleAsync(user, "Employee");
    }
}

app.Run();
