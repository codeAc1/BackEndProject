using Juan.DAL;
using Juan.Models;
using Juan.ViewModels;
using Microsoft.AspNetCore.Hosting;
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
    public class OrderController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;
        public OrderController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public async Task<IActionResult> Index(bool? status, int page = 1)
        {
            ViewBag.Status = status;


            IQueryable<Order> orders = _context.Orders
                .Include(o => o.OrderItems)
                .Include(o=>o.AppUser)
                .AsQueryable();

            if (status != null)
                orders = orders.Where(c => c.IsDeleted == status);

            ViewBag.PageIndex = page;
            ViewBag.PageCount = Math.Ceiling((double)orders.Count() / 5);
            return View(await orders.Skip((page - 1) * 5).Take(5).ToListAsync());
        }
        public async Task<IActionResult> Update(int? id)
        {
            if (id == null) return BadRequest();

            Order order = await _context.Orders
                .Include(o=>o.OrderItems).ThenInclude(o=>o.Product)
                .Include(o=>o.AppUser)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (order == null) return NotFound();

            return View(order);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? id, int orderStatus,bool? status, int page = 1)
        {
            if (id == null) return BadRequest();

            Order order = await _context.Orders
                .Include(o => o.AppUser)
                .Include(o => o.OrderItems).ThenInclude(oi => oi.Product)
                .FirstOrDefaultAsync(o => o.Id == id && !o.IsDeleted);

            if (order == null) return NotFound();

            if (order.Status != OrderStatus.Accepted && orderStatus == 1)
            {
                foreach (var item in order.OrderItems)
                {
                    item.Product.Count -= item.Count;
                }
            }

            order.Status = (OrderStatus)orderStatus;
            order.UpdatedAt = DateTime.UtcNow.AddHours(4);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", new { status = status, page = page });
        }

        public async Task<IActionResult> DeleteRestore(int? id, bool? status, int page = 1)
        {
            if (id == null) return BadRequest();

            Order order = await _context.Orders
                .Include(o => o.OrderItems).ThenInclude(o => o.Product)
                .Include(o => o.AppUser)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (order == null) return NotFound();

            if (order.IsDeleted)
            {
                order.IsDeleted = false;
                order.DeletedAt = null;
            }
            else
            {
                order.IsDeleted = true;
                order.DeletedAt = DateTime.UtcNow.AddHours(4);
            }            
            await _context.SaveChangesAsync();

            ViewBag.Status = status;


            IQueryable<Order> orders = _context.Orders
                .Include(o => o.OrderItems)
                .Include(o => o.AppUser)
                .AsQueryable();

            if (status != null)
                orders = orders.Where(c => c.IsDeleted == status);

            ViewBag.PageIndex = page;
            ViewBag.PageCount = Math.Ceiling((double)orders.Count() / 5);
            return PartialView("_OrderIndexPartial", orders.Skip((page - 1) * 5).Take(5));
        }
    }
}
