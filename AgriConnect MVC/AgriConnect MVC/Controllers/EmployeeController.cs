using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AgriConnect_MVC.Data;
using AgriConnect_MVC.Models;
using Microsoft.AspNetCore.Identity;

namespace AgriConnect_MVC.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public EmployeeController(ApplicationDbContext context, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        // GET: Employee
        public async Task<IActionResult> Index()
        {
            return View(await _context.Employees.ToListAsync());
        }

        // GET: Employee/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employeeModel = await _context.Employees
                .FirstOrDefaultAsync(m => m.EmployeeId == id);
            if (employeeModel == null)
            {
                return NotFound();
            }

            return View(employeeModel);
        }

        // GET: Employee/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Employee/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("EmployeeId,Name,Surname,Number,Email,Password,ConfirmPassword")] EmployeeModel employeeModel)
        {
            if (ModelState.IsValid)
            {
                // Create a new IdentityUser
                var user = new IdentityUser { UserName = employeeModel.Email, Email = employeeModel.Email, EmailConfirmed = true };
                var result = await _userManager.CreateAsync(user, employeeModel.Password);

                if (result.Succeeded)
                {
                    // Assign the Employee role
                    if (!await _roleManager.RoleExistsAsync("Employee"))
                    {
                        await _roleManager.CreateAsync(new IdentityRole("Employee"));
                    }

                    await _userManager.AddToRoleAsync(user, "Employee");

                    // Add the employee to the Employee table
                    _context.Add(employeeModel);
                    await _context.SaveChangesAsync();

                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            return View(employeeModel);

            //try
            //{
            //    // Create a new IdentityUser
            //    var user = new IdentityUser { UserName = employeeModel.Email, Email = employeeModel.Email, EmailConfirmed = true };
            //    var result = await _userManager.CreateAsync(user, employeeModel.Password);

            //    if (result.Succeeded)
            //    {
            //        // Assign the Employee role
            //        if (!await _roleManager.RoleExistsAsync("Employee"))
            //        {
            //            await _roleManager.CreateAsync(new IdentityRole("Employee"));
            //        }

            //        await _userManager.AddToRoleAsync(user, "Employee");

            //        // Add the employee to the Employee table
            //        _context.Add(employeeModel);
            //        await _context.SaveChangesAsync();

            //        return RedirectToAction(nameof(Index));
            //    }
            //    else
            //    {
            //        foreach (var error in result.Errors)
            //        {
            //            ModelState.AddModelError(string.Empty, error.Description);
            //        }
            //    }

            //    _context.Add(employeeModel);
            //    await _context.SaveChangesAsync();
            //    return RedirectToAction(nameof(Index));
            //}
            //catch (DbUpdateException ex)
            //{
            //    // Log the exception or handle it appropriately
            //    Console.WriteLine($"Error: {ex.Message}");
            //    return View(employeeModel);
            //}
        }

        // GET: Employee/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employeeModel = await _context.Employees.FindAsync(id);
            if (employeeModel == null)
            {
                return NotFound();
            }
            return View(employeeModel);
        }

        // POST: Employee/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("EmployeeId,Name,Surname,Email,Number,Password,ConfirmPassword")] EmployeeModel employeeModel)
        {
            if (id != employeeModel.EmployeeId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(employeeModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmployeeModelExists(employeeModel.EmployeeId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(employeeModel);
        }

        // GET: Employee/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employeeModel = await _context.Employees
                .FirstOrDefaultAsync(m => m.EmployeeId == id);
            if (employeeModel == null)
            {
                return NotFound();
            }

            return View(employeeModel);
        }

        // POST: Employee/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var employeeModel = await _context.Employees.FindAsync(id);
            if (employeeModel != null)
            {
                _context.Employees.Remove(employeeModel);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EmployeeModelExists(int id)
        {
            return _context.Employees.Any(e => e.EmployeeId == id);
        }
    }
}
