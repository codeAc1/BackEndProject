using Juan.DAL;
using Juan.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Juan.Controllers
{
    
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;
        public HomeController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            HomeVM homeVM = new HomeVM()
            {
                Sliders = _context.Sliders.Where(s=>!s.IsDeleted),
                Products = _context.Products.Include(p=>p.ProductImages).Include(x=>x.Reviews).Where(p => !p.IsDeleted),
                Blogs= _context.Blogs.Include(a=>a.AppUser).Where(b=>!b.IsDeleted),
                Brands = _context.Brands.Where(b => !b.IsDeleted),

            };
            return View(homeVM);
        }
    }
}
