using Juan.DAL;
using Juan.Extensions;
using Juan.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Juan.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,Publisher,Manager")]
    public class CategoryController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;
        public CategoryController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public IActionResult Index(bool? status, int page = 1)
        {
            ViewBag.Status = status;

            IQueryable<Category> categories = _context.Categories
                .Include(c => c.Products)
                .OrderByDescending(c => c.CreatedAt)
                .AsQueryable();

            if (status != null)
                categories = categories.Where(c => c.IsDeleted == status);

            ViewBag.PageIndex = page;
            ViewBag.PageCount = Math.Ceiling((double)categories.Count() / 5);
            return View(categories.Skip((page - 1) * 5).Take(5).ToList());
        }
        public IActionResult Create()
        {
            
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Category category, bool? status,  int page = 1)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            if (category.Name.CheckString())
            {
                ModelState.AddModelError("Name", "Bosluq reqem ve simvol olmaz");
                return View();
            }

            if (await _context.Categories.AnyAsync(t => t.Name.ToLower() == category.Name.ToLower()))
            {
                ModelState.AddModelError("Name", "Alreade Exists");
                return View();
            }
            category.CreatedAt = DateTime.UtcNow.AddHours(4);
            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", new { status = status, page = page });
        }

        public async Task<IActionResult> Update(int? id)
        {
            if (id == null) return BadRequest();

            Category category = await _context.Categories.FirstOrDefaultAsync(c => c.Id == id);

            if (category == null) return NotFound();

            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? id, Category category, bool? status, int page = 1)
        {
            if (!ModelState.IsValid) return View(category);

            if (id == null) return BadRequest();

            if (id != category.Id) return BadRequest();

            Category dbCategory = await _context.Categories.FirstOrDefaultAsync(t => t.Id == id);

            if (dbCategory == null) return NotFound();

            if (string.IsNullOrWhiteSpace(category.Name))
            {
                ModelState.AddModelError("Name", "Bosluq Olmamalidir");
                return View(category);
            }

            if (category.Name.CheckString())
            {
                ModelState.AddModelError("Name", "Reqem ve bosluq olmaz");
                return View(category);
            }

            if (await _context.Categories.AnyAsync(c => c.Id != category.Id && c.Name.ToLower() == category.Name.ToLower()))
            {
                ModelState.AddModelError("Name", "Alreade Exists");
                return View(category);
            }

            dbCategory.Name = category.Name;
            dbCategory.UpdatedAt = DateTime.UtcNow.AddHours(4);

            await _context.SaveChangesAsync();


            return RedirectToAction("Index", new { status = status, page = page });
        }

        public async Task<IActionResult> DeleteRestore(int? id, bool? status, int page = 1)
        {
            if (id == null) return BadRequest();

            Category dbCategory = await _context.Categories.FirstOrDefaultAsync(c => c.Id == id);

            if (dbCategory == null) return NotFound();
            if (!dbCategory.IsDeleted)
            {
                dbCategory.IsDeleted = true;
                dbCategory.DeletedAt = DateTime.UtcNow.AddHours(4);
            }
            else
            {
                dbCategory.IsDeleted = false;
                dbCategory.DeletedAt = null;
            }

            await _context.SaveChangesAsync();
            ViewBag.Status = status;
            IEnumerable<Category> categories = await _context.Categories
                .Include(c => c.Products)
                .Where(c => status != null ? c.IsDeleted == status : true)
                .OrderByDescending(c => c.Id)
                .ToListAsync();
            ViewBag.PageIndex = page;
            ViewBag.PageCount = Math.Ceiling((double)categories.Count() / 5);
            return PartialView("_CategoryIndexPartial", categories.Skip((page - 1) * 5).Take(5));

        }
    }
}
