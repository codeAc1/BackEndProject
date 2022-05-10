using Juan.DAL;
using Juan.Extensions;
using Juan.Helpers;
using Juan.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Juan.Areas.Admin.Controllers
{
    [Area ("Admin")]
    [Authorize(Roles = "Admin,Publisher,Manager")]
    public class ProductController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;
        public ProductController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public async Task<IActionResult> Index(bool? status, int page = 1)
        {
            ViewBag.Status = status;


            IQueryable<Product> products = _context.Products
                .Include(p => p.Brand)
                .Include(p => p.Category)
                .Include(p => p.ProductTags).ThenInclude(pt => pt.Tag)
                .Include(p=>p.productSizes).ThenInclude(ps=>ps.Size)
                .Include(p=>p.ProductColors).ThenInclude(pc=>pc.Color)
                .OrderByDescending(c => c.Id)
                .AsQueryable();

            if (status != null)
                products = products.Where(c => c.IsDeleted == status);

            ViewBag.PageIndex = page;
            ViewBag.PageCount = Math.Ceiling((double)products.Count() / 5);
            return View(await products.Skip((page - 1) * 5).Take(5).ToListAsync());
        }
        public async Task<IActionResult> Create(bool? status, int page = 1)
        {
            ViewBag.Brands = await _context.Brands.Where(b => !b.IsDeleted).ToListAsync();
            ViewBag.Categories = await _context.Categories.Where(c => !c.IsDeleted).ToListAsync();
            ViewBag.Tags = await _context.Tags.Where(t => !t.IsDeleted).ToListAsync();
            ViewBag.Colors = await _context.Colors.Where(c => !c.IsDeleted).ToListAsync();
            ViewBag.Sizes = await _context.Sizes.Where(s => !s.IsDeleted).ToListAsync();

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product product, bool? status, int page = 1)
        {
            ViewBag.Brands = await _context.Brands.Where(b => !b.IsDeleted).ToListAsync();
            ViewBag.Categories = await _context.Categories.Where(b => !b.IsDeleted).ToListAsync();
            ViewBag.Tags = await _context.Tags.Where(t => !t.IsDeleted).ToListAsync();
            ViewBag.Colors = await _context.Colors.Where(c => !c.IsDeleted).ToListAsync();
            ViewBag.Sizes = await _context.Sizes.Where(s => !s.IsDeleted).ToListAsync();

            if (!ModelState.IsValid)
            {
                return View();
            }
            if (product.ProductImagesFile == null)
            {
                ModelState.AddModelError("ProductImagesFile", "Sekil secilmeyib minimum 1 sekil secilmelidir");
                return View();
            }
            if (product.ProductImagesFile.Count() > 6)
            {
                ModelState.AddModelError("ProductImagesFile", $"maksimum yukleyebileceyin say 6");
                return View();
            }

            if (!await _context.Brands.AnyAsync(b => b.Id == product.BrandId && !b.IsDeleted))
            {
                ModelState.AddModelError("BrandId", "Duzgun Brand Secin");
                return View();
            }

            if (!await _context.Categories.AnyAsync(b => b.Id == product.CategoryId && !b.IsDeleted))
            {
                ModelState.AddModelError("CategoryId", "Duzgun Category Secin ");
                return View(product);
            }

            if (product.TagIds.Count > 0)
            {
                List<ProductTag> productTags = new List<ProductTag>();

                foreach (int item in product.TagIds)
                {
                    if (!await _context.Tags.AnyAsync(t => t.Id != item && !t.IsDeleted))
                    {
                        ModelState.AddModelError("TagIds", $"Secilen Id {item} - li Tag Yanlisdir");
                        return View();
                    }

                    ProductTag productTag = new ProductTag
                    {
                        TagId = item
                    };

                    productTags.Add(productTag);
                }

                product.ProductTags = productTags;
            }
            if (product.ColorIds.Count == 0)
            {
                ModelState.AddModelError("ColorIds", "Minimum bir reng secilmelidir ");
                return View(product);
            }
            if (product.ColorIds.Count > 0)
            {
                List<ProductColor> productColors = new List<ProductColor>();

                foreach (int item in product.ColorIds)
                {
                    if (!await _context.Colors.AnyAsync(t => t.Id != item && !t.IsDeleted))
                    {
                        ModelState.AddModelError("ColorIds", $"Secilen Id {item} - li Color Yanlisdir");
                        return View();
                    }

                    ProductColor productColor = new ProductColor
                    {
                        ColorId = item
                    };

                    productColors.Add(productColor);
                }

                product.ProductColors = productColors;
            }
            if (product.SizeIds.Count == 0)
            {
                ModelState.AddModelError("SizeIds", "Minimum bir olcu secilmelidir ");
                return View();
            }
            if (product.SizeIds.Count > 0)
            {
                List<ProductSize> productSizes = new List<ProductSize>();

                foreach (int item in product.SizeIds)
                {
                    if (!await _context.Sizes.AnyAsync(t => t.Id != item && !t.IsDeleted))
                    {
                        ModelState.AddModelError("SizeIds", $"Secilen Id {item} - li olcu Yanlisdir");
                        return View();
                    }

                    ProductSize productSize = new ProductSize
                    {
                        SizeId = item
                    };

                    productSizes.Add(productSize);
                }

                product.productSizes = productSizes;
            }
            if (product.MainImageFile != null)
            {
                if (!product.MainImageFile.CheckFileContentType("image/jpeg"))
                {
                    ModelState.AddModelError("MainImageFile", "Secilen Seklin Novu Uygun deyil");
                    return View();
                }

                if (!product.MainImageFile.CheckFileSize(3000))
                {
                    ModelState.AddModelError("MainImageFile", "Secilen Seklin Olcusu Maksimum 300 Kb Ola Biler");
                    return View();
                }

                product.MainImage = product.MainImageFile.CreateFile(_env, "user", "assets", "img", "product");
            }
            else
            {
                ModelState.AddModelError("MainImageFile", "Main Sekil Mutleq Secilmelidir");
                return View(product);
            }



            if (product.ProductImagesFile.Count() > 0)
            {
                List<ProductImage> productImages = new List<ProductImage>();

                foreach (IFormFile file in product.ProductImagesFile)
                {
                    if (!file.CheckFileContentType("image/jpeg"))
                    {
                        ModelState.AddModelError("ProductImagesFile", "Secilen Seklin Novu Uygun deyil");
                        return View(product);
                    }

                    if (!file.CheckFileSize(3000))
                    {
                        ModelState.AddModelError("ProductImagesFile", "Secilen Seklin Olcusu Maksimum 300 Kb Ola Biler");
                        return View(product);
                    }

                    ProductImage productImage = new ProductImage
                    {
                        Image = file.CreateFile(_env, "user", "assets", "img", "product"),
                        CreatedAt = DateTime.UtcNow.AddHours(4)
                    };

                    productImages.Add(productImage);
                }

                product.ProductImages = productImages;
            }
            else
            {
                ModelState.AddModelError("ProductImagesFile", "Product Images File Sekil Mutleq Secilmelidir");
                return View(product);
            }

            product.Availability = product.Count > 0 ? true : false;
            product.CreatedAt = DateTime.UtcNow.AddHours(4);

            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();

            return RedirectToAction("index", new { status, page });
        }

        public async Task<IActionResult> Update(int? id, bool? status, int page = 1)
        {
            ViewBag.Brands = await _context.Brands.Where(b => !b.IsDeleted).ToListAsync();
            ViewBag.Categories = await _context.Categories.Where(c => !c.IsDeleted).ToListAsync();
            ViewBag.Tags = await _context.Tags.Where(t => !t.IsDeleted).ToListAsync();
            ViewBag.Colors = await _context.Colors.Where(c => !c.IsDeleted).ToListAsync();
            ViewBag.Sizes = await _context.Sizes.Where(s => !s.IsDeleted).ToListAsync();

            if (id == null) return BadRequest();

            Product product = await _context.Products
                .Include(p => p.ProductTags).ThenInclude(pt => pt.Tag)
                .Include(p => p.productSizes).ThenInclude(ps => ps.Size)
                .Include(p => p.ProductColors).ThenInclude(pc => pc.Color)
                .Include(p => p.ProductImages).FirstOrDefaultAsync(p => p.Id == id);

            if (product == null) return NotFound();

            product.TagIds = product.ProductTags.Select(pt => pt.Tag.Id).ToList();
            product.ColorIds = product.ProductColors.Select(pt => pt.Color.Id).ToList();
            product.SizeIds = product.productSizes.Select(pt => pt.Size.Id).ToList();

            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? id, Product product, bool? status, int page = 1)
        {
            ViewBag.Brands = await _context.Brands.Where(b => !b.IsDeleted).ToListAsync();
            ViewBag.Categories = await _context.Categories.Where(c => !c.IsDeleted).ToListAsync();
            ViewBag.Tags = await _context.Tags.Where(t => !t.IsDeleted).ToListAsync();
            ViewBag.Colors = await _context.Colors.Where(c => !c.IsDeleted).ToListAsync();
            ViewBag.Sizes = await _context.Sizes.Where(s => !s.IsDeleted).ToListAsync();

            if (id == null) return BadRequest();

            if (id != product.Id) return BadRequest();

            Product dbProduct = await _context.Products
                .Include(p => p.ProductTags).ThenInclude(pt => pt.Tag)
                .Include(p => p.ProductImages)
                .Include(p => p.productSizes).ThenInclude(ps => ps.Size)
                .Include(p => p.ProductColors).ThenInclude(pc => pc.Color)
                .FirstOrDefaultAsync(p => p.Id == id && !p.IsDeleted);

            if (dbProduct == null) return NotFound();

            if (!ModelState.IsValid) return View(dbProduct);

            int canuploadimage = 6 - (int)(dbProduct.ProductImages?.Where(p => !p.IsDeleted).Count());

            if (product.ProductImagesFile != null && product.ProductImagesFile.Length > canuploadimage)
            {
                ModelState.AddModelError("ProductImagesFile", $"maksimum yukleyebileceyin say {canuploadimage}");
                return View(dbProduct);
            }

            if (!await _context.Brands.AnyAsync(b => b.Id == product.BrandId && !b.IsDeleted))
            {
                ModelState.AddModelError("BrandId", "Duzgun Brand Secin");
                return View(product);
            }

            if (!await _context.Categories.AnyAsync(b => b.Id == product.CategoryId && !b.IsDeleted))
            {
                ModelState.AddModelError("CategoryId", "Duzgun Category Secin ");
                return View(product);
            }

            

            if (product.TagIds.Count > 0)
            {
                _context.ProductTags.RemoveRange(dbProduct.ProductTags);

                List<ProductTag> productTags = new List<ProductTag>();

                foreach (int item in product.TagIds)
                {
                    if (!await _context.Tags.AnyAsync(t => t.Id != item && !t.IsDeleted))
                    {
                        ModelState.AddModelError("TagIds", $"Secilen Id {item} - li Tag Yanlisdir");
                        return View(product);
                    }

                    ProductTag productTag = new ProductTag
                    {
                        TagId = item
                    };

                    productTags.Add(productTag);
                }

                dbProduct.ProductTags = productTags;
            }
            else
            {
                _context.ProductTags.RemoveRange(dbProduct.ProductTags);
            }
            if (product.ColorIds.Count==0)
            {
                ModelState.AddModelError("ColorIds", "Minimum bir reng secilmelidir ");
                //product.MainImage = dbProduct.MainImage
                return View(dbProduct);
            }
            if (product.ColorIds.Count > 0)
            {
                _context.ProductColors.RemoveRange(dbProduct.ProductColors);

                List<ProductColor> productColors = new List<ProductColor>();

                foreach (int item in product.ColorIds)
                {
                    if (!await _context.Colors.AnyAsync(t => t.Id != item && !t.IsDeleted))
                    {
                        ModelState.AddModelError("ColorIds", $"Secilen Id {item} - li Color Yanlisdir");
                        return View(product);
                    }

                    ProductColor productColor = new ProductColor
                    {
                        ColorId = item
                    };

                    productColors.Add(productColor);
                }

                dbProduct.ProductColors = productColors;
            }
            else
            {
                _context.ProductColors.RemoveRange(dbProduct.ProductColors);
            }
            if (product.SizeIds.Count == 0)
            {
                ModelState.AddModelError("SizeIds", "Minimum bir olcu secilmelidir ");
                return View();
            }
            if (product.SizeIds.Count > 0)
            {
                _context.ProductSizes.RemoveRange(dbProduct.productSizes);

                List<ProductSize> productSizes = new List<ProductSize>();

                foreach (int item in product.SizeIds)
                {
                    if (!await _context.Sizes.AnyAsync(t => t.Id != item && !t.IsDeleted))
                    {
                        ModelState.AddModelError("SizeIds", $"Secilen Id {item} - li Size Yanlisdir");
                        return View(product);
                    }

                    ProductSize productSize = new ProductSize
                    {
                        SizeId = item
                    };

                    productSizes.Add(productSize);
                }

                dbProduct.productSizes = productSizes;
            }
            else
            {
                _context.ProductSizes.RemoveRange(dbProduct.productSizes);
            }



            if (product.MainImageFile != null)
            {
                if (!product.MainImageFile.CheckFileContentType("image/jpeg"))
                {
                    ModelState.AddModelError("MainImageFile", "Secilen Seklin Novu Uygun");
                    return View();
                }

                if (!product.MainImageFile.CheckFileSize(300))
                {
                    ModelState.AddModelError("MainImageFile", "Secilen Seklin Olcusu Maksimum 300 Kb Ola Biler");
                    return View();
                }
                Helper.DeleteFile(_env, dbProduct.MainImage, "user", "assets", "img", "product");

                dbProduct.MainImage = product.MainImageFile.CreateFile(_env, "user", "assets", "img", "product");
            }


            if (product.ProductImagesFile != null && product.ProductImagesFile.Count() > 0)
            {
                List<ProductImage> productImages = new List<ProductImage>();

                foreach (IFormFile file in product.ProductImagesFile)
                {
                    if (!file.CheckFileContentType("image/jpeg"))
                    {
                        ModelState.AddModelError("ProductImagesFile", "Secilen Seklin Novu Uygun");
                        return View();
                    }

                    if (!file.CheckFileSize(3000))
                    {
                        ModelState.AddModelError("ProductImagesFile", "Secilen Seklin Olcusu Maksimum 300 Kb Ola Biler");
                        return View();
                    }

                    ProductImage productImage = new ProductImage
                    {
                        Image = file.CreateFile(_env, "user", "assets", "img", "product"),
                        CreatedAt = DateTime.UtcNow.AddHours(4)
                    };

                    dbProduct.ProductImages.Add(productImage);
                }
            }

            dbProduct.BrandId = product.BrandId;
            dbProduct.CategoryId = product.CategoryId;
            dbProduct.Title = product.Title;
            dbProduct.Price = product.Price;
            dbProduct.DiscountPrice = product.DiscountPrice;
            dbProduct.Description = product.Description;
            dbProduct.Count = product.Count;
            dbProduct.Availability = product.Count > 0 ? true : false;
            dbProduct.UpdatedAt = DateTime.UtcNow.AddHours(4);

            await _context.SaveChangesAsync();

            return RedirectToAction("index", new { status, page });
        }
        public async Task<IActionResult> DeleteImage(int? id)
        {
            
            ViewBag.Brands = await _context.Brands.Where(b => !b.IsDeleted).ToListAsync();
            ViewBag.Categories = await _context.Categories.Where(c => !c.IsDeleted).ToListAsync();
            ViewBag.Tags = await _context.Tags.Where(t => !t.IsDeleted).ToListAsync();
            ViewBag.Colors = await _context.Colors.Where(c => !c.IsDeleted).ToListAsync();
            ViewBag.Sizes = await _context.Sizes.Where(s => !s.IsDeleted).ToListAsync();

            if (id == null) return BadRequest();


            Product product = await _context.Products
                .Include(p => p.ProductImages)
                .Include(p => p.ProductTags).ThenInclude(pt => pt.Tag)
                .Include(p => p.productSizes).ThenInclude(ps => ps.Size)
                .Include(p => p.ProductColors).ThenInclude(pc => pc.Color)
                .FirstOrDefaultAsync(p => p.ProductImages.Any(pi => pi.Id == id && !pi.IsDeleted));
            if (product.ProductImages.Count() == 1)
            {
                return View(product);
            }
            if (product == null) return NotFound();


            Helper.DeleteFile(_env, product.ProductImages.FirstOrDefault(pi=>pi.Id == id).Image, "user", "assets", "img", "product");

            _context.ProductImages.Remove(product.ProductImages.FirstOrDefault(pi => pi.Id == id));
            await _context.SaveChangesAsync();

            product.TagIds = product.ProductTags.Select(pt => pt.Tag.Id).ToList();
            product.ColorIds = product.ProductColors.Select(pt => pt.Color.Id).ToList();
            product.SizeIds = product.productSizes.Select(pt => pt.Size.Id).ToList();

            return PartialView("_ProductUpdatePartial", product);
        }
        public async Task<IActionResult> Detail(int? id, bool? status, int page = 1)
        {
            if (id == null) return BadRequest();

            Product product = await _context.Products
                .Include(p => p.ProductTags).ThenInclude(pt => pt.Tag)
                .Include(p => p.ProductImages)
                .Include(p => p.Brand)
                .Include(p => p.productSizes).ThenInclude(ps => ps.Size)
                .Include(p => p.ProductColors).ThenInclude(pc => pc.Color)
                .Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null) return NotFound();

            return View(product);
        }

        public async Task<IActionResult> Delete(int? id, bool? status, int page = 1)
        {
            ViewBag.Status = status;
            ViewBag.PageIndex = page;
            if (id == null) return BadRequest();

            Product product = await _context.Products
                .Include(p => p.ProductTags).ThenInclude(pt => pt.Tag)
                .Include(p => p.ProductImages)
                .Include(p => p.Brand)
                .Include(p => p.Category)
                .Include(p => p.productSizes).ThenInclude(ps => ps.Size)
                .Include(p => p.ProductColors).ThenInclude(pc => pc.Color)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null) return NotFound();

            return View(product);
        }
       
        public async Task<IActionResult> DeleteRestore(int? id, bool? status, int page = 1)
        {
            if (id == null) return BadRequest();

            Product product = await _context.Products
                .Include(p => p.ProductTags).ThenInclude(pt => pt.Tag)
                .Include(p => p.ProductImages)
                .Include(p => p.Brand)
                .Include(p => p.Category)
                .Include(p => p.productSizes).ThenInclude(ps => ps.Size)
                .Include(p => p.ProductColors).ThenInclude(pc => pc.Color)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null) return NotFound();
            if (product.IsDeleted)
            {
                product.IsDeleted = false;
                product.DeletedAt = null;
            }
            else
            {
                product.IsDeleted = true;
                product.DeletedAt = DateTime.UtcNow.AddHours(4);
            }


            await _context.SaveChangesAsync();

            return RedirectToAction("index", new { status, page });
        }
    }
}
