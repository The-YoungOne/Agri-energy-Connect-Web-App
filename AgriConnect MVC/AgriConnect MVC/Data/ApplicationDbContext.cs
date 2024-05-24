using AgriConnect_MVC.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace AgriConnect_MVC.Data
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        //model contexts:
        public DbSet<EmployeeModel> Employees { get; set; }
        public DbSet<FarmerModel> Farmers { get; set; }
        public DbSet<ProductModel> Products { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        //model creating method for linking tables
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); // This is important to call to configure the identity model

            //makes FarmerId a foreign key in Products table
            modelBuilder.Entity<ProductModel>()
                .HasOne(m => m.Farmer)
                .WithMany(s => s.Products)
                .HasForeignKey(m => m.FarmerId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
