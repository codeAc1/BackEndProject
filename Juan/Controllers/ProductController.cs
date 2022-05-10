using Juan.DAL;
using Juan.Models;
using Juan.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Juan.Controllers
{
    public class ProductController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        public ProductController(AppDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }


        public async Task<IActionResult>  Index( int? categoryId, decimal? minPrice, decimal? maxPrice, List<int> colorIds, List<int> sizeIds, int page = 1)
        {

            var products = _context.Products
                .Include(x => x.Brand)
                .Include(x => x.Category)
                .Include(x => x.ProductColors)
                .Include(x => x.ProductImages)
                .Include(x => x.Reviews)
                .Where(x => !x.IsDeleted);

            ViewBag.CategoryId = categoryId;
            ViewBag.PageIndex = page;
            ViewBag.ColorIds = colorIds;
            ViewBag.SizeIds = colorIds;

            ViewBag.TotalProducts = products.Count();

            ProductListVM productListVM = new ProductListVM();



            if (categoryId != null)
            {
                products = products.Where(x => x.CategoryId == categoryId);
            }

            if (colorIds != null && colorIds.Count > 0)
                products = products.Where(x => x.ProductColors.Any(p => colorIds.Contains((int)p.ColorId)));

            if (sizeIds != null && sizeIds.Count > 0)
                products = products.Where(x => x.productSizes.Any(p => sizeIds.Contains((int)p.SizeId)));

            if (products.Any())
            {
                productListVM.MinPrice = (decimal)products.Min(x => x.Price);
                productListVM.MaxPrice = (decimal)products.Max(x => x.Price);
            }

            ViewBag.FilterMinPrice = minPrice ?? productListVM.MinPrice;
            ViewBag.FilterMaxPrice = maxPrice ?? productListVM.MaxPrice;

            if (minPrice != null && maxPrice != null)
                products = products.Where(x => x.Price>(double)(minPrice) && x.Price<(double)(maxPrice));

            ViewBag.TotalPages = (int)Math.Ceiling(products.Count() / 6d);

            productListVM.Settings = await _context.Settings.ToListAsync();
            productListVM.Products = products.Skip((page - 1) * 6).Take(6).ToList();
            productListVM.Categories =await _context.Categories.Include(x => x.Products).Where(x => !x.IsDeleted).ToListAsync();
            productListVM.Colors = await _context.Colors.Where(x => !x.IsDeleted).ToListAsync();
            productListVM.Sizes = await _context.Sizes.Where(x => !x.IsDeleted).ToListAsync();
            productListVM.FilterColorIds = colorIds;
            productListVM.FilterSizeIds = sizeIds;
            return View(productListVM);
        }

        public async Task<IActionResult> DetailModal(int? id)
        {
            if (id == null) return BadRequest();

            Product product = await _context.Products
                .Include(p => p.ProductImages)
                .Include(p=>p.Reviews)
                .Include(p=>p.ProductColors).ThenInclude(p=>p.Color)
                .Include(p => p.productSizes).ThenInclude(p => p.Size)
                .FirstOrDefaultAsync(p => p.Id == id && !p.IsDeleted);

            if (product == null) return NotFound();

            return PartialView("_ProductDetailPartial", product);
        }

        public async Task<IActionResult> ProductDetail(int? id)
        {
            if (id == null) return BadRequest();
            Product product = await _context.Products
                .Include(x => x.Brand)
                .Include(x => x.ProductImages)
                .Include(x => x.Category)
                .Include(x => x.ProductColors).ThenInclude(x => x.Color)
                .Include(x => x.productSizes).ThenInclude(x => x.Size)
                .Include(x => x.Reviews).ThenInclude(c => c.AppUser)
                .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
            if (product == null) return NotFound();

            ProductDetailViewModel ProductDetailVM = new ProductDetailViewModel
            {
                Product = product,
                Review = new Review { ProductId = id },
                Products = _context.Products.Include(x => x.ProductImages).Where(x => x.CategoryId == product.CategoryId).Take(6).ToList()
            };

            return View(ProductDetailVM);
        }
        public async Task<IActionResult> AddBasket(int? id, int count = 1,int colorId=1,int sizeId=1)
        {
            if (id == null) return BadRequest();

            Product product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);

            if (product == null) return NotFound();

            string cookieBasket = HttpContext.Request.Cookies["basket"];

            List<BasketVM> basketVMs = null;

            if (cookieBasket != null)
            {
                basketVMs = JsonConvert.DeserializeObject<List<BasketVM>>(cookieBasket);

                if (basketVMs.Any(b => b.ProductId == id && colorId==b.Color && sizeId==b.Size))
                {
                    basketVMs.Find(b => b.ProductId == id).Count += count;
                }
                else
                {

                    basketVMs.Add(new BasketVM
                    {
                        ProductId = (int)id,
                        Count = count,
                        Color=colorId,
                        Size=sizeId
                    });
                }
            }
            else
            {
                basketVMs = new List<BasketVM>();
                basketVMs.Add(new BasketVM
                {
                    ProductId = (int)id,
                    Count = count,
                    Color = colorId,
                    Size = sizeId
                });
            }

            cookieBasket = JsonConvert.SerializeObject(basketVMs);
            HttpContext.Response.Cookies.Append("basket", cookieBasket);

            foreach (BasketVM basketVM in basketVMs)
            {

                Product dbProduct = await _context.Products
                    .Include(x=>x.ProductColors).ThenInclude(x=>x.Color)
                    .Include(x=>x.productSizes).ThenInclude(x=>x.Size)
                    .FirstOrDefaultAsync(p => p.Id == basketVM.ProductId);
                basketVM.Image = dbProduct.MainImage;
                basketVM.Price = (double)(dbProduct.DiscountPrice > 0 ? dbProduct.DiscountPrice : dbProduct.Price);
                basketVM.Title = dbProduct.Title;
                
            }

            return PartialView("_BasketPartial", basketVMs);

            

        }

        public async Task<IActionResult>  AddReviews(Review review)
        {

            AppUser member = null;
            if (User.Identity.IsAuthenticated)
            {
                member = _userManager.Users.FirstOrDefault(x => x.UserName == User.Identity.Name );
            }
            if (member == null)
                return RedirectToAction("login", "account");

            Product product = await _context.Products
                 .Include(x => x.Brand)
                 .Include(x => x.ProductImages)
                 .Include(x => x.Category)
                 .Include(x => x.ProductColors).ThenInclude(x => x.Color)
                 .Include(x => x.productSizes).ThenInclude(x => x.Size)
                 .Include(x => x.Reviews).ThenInclude(c => c.AppUser)
                 .FirstOrDefaultAsync(x => x.Id == review.ProductId && !x.IsDeleted);
            if (product == null) return NotFound();
            
            if (!ModelState.IsValid)
            {

                ProductDetailViewModel ProductDetailVM = new ProductDetailViewModel
                {
                    Product = product,
                    Review = new Review { ProductId = review.ProductId },
                    Products = _context.Products.Include(x => x.ProductImages).Where(x => x.CategoryId == product.CategoryId).Take(6).ToList()
                };

                return View("ProductDetail", ProductDetailVM);
            }

            review.AppUserId = member.Id;


            review.CreatedAt = DateTime.UtcNow.AddHours(4);
            product.Reviews.Add(review);

            _context.SaveChanges();

            return RedirectToAction("ProductDetail", new { id = review.ProductId });
        }

    }
}
