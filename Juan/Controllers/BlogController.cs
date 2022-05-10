using Juan.DAL;
using Juan.Models;
using Juan.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Juan.Controllers
{
    public class BlogController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        public BlogController(AppDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public async Task<IActionResult> Index(int? categoryId,  List<int> TagIds, int page = 1)
        {

            var blogs = _context.Blogs
                .Include(x => x.Category)
                .Include(x => x.Reviews)
                .Include(x=>x.BlogTags).ThenInclude(x=>x.Tag)
                .Include(x=>x.AppUser)
                .Where(x => !x.IsDeleted);

            ViewBag.CategoryId = categoryId;
            ViewBag.PageIndex = page;

            ViewBag.TotalBlogs = blogs.Count();

            BlogListVM blogListVM = new BlogListVM();



            if (categoryId != null)
            {
                blogs = blogs.Where(x => x.CategoryId == categoryId);
            }

            if (TagIds != null && TagIds.Count > 0)
                blogs = blogs.Where(x => x.BlogTags.Any(p => TagIds.Contains((int)p.TagId)));



            ViewBag.TotalPages = (int)Math.Ceiling(blogs.Count() / 6d);

            blogListVM.Blogs = blogs.Skip((page - 1) * 6).Take(6).ToList();
            blogListVM.Categories = await _context.Categories.Include(x => x.Blogs).Where(x => !x.IsDeleted).ToListAsync();
            blogListVM.Tags = await _context.Tags.Where(x => !x.IsDeleted).ToListAsync();
            blogListVM.FilterTagIds = TagIds;
            return View(blogListVM);
        }

        public async Task<IActionResult> BlogDetail(int? id)
        {
            if (id == null) return BadRequest();
            
            Blog blog = await _context.Blogs
                .Include(x => x.Category)
                .Include(x => x.BlogTags).ThenInclude(x => x.Tag)
                .Include(x => x.Reviews).ThenInclude(c => c.AppUser)
                .Include(x=>x.AppUser)
                .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
            if (blog == null) return NotFound();

            BlogDetailVM blogDetailVM = new BlogDetailVM
            {
                Blog = blog,
                Review = new Review { BlogId = id },
                Blogs = await _context.Blogs.Where(x => x.CategoryId == blog.CategoryId).Take(4).OrderByDescending(x=>x.CreatedAt).ToListAsync()
            };

            return View(blogDetailVM);
        }
    }
}
