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
    [Area ("Admin")]
    public class ColorController : Controller
    {
        private readonly AppDbContext _context;
        public ColorController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index(bool? status, int page = 1)
        {

            ViewBag.Status = status;
            IEnumerable<Color> colors = await _context.Colors
                .Include(c => c.ProductColors)
                .Where(c => status != null ? c.IsDeleted == status : true)
                .OrderByDescending(t => t.CreatedAt)
                .ToListAsync();
            ViewBag.PageIndex = page;
            ViewBag.PageCount = Math.Ceiling((double)colors.Count() / 5);
            return View(colors.Skip((page - 1) * 5).Take(5));
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Color color)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            if (color.Name.CheckString())
            {
                ModelState.AddModelError("Name", "Yalniz Herf Ola Biler");
                return View();
            }

            if (await _context.Colors.AnyAsync(t => t.Name.ToLower() == color.Name.ToLower()))
            {
                ModelState.AddModelError("Name", "Alreade Exists");
                return View();
            }
            color.CreatedAt = DateTime.UtcNow.AddHours(4);
            await _context.Colors.AddAsync(color);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Update(int? id)
        {
            if (id == null) return BadRequest();

            Color color = await _context.Colors.FirstOrDefaultAsync(c => c.Id == id);

            if (color == null) return NotFound();

            return View(color);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? id, Color color, bool? status, int page = 1)
        {
            if (!ModelState.IsValid) return View(color);

            if (id == null) return BadRequest();

            if (id != color.Id) return BadRequest();

            Color dbColor = await _context.Colors.FirstOrDefaultAsync(t => t.Id == id);

            if (dbColor == null) return NotFound();

            if (string.IsNullOrWhiteSpace(color.Name))
            {
                ModelState.AddModelError("Name", "Bosluq Olmamalidir");
                return View(color);
            }

            if (color.Name.CheckString())
            {
                ModelState.AddModelError("Name", "Yalniz Herf Ola Biler");
                return View(color);
            }

            if (await _context.Colors.AnyAsync(c => c.Id != color.Id && c.Name.ToLower() == color.Name.ToLower()))
            {
                ModelState.AddModelError("Name", "Alreade Exists");
                return View(color);
            }

            dbColor.Name = color.Name;
            dbColor.UpdatedAt = DateTime.UtcNow.AddHours(4);

            await _context.SaveChangesAsync();


            return RedirectToAction("Index", new { status = status, page = page });
        }

        public async Task<IActionResult> DeleteRestore(int? id, bool? status, int page = 1)
        {
            if (id == null) return BadRequest();

            Color dbColor = await _context.Colors.FirstOrDefaultAsync(t => t.Id == id);

            if (dbColor == null) return NotFound();
            if (!dbColor.IsDeleted)
            {
                dbColor.IsDeleted = true;
                dbColor.DeletedAt = DateTime.UtcNow.AddHours(4);
            }
            else
            {
                dbColor.IsDeleted = false;
                dbColor.DeletedAt = null;
            }

            await _context.SaveChangesAsync();
            ViewBag.Status = status;
            IEnumerable<Color> colors = await _context.Colors
                .Include(c => c.ProductColors)
                .Where(c => status != null ? c.IsDeleted == status : true)
                .OrderByDescending(t => t.Id)
                .ToListAsync();
            ViewBag.PageIndex = page;
            ViewBag.PageCount = Math.Ceiling((double)colors.Count() / 5);
            return PartialView("_ColorIndexPartial", colors.Skip((page - 1) * 5).Take(5));

        }


    }
}
