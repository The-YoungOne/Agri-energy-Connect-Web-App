using AgriConnect_MVC.Controllers;
using AgriConnect_MVC.Data;
using AgriConnect_MVC.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

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

// Bypass SSL certificate validation for development or testing environment
if (app.Environment.IsDevelopment())
{
    ServicePointManager.ServerCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;
}

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
using (var scope = app.Services.CreateScope())
{
    var roleManger =
        scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

    var roles = new[] { "Employee", "Farmer" };

    foreach (var role in roles)
    {
        if (!await roleManger.RoleExistsAsync(role))
        {
            await roleManger.CreateAsync(new IdentityRole(role));
        }
    }
}

//creates the first default admin user.
using (var scope = app.Services.CreateScope())
{
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

    //default employee user
    string email = "employee@employee.com";
    string password = "Admin1234#";

    if (await userManager.FindByEmailAsync(email) == null)
    {
        var user = new IdentityUser { UserName = email, Email = email, EmailConfirmed = true };
        await userManager.CreateAsync(user, password);
        await userManager.AddToRoleAsync(user, "Employee");
    }

    // Create default farmer user
    string farmerEmail = "john@doefarming.com";
    string farmerPassword = "Farmer1234#";

    if (await userManager.FindByEmailAsync(farmerEmail) == null)
    {
        var user = new IdentityUser { UserName = farmerEmail, Email = farmerEmail, EmailConfirmed = true };
        await userManager.CreateAsync(user, farmerPassword);
        await userManager.AddToRoleAsync(user, "Farmer");

        // Add Farmer details
        var farmer = new FarmerModel
        {
            FarmerId = 0,
            Name = "John",
            Surname = "Doe",
            Email = "john@doefarming.com",
            Number = "1234567890",
            Approved = "Yes"
        };

        context.Farmers.Add(farmer);
        await context.SaveChangesAsync();

        // Add products for the farmer
        var products = new[]
        {
            new ProductModel { Name = "Carrots", Category = ProductCategory.Vegetable, Quantity = 100, ProductionDate = DateTime.Now.AddDays(-10), ImageUrl = "https://images.pexels.com/photos/3650647/pexels-photo-3650647.jpeg?auto=compress&cs=tinysrgb&w=600", FarmerId = farmer.FarmerId },
            new ProductModel { Name = "Chicken Eggs", Category = ProductCategory.Protein, Quantity = 50, ProductionDate = DateTime.Now.AddDays(-5), ImageUrl = "https://images.pexels.com/photos/4207676/pexels-photo-4207676.jpeg?auto=compress&cs=tinysrgb&w=600", FarmerId = farmer.FarmerId },
            new ProductModel { Name = "Bananas", Category = ProductCategory.Fruit, Quantity = 200, ProductionDate = DateTime.Now.AddDays(-2), ImageUrl = "https://images.pexels.com/photos/47305/bananas-banana-shrub-fruits-yellow-47305.jpeg?auto=compress&cs=tinysrgb&w=600", FarmerId = farmer.FarmerId }
        };

        context.Products.AddRange(products);
        await context.SaveChangesAsync();
    }
}

app.Run();
