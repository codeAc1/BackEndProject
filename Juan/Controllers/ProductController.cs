using Juan.DAL;
using Juan.Models;
using Juan.ViewModels;
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
        public ProductController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
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
    }
}
