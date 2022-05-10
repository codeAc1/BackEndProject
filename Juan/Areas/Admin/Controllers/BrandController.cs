using Juan.DAL;
using Juan.Extensions;
using Juan.Helpers;
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
    public class BrandController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;
        public BrandController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public async Task<IActionResult> Index(bool? status, int page = 1)
        {

            ViewBag.Status = status;
            IEnumerable<Brand> brands = await _context.Brands
                .Include(b => b.Products)
                .Where(b => status != null ? b.IsDeleted == status : true)
                .OrderByDescending(t => t.CreatedAt)
                .ToListAsync();
            ViewBag.PageIndex = page;
            ViewBag.PageCount = Math.Ceiling((double)brands.Count() / 5);
            return View(brands.Skip((page - 1) * 5).Take(5));
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Brand brand)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            if (string.IsNullOrWhiteSpace(brand.Name))
            {
                ModelState.AddModelError("Name", "Bosluq Olmamalidir");
                return View();
            }

            if (brand.Name.CheckString())
            {
                ModelState.AddModelError("Name", "Yalniz Herf Ola Biler");
                return View();
            }

            if (await _context.Brands.AnyAsync(t => t.Name.ToLower() == brand.Name.ToLower()))
            {
                ModelState.AddModelError("Name", "Alreade Exists");
                return View();
            }

            if (brand.ImageFile != null)
            {
                if (!brand.ImageFile.CheckFileContentType("image/png"))
                {
                    ModelState.AddModelError("ImageFile", "Secilen Seklin Novu Uygun deyil");
                    return View();
                }

                if (!brand.ImageFile.CheckFileSize(3000))
                {
                    ModelState.AddModelError("ImageFile", "Secilen Seklin Olcusu Maksimum 300 Kb Ola Biler");
                    return View();
                }

                brand.ImageUrl = brand.ImageFile.CreateFile(_env, "user", "assets", "img", "brand");
            }
            else
            {
                ModelState.AddModelError("ImageFile", "Main Sekil Mutleq Secilmelidir");
                return View(brand);
            }
            brand.CreatedAt = DateTime.UtcNow.AddHours(4);
            await _context.Brands.AddAsync(brand);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Update(int? id)
        {
            if (id == null) return BadRequest();

            Brand brand = await _context.Brands.FirstOrDefaultAsync(t => t.Id == id);

            if (brand == null) return NotFound();

            return View(brand);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? id, Brand brand, bool? status, int page = 1)
        {
            if (!ModelState.IsValid) return View(brand);

            if (id == null) return BadRequest();

            if (id != brand.Id) return BadRequest();

            Brand dbBrand = await _context.Brands.FirstOrDefaultAsync(t => t.Id == id);

            if (dbBrand == null) return NotFound();

            if (string.IsNullOrWhiteSpace(brand.Name))
            {
                ModelState.AddModelError("Name", "Bosluq Olmamalidir");
                return View(brand);
            }

            if (brand.Name.CheckString())
            {
                ModelState.AddModelError("Name", "Yalniz Herf Ola Biler");
                return View(brand);
            }

            if (await _context.Tags.AnyAsync(t => t.Id != brand.Id && t.Name.ToLower() == brand.Name.ToLower()))
            {
                ModelState.AddModelError("Name", "Alreade Exists");
                return View(brand);
            }

            if (brand.ImageFile != null)
            {
                if (!brand.ImageFile.CheckFileContentType("image/png"))
                {
                    ModelState.AddModelError("ImageFile", "Secilen Seklin Novu Uygun deyil ancaq png secile biler");
                    return View();
                }

                if (!brand.ImageFile.CheckFileSize(300))
                {
                    ModelState.AddModelError("ImageFile", "Secilen Seklin Olcusu Maksimum 3 Kb Ola Biler");
                    return View(brand);
                }
                if (dbBrand.ImageUrl!=null)
                {
                    Helper.DeleteFile(_env, dbBrand.ImageUrl, "user", "assets", "img", "brand");
                }
                

                dbBrand.ImageUrl = brand.ImageFile.CreateFile(_env, "user", "assets", "img", "brand");
            }

            dbBrand.Name = brand.Name;
            brand.UpdatedAt = DateTime.UtcNow.AddHours(4);

            await _context.SaveChangesAsync();


            return RedirectToAction("Index", new { status = status, page = page });
        }

        public async Task<IActionResult> DeleteRestore(int? id, bool? status, int page = 1)
        {
            if (id == null) return BadRequest();

            Brand dbBrand = await _context.Brands.FirstOrDefaultAsync(t => t.Id == id);

            if (dbBrand == null) return NotFound();

            if (dbBrand.IsDeleted)
            {
                dbBrand.IsDeleted = false;
            }
            else
            {
                dbBrand.IsDeleted = true;
                dbBrand.DeletedAt = DateTime.UtcNow.AddHours(4);
            }
            

            await _context.SaveChangesAsync();

            ViewBag.Status = status;
            IEnumerable<Brand> brands = await _context.Brands
                .Include(t => t.Products)
                .Where(t => status != null ? t.IsDeleted == status : true)
                .OrderByDescending(t => t.CreatedAt)
                .ToListAsync();
            ViewBag.PageIndex = page;
            ViewBag.PageCount = Math.Ceiling((double)brands.Count() / 5);
            return PartialView("_BrandIndexPartial", brands.Skip((page - 1) * 5).Take(5));

        }

        
    }
}
