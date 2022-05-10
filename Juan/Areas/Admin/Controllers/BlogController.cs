using Juan.DAL;
using Juan.Extensions;
using Juan.Helpers;
using Juan.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Juan.Helpers.Helper;

namespace Juan.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,Publisher,Manager")]
    public class BlogController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public BlogController(AppDbContext context, IWebHostEnvironment env, UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _env = env;
            _userManager = userManager;
            _roleManager = roleManager;
        }
        public async Task<IActionResult> Index(bool? status, int page = 1)
        {
            ViewBag.Status = status;


            IQueryable<Blog> blogs = _context.Blogs
                .Include(b => b.Category)
                .Include(b => b.BlogTags).ThenInclude(bt => bt.Tag)
                .Include(b=>b.AppUser)
                .Where(b=>b.AppUser.UserName==User.Identity.Name)
                .OrderByDescending(c => c.Id)
                .AsQueryable();

            if (status != null)
                blogs = blogs.Where(c => c.IsDeleted == status);

            ViewBag.PageIndex = page;
            ViewBag.PageCount = Math.Ceiling((double)blogs.Count() / 5);
            return View(await blogs.Skip((page - 1) * 5).Take(5).ToListAsync());
        }

        public async Task<IActionResult> Create(bool? status, int page = 1)
        {

            

            ViewBag.Brands = await _context.Brands.Where(b => !b.IsDeleted).ToListAsync();
            ViewBag.Categories = await _context.Categories.Where(c => !c.IsDeleted).ToListAsync();
            ViewBag.Tags = await _context.Tags.Where(t => !t.IsDeleted).ToListAsync();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Blog blog, bool? status, int page = 1)
        {
            ViewBag.Brands = await _context.Brands.Where(b => !b.IsDeleted).ToListAsync();
            ViewBag.Categories = await _context.Categories.Where(c => !c.IsDeleted).ToListAsync();
            ViewBag.Tags = await _context.Tags.Where(t => !t.IsDeleted).ToListAsync();

            AppUser appUser = _userManager.Users.FirstOrDefault(x => x.UserName == User.Identity.Name);

            if (appUser == null)
            {
                
                ModelState.AddModelError("", "Siz Login Olmamisiz");
                return View(blog);
            }
            if (!((await _userManager.GetRolesAsync(appUser))[0] == Roles.Publisher.ToString()))
            {
                ModelState.AddModelError("", "Siz Publisher deyilsiniz");
                return View(blog);
            }

            
            

            blog.AppUserId = appUser.Id;

            if (!ModelState.IsValid)
            {
                return View();
            }



            if (!await _context.Categories.AnyAsync(b => b.Id == blog.CategoryId && !b.IsDeleted))
            {
                ModelState.AddModelError("CategoryId", "Duzgun Category Secin ");
                return View(blog);
            }

            if (blog.TagIds.Count > 0)
            {
                List<BlogTag> blogTags = new List<BlogTag>();

                foreach (int item in blog.TagIds)
                {
                    if (!await _context.Tags.AnyAsync(t => t.Id != item && !t.IsDeleted))
                    {
                        ModelState.AddModelError("TagIds", $"Secilen Id {item} - li Tag Yanlisdir");
                        return View();
                    }

                    BlogTag blogTag = new BlogTag
                    {
                        TagId = item
                    };

                    blogTags.Add(blogTag);
                }

                blog.BlogTags = blogTags;
            }

            if (blog.ImageFile != null)
            {
                if (!blog.ImageFile.CheckFileContentType("image/jpeg"))
                {
                    ModelState.AddModelError("ImageFile", "Secilen Seklin Novu Uygun deyil");
                    return View();
                }

                if (!blog.ImageFile.CheckFileSize(3000))
                {
                    ModelState.AddModelError("ImageFile", "Secilen Seklin Olcusu Maksimum 300 Kb Ola Biler");
                    return View();
                }

                blog.ImageUrl = blog.ImageFile.CreateFile(_env, "user", "assets", "img", "blog");
            }
            else
            {
                ModelState.AddModelError("ImageFile", "Main Sekil Mutleq Secilmelidir");
                return View(blog);
            }

            blog.CreatedAt = DateTime.UtcNow.AddHours(4);
            await _context.Blogs.AddAsync(blog);
            await _context.SaveChangesAsync();
            return RedirectToAction("index", new { status, page });
        }
        public async Task<IActionResult> Detail(int? id, bool? status, int page = 1)
        {
            ViewBag.Status = status;
            ViewBag.PageIndex = page;
            if (id == null) return BadRequest();

            Blog blog = await _context. Blogs
                .Include(b => b.Category)
                .Include(b => b.BlogTags).ThenInclude(bt => bt.Tag)
                .Include(b => b.AppUser)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (blog == null) return NotFound();

            return View(blog);
        }
        public async Task<IActionResult> Delete(int? id, bool? status, int page = 1)
        {
            ViewBag.Status = status;
            ViewBag.PageIndex = page;
            if (id == null) return BadRequest();


            Blog blog = await _context.Blogs
                .Include(b => b.Category)
                .Include(b => b.BlogTags).ThenInclude(bt => bt.Tag)
                .Include(b => b.AppUser)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (blog == null) return NotFound();


            return View(blog);
        }

        public async Task<IActionResult> DeleteRestore(int? id, bool? status, int page = 1)
        {
            AppUser appUser = _userManager.Users.FirstOrDefault(x => x.UserName == User.Identity.Name);
            if (appUser == null)
            {
                return RedirectToAction("index", new { status, page });
            }

            if (!((await _userManager.GetRolesAsync(appUser))[0] == Roles.Publisher.ToString()))
            {
                return RedirectToAction("index", new { status, page });
            }

            if (id == null) return BadRequest();

            Blog blog = await _context.Blogs
                .FirstOrDefaultAsync(p => p.Id == id);

            if (blog == null) return NotFound();

            

            if (blog.IsDeleted)
            {
                blog.IsDeleted = false;
                blog.DeletedAt = null;
            }
            else
            {
                blog.IsDeleted = true;
                blog.DeletedAt = DateTime.UtcNow.AddHours(4);
            }


            await _context.SaveChangesAsync();

            return RedirectToAction("index", new { status, page });
        }
        public async Task<IActionResult> Update(int? id, bool? status, int page = 1)
        {
            

            if (id == null) return BadRequest();

            Blog blog = await _context.Blogs
               .Include(b => b.Category)
               .Include(b => b.BlogTags).ThenInclude(bt => bt.Tag)
               .Include(b => b.AppUser)
               .FirstOrDefaultAsync(p => p.Id == id);

            if (blog == null) return NotFound();

            ViewBag.Brands = await _context.Brands.Where(b => !b.IsDeleted).ToListAsync();
            ViewBag.Categories = await _context.Categories.Where(c => !c.IsDeleted).ToListAsync();
            ViewBag.Tags = await _context.Tags.Where(t => !t.IsDeleted).ToListAsync();
            blog.TagIds = blog.BlogTags.Select(pt => pt.Tag.Id).ToList();
            return View(blog);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? id, Blog blog, bool? status, int page = 1)
        {
            AppUser appUser = _userManager.Users.FirstOrDefault(x => x.UserName == User.Identity.Name);
            if (appUser == null)
            {
                return RedirectToAction("index", new { status, page });
            }

            if (!((await _userManager.GetRolesAsync(appUser))[0] == Roles.Publisher.ToString()))
            {
                return RedirectToAction("index", new { status, page });
            }

            ViewBag.Brands = await _context.Brands.Where(b => !b.IsDeleted).ToListAsync();
            ViewBag.Categories = await _context.Categories.Where(c => !c.IsDeleted).ToListAsync();
            ViewBag.Tags = await _context.Tags.Where(t => !t.IsDeleted).ToListAsync();

            if (id == null) return BadRequest();

            if (id != blog.Id) return BadRequest();

            Blog DBblog = await _context.Blogs
               .Include(b => b.Category)
               .Include(b => b.BlogTags).ThenInclude(bt => bt.Tag)
               .Include(b => b.AppUser)
               .FirstOrDefaultAsync(p => p.Id == id);

            if (DBblog == null) return NotFound();

            if (!ModelState.IsValid) return View(DBblog);


            if (!await _context.Categories.AnyAsync(b => b.Id == blog.CategoryId && !b.IsDeleted))
            {
                ModelState.AddModelError("CategoryId", "Duzgun Category Secin ");
                return View(blog);
            }



            if (blog.TagIds.Count > 0)
            {
                _context.BlogTags.RemoveRange(DBblog.BlogTags);

                List<BlogTag> blogTags = new List<BlogTag>();

                foreach (int item in blog.TagIds)
                {
                    if (!await _context.Tags.AnyAsync(t => t.Id != item && !t.IsDeleted))
                    {
                        ModelState.AddModelError("TagIds", $"Secilen Id {item} - li Tag Yanlisdir");
                        return View(blog);
                    }

                    BlogTag blogTag = new BlogTag
                    {
                        TagId = item
                    };

                    blogTags.Add(blogTag);
                }

                DBblog.BlogTags = blogTags;
            }
            else
            {
                _context.BlogTags.RemoveRange(DBblog.BlogTags);
            }




            if (blog.ImageFile != null)
            {
                if (!blog.ImageFile.CheckFileContentType("image/jpeg"))
                {
                    ModelState.AddModelError("ImageFile", "Secilen Seklin Novu Uygun");
                    return View();
                }

                if (!blog.ImageFile.CheckFileSize(300))
                {
                    ModelState.AddModelError("ImageFile", "Secilen Seklin Olcusu Maksimum 300 Kb Ola Biler");
                    return View();
                }
                Helper.DeleteFile(_env, DBblog.ImageUrl, "user", "assets", "img", "blog");

                DBblog.ImageUrl = blog.ImageFile.CreateFile(_env, "user", "assets", "img", "blog");
            }


            DBblog.CategoryId = blog.CategoryId;
            DBblog.Title = blog.Title;
            blog.MainDes = blog.MainDes;
            blog.Desc = blog.Desc;
            DBblog.UpdatedAt = DateTime.UtcNow.AddHours(4);

            await _context.SaveChangesAsync();

            return RedirectToAction("index", new { status, page });
        }
    }
}
