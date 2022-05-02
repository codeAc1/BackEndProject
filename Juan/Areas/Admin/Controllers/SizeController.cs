using Juan.DAL;
using Juan.Extensions;
using Juan.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Juan.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SizeController : Controller
    {
        private readonly AppDbContext _context;
        public SizeController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index(bool? status, int page = 1)
        {

            ViewBag.Status = status;
            IEnumerable<Size> sizes = await _context.Sizes
                .Include(s => s.ProductSizes)
                .Where(s => status != null ? s.IsDeleted == status : true)
                .OrderByDescending(t => t.CreatedAt)
                .ToListAsync();
            ViewBag.PageIndex = page;
            ViewBag.PageCount = Math.Ceiling((double)sizes.Count() / 5);
            return View(sizes.Skip((page - 1) * 5).Take(5));
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Size size)
        {

            if (!ModelState.IsValid)
            {
                return View();
            }

            if (await _context.Sizes.AnyAsync(t => t.Name == size.Name))
            {
                ModelState.AddModelError("Name", "Alreade Exists");
                return View();
            }
            size.CreatedAt = DateTime.UtcNow.AddHours(4);
            await _context.Sizes.AddAsync(size);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Update(int? id)
        {
            if (id == null) return BadRequest();

            Size size = await _context.Sizes.FirstOrDefaultAsync(s => s.Id == id);

            if (size == null) return NotFound();

            return View(size);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? id, Size size, bool? status, int page = 1)
        {
            if (!ModelState.IsValid) return View(size);

            if (id == null) return BadRequest();

            if (id != size.Id) return BadRequest();

            Size dbSize = await _context.Sizes.FirstOrDefaultAsync(t => t.Id == id);

            if (dbSize == null) return NotFound();


            if (await _context.Sizes.AnyAsync(c => c.Id != size.Id && c.Name == size.Name))
            {
                ModelState.AddModelError("Name", "Alreade Exists");
                return View(size);
            }

            dbSize.Name = size.Name;
            dbSize.UpdatedAt = DateTime.UtcNow.AddHours(4);

            await _context.SaveChangesAsync();


            return RedirectToAction("Index", new { status = status, page = page });
        }

        public async Task<IActionResult> DeleteRestore(int? id, bool? status, int page = 1)
        {
            if (id == null) return BadRequest();

            Size dbSize = await _context.Sizes.FirstOrDefaultAsync(s => s.Id == id);

            if (dbSize == null) return NotFound();
            if (!dbSize.IsDeleted)
            {
                dbSize.IsDeleted = true;
                dbSize.DeletedAt = DateTime.UtcNow.AddHours(4);
            }
            else
            {
                dbSize.IsDeleted = false;
                dbSize.DeletedAt = null;
            }

            await _context.SaveChangesAsync();
            ViewBag.Status = status;
            IEnumerable<Size> sizes = await _context.Sizes
                .Include(s => s.ProductSizes)
                .Where(s => status != null ? s.IsDeleted == status : true)
                .OrderByDescending(t => t.Id)
                .ToListAsync();
            ViewBag.PageIndex = page;
            ViewBag.PageCount = Math.Ceiling((double)sizes.Count() / 5);
            return PartialView("_SizeIndexPartial", sizes.Skip((page - 1) * 5).Take(5));

        }
    }
}
