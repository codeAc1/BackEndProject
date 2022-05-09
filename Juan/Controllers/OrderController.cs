using Juan.DAL;
using Juan.Models;
using Juan.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Juan.Controllers
{
    [Authorize(Roles = "Member")]
    public class OrderController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public OrderController(AppDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public async Task<IActionResult> Create()
        {
            AppUser appUser = await _userManager.Users.FirstOrDefaultAsync(u => u.UserName == User.Identity.Name && !u.isAdmin);

            if (appUser == null)
            {
                return RedirectToAction("login", "Account");
            }

            double total = 0;
            List<Basket> baskets = await _context.Baskets.Include(b => b.Product).Where(b => b.AppUserId == appUser.Id).ToListAsync();

            foreach (Basket item in baskets)
            {
                total = (double)(total + (item.Count * (item.Product.DiscountPrice > 0 ? item.Product.DiscountPrice : item.Product.Price)));
            }

            ViewBag.Total = total;

            OrderVM orderVM = new OrderVM
            {
                Name = appUser.Name,
                Surname=appUser.SurName,
                Email = appUser.Email,
                Address = appUser.Address,
                City = appUser.City,
                Country = appUser.Country,
                State = appUser.State,
                ZipCode = appUser.ZipCode,
                Phone=appUser.PhoneNumber,

                Baskets = await _context.Baskets
                .Include(o => o.AppUser)
                .Where(o => !o.IsDeleted && o.AppUserId == appUser.Id).ToListAsync()
            };



            return View(orderVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(OrderVM orderVM)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            AppUser appUser = await _userManager.Users.FirstOrDefaultAsync(u => u.UserName == User.Identity.Name && !u.isAdmin);

            if (appUser == null)
            {
                return RedirectToAction("login", "Account");
            }

            List<Basket> baskets = await _context.Baskets.Include(b => b.Product).Where(b => b.AppUserId == appUser.Id).ToListAsync();
            List<OrderItem> orderItems = new List<OrderItem>();
            double total = 0;

            foreach (Basket item in baskets)
            {
                total = (double)(total + (item.Count * (item.Product.DiscountPrice > 0 ? item.Product.DiscountPrice : item.Product.Price)));

                OrderItem orderItem = new OrderItem
                {
                    Count = item.Count,
                    Price = ((double)(item.Product.DiscountPrice > 0 ? item.Product.DiscountPrice : item.Product.Price)),
                    ProductId = item.ProductId,
                    TotalPrice = ((double)(item.Count * (item.Product.DiscountPrice > 0 ? item.Product.DiscountPrice : item.Product.Price))),
                    CreatedAt = DateTime.UtcNow.AddHours(4)
                };
                orderItems.Add(orderItem);
            }

            Order order = new Order
            {
                Address = orderVM.Address,
                AppUserId = appUser.Id,
                City = orderVM.City,
                Country = orderVM.Country,
                State = orderVM.State,
                TotalPrice = total,
                CreatedAt = DateTime.UtcNow.AddHours(4),
                ZipCode = orderVM.ZipCode,
                OrderItems = orderItems
            };

            _context.Baskets.RemoveRange(baskets);
            HttpContext.Response.Cookies.Delete("basket");
            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();

            return RedirectToAction("index", "home");
        }

        public async Task<IActionResult> GetMyOrder(int? orderId, string UserName)
        {
            AppUser appUser = _userManager.Users.FirstOrDefault(x => x.UserName == UserName);

            if (appUser != null) return BadRequest();

            if (orderId == null) return BadRequest();
            Order order = await _context.Orders
                .Include(o=>o.OrderItems).ThenInclude(p=>p.Product)
                .FirstOrDefaultAsync(o => o.Id == orderId);
            if (order == null) return NotFound();


            return View(order);
        }
    }
}
