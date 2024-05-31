using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AgriConnect_MVC.Data;
using AgriConnect_MVC.Models;
using Microsoft.AspNetCore.Authorization;

namespace AgriConnect_MVC.Controllers
{
    [Authorize(Roles = "Employee")]
    public class FarmerController : Controller
    {
        private readonly ApplicationDbContext _context;

        public FarmerController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Farmer/Profile
        public async Task<IActionResult> Profile()
        {
            // Get the email of the currently logged-in user
            var userEmail = User.Identity.Name;

            // Find the farmer with the corresponding email
            var farmer = await _context.Farmers.SingleOrDefaultAsync(f => f.Email == userEmail);

            if (farmer == null)
            {
                return NotFound("Farmer not found.");
            }

            // Fetch the products of the logged-in farmer
            var products = await _context.Products
                                         .Where(p => p.FarmerId == farmer.FarmerId)
                                         .ToListAsync();

            return View(products);
        }

        //this method adds the approval button for employees to toggle
        [HttpPost]
        public async Task<IActionResult> Approve(int id)
        {
            var farmer = await _context.Farmers.FindAsync(id);
            if (farmer == null)
            {
                return NotFound();
            }

            farmer.Approved = farmer.Approved == "Yes" ? "No" : "Yes"; // Toggle approval status
            _context.Update(farmer);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index)); // Redirect to the list of farmers
        }

        // GET: Farmer
        public async Task<IActionResult> Index()
        {
            return View(await _context.Farmers.ToListAsync());
        }

        // GET: Farmer/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var farmerModel = await _context.Farmers
                .FirstOrDefaultAsync(m => m.FarmerId == id);
            if (farmerModel == null)
            {
                return NotFound();
            }

            return View(farmerModel);
        }

        // GET: Farmer/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Farmer/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FarmerId,Name,Surname,Email,Number,Approved")] FarmerModel farmerModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(farmerModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(farmerModel);
        }

        // GET: Farmer/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var farmerModel = await _context.Farmers.FindAsync(id);
            if (farmerModel == null)
            {
                return NotFound();
            }
            return View(farmerModel);
        }


        // POST: Farmer/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("FarmerId,Name,Surname,Email,Number,Approved")] FarmerModel farmerModel)
        {
            if (id != farmerModel.FarmerId)
            {
                return NotFound();
            }

            try
            {
                try
                {
                    _context.Update(farmerModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FarmerModelExists(farmerModel.FarmerId))
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
            catch (DbUpdateException ex)
            {
                // Log the exception or handle it appropriately
                Console.WriteLine($"Error: {ex.Message}");
                return View(farmerModel);
            }
        }

        // GET: Farmer/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var farmerModel = await _context.Farmers
                .FirstOrDefaultAsync(m => m.FarmerId == id);
            if (farmerModel == null)
            {
                return NotFound();
            }

            return View(farmerModel);
        }

        // POST: Farmer/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var farmerModel = await _context.Farmers.FindAsync(id);
            if (farmerModel != null)
            {
                _context.Farmers.Remove(farmerModel);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FarmerModelExists(int id)
        {
            return _context.Farmers.Any(e => e.FarmerId == id);
        }
    }
}
