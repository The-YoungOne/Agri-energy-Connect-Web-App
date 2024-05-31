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
using Microsoft.AspNetCore.Authorization;

namespace AgriConnect_MVC.Controllers
{
    [Authorize]
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public ProductController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Product/AllProducts
        public async Task<IActionResult> AllProducts(string farmerName, string farmerSurname, ProductCategory? category, DateTime? startDate, DateTime? endDate)
        {
            var query = _context.Products.Include(p => p.Farmer).AsQueryable();

            if (!string.IsNullOrEmpty(farmerName))
            {
                query = query.Where(p => p.Farmer.Name.Contains(farmerName));
            }

            if (!string.IsNullOrEmpty(farmerSurname))
            {
                query = query.Where(p => p.Farmer.Surname.Contains(farmerSurname));
            }

            if (category.HasValue)
            {
                query = query.Where(p => p.Category == category);
            }

            if (startDate.HasValue)
            {
                query = query.Where(p => p.ProductionDate >= startDate);
            }

            if (endDate.HasValue)
            {
                query = query.Where(p => p.ProductionDate <= endDate);
            }

            var products = await query.ToListAsync();
            return View(products);
        }

        // GET: Product/ViewFarmerProducts/5
        public async Task<IActionResult> ViewFarmerProducts(int farmerId)
        {
            var products = await _context.Products
                                         .Include(p => p.Farmer)
                                         .Where(p => p.FarmerId == farmerId)
                                         .ToListAsync();

            if (!products.Any())
            {
                return NotFound("No products found for this farmer.");
            }

            return View(products);
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


        // GET: Product
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Products.Include(p => p.Farmer);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Product/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productModel = await _context.Products
                .Include(p => p.Farmer)
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (productModel == null)
            {
                return NotFound();
            }

            return View(productModel);
        }

        // GET: Product/Create
        public IActionResult Create()
        {
            ViewData["FarmerId"] = new SelectList(_context.Farmers, "FarmerId", "Approved");
            return View();
        }

        // POST: Product/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductId,Name,Category,Quantity,ProductionDate,ImageUrl")] ProductModel productModel)
        {
            try
            {
                var userEmail = User.Identity.Name;
                var farmer = await _context.Farmers.SingleOrDefaultAsync(f => f.Email == userEmail);

                if (farmer != null)
                {
                    productModel.FarmerId = farmer.FarmerId;
                    _context.Add(productModel);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Profile));
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Farmer not found.");
                    return RedirectToAction(nameof(Profile));
                }
            }
            catch (DbUpdateException ex)
            {
                // Log the exception or handle it appropriately
                Console.WriteLine($"Error: {ex.Message}");
                return RedirectToAction(nameof(Profile));
            }
        }


        // GET: Product/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productModel = await _context.Products.FindAsync(id);
            if (productModel == null)
            {
                return NotFound();
            }
            ViewData["FarmerId"] = new SelectList(_context.Farmers, "FarmerId", "Approved", productModel.FarmerId);
            return View(productModel);
        }

        // POST: Product/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProductId,Name,Category,Quantity,ProductionDate,ImageUrl,FarmerId")] ProductModel productModel)
        {
            try
            {
                if (id != productModel.ProductId)
                {
                    return NotFound();
                }

                try
                {
                    _context.Update(productModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductModelExists(productModel.ProductId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Profile));
            }
            catch (DbUpdateConcurrencyException ex)
            {
                Console.WriteLine($"Error: -- {ex.Message}"); 
                ViewData["FarmerId"] = new SelectList(_context.Farmers, "FarmerId", "Approved", productModel.FarmerId);
                return View(productModel);
            }
        }


        // GET: Product/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productModel = await _context.Products
                .Include(p => p.Farmer)
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (productModel == null)
            {
                return NotFound();
            }

            return View(productModel);
        }

        // POST: Product/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var productModel = await _context.Products.FindAsync(id);
            if (productModel != null)
            {
                _context.Products.Remove(productModel);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Profile));
        }

        private bool ProductModelExists(int id)
        {
            return _context.Products.Any(e => e.ProductId == id);
        }
    }
}
