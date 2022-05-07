using Juan.DAL;
using Juan.Extensions;
using Juan.Helpers;
using Juan.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Juan.Areas.Admin.Controllers
{
    [Area("Admin")]
    
    public class SliderController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;
        public SliderController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public async Task<IActionResult>  Index()
        {
            IEnumerable<Slider> sliders = await _context.Sliders
                .OrderByDescending(t => t.CreatedAt)
                .ToListAsync();
            ViewBag.PageCount = Math.Ceiling((double)sliders.Count() / 5);
            return View(sliders);
        }
        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null) return BadRequest();
            Slider slider = await _context.Sliders.FirstOrDefaultAsync(c => c.Id == id);

            if (slider == null) return NotFound();
            return View(slider);
        }
        public IActionResult Create()
        {
            if (_context.Sliders.Count() >= 5)
            {
                return RedirectToAction(nameof(Index));
            }
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Slider slider)
        {
            if (slider.Photo==null)
            {
                ModelState.AddModelError("Photo", "Sekil secilmeyib");
                return View(slider);
            }
            if (ModelState["Photo"].ValidationState == ModelValidationState.Invalid) return View();
            if (!slider.Photo.CheckFileContentType("image/jpeg"))
            {
                ModelState.AddModelError("Photo", "Ancaq şəkil seçilə bilər");
                return View(slider);
            }

            var size = 200;
            if (!slider.Photo.CheckFileSize(size))
            {
                ModelState.AddModelError("Photo", $"Şəklin ölşüsü {size}-kb dan çox olmamalıdır sizin seçdiyiniz fayil {Math.Ceiling((decimal)slider.Photo.Length) / 1024:N2} kb-dir");
                return View();
            }

            slider.ImageUrl = slider.Photo.CreateFile(_env, "user", "assets", "img", "slider");
            slider.CreatedAt = DateTime.UtcNow.AddHours(4);
            await _context.Sliders.AddAsync(slider);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {

            if (id == null) return BadRequest();
            if (_context.Sliders.Count() == 1)
            {
                return Content("olmaz");
            }
            Slider slider = await _context.Sliders.FindAsync(id);
            if (slider == null) return NotFound();
            return View(slider);
        }
        [HttpPost]

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeletePost(int? id)
        {
            if (id == null) return BadRequest();
            Slider slider = await _context.Sliders.FindAsync(id);
            if (slider == null) return NotFound();
            if (_context.Sliders.Count() == 1)
            {
                return View();
            }
            Helper.DeleteFile(_env, slider.ImageUrl, "user", "assets", "img", "slider");
            _context.Sliders.Remove(slider);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Update(int? id)
        {
            if (id==null)
            {
                return BadRequest();
            }
            Slider slider= await _context.Sliders.Where(s => s.Id == id).FirstOrDefaultAsync();
            if (slider==null)
            {
                return NotFound();
            }
            return View(slider);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(Slider slider)
        {
            Slider dbSlider = await _context.Sliders.FirstOrDefaultAsync(x=>x.Id==slider.Id);

            slider.ImageUrl = dbSlider.ImageUrl;

            if (!ModelState.IsValid)
            {
                return View(slider);
            }

            if (slider.Photo != null)
            {
                if (!slider.Photo.CheckFileContentType("image/jpeg"))
                {
                    ModelState.AddModelError("Photo", "Secilen Seklin Novu Uygun deyil ancaq jpeg ve ya jpg secile biler");
                    return View();
                }

                if (!slider.Photo.CheckFileSize(500))
                {
                    ModelState.AddModelError("Photo", "Secilen Seklin Olcusu Maksimum 500 Kb Ola Biler");
                    return View(slider);
                }

                Helper.DeleteFile(_env, dbSlider.ImageUrl, "user", "assets", "img", "slider");

                dbSlider.ImageUrl = slider.Photo.CreateFile(_env, "user", "assets", "img", "slider");
            }

            dbSlider.SlideSubtitle = slider.SlideSubtitle;
            dbSlider.SlideTitle = slider.SlideTitle;
            dbSlider.SlideDesc = slider.SlideDesc;

            dbSlider.UpdatedAt = DateTime.UtcNow.AddHours(4);

            await _context.SaveChangesAsync();

            return RedirectToAction("index");
        }

    }
}
