using Juan.DAL;
using Juan.Extensions;
using Juan.Helpers;
using Juan.Models;
using Juan.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Juan.Helpers.Helper;

namespace Juan.Controllers
{
    public class AccountController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IWebHostEnvironment _env;
        public AccountController
            (
                UserManager<AppUser> userManager,
                SignInManager<AppUser> signInManager,
                AppDbContext context,
                RoleManager<IdentityRole> roleManager,
                IWebHostEnvironment env
            )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
            _roleManager = roleManager;
            _env = env;
        }

        [Authorize(Roles = "Member")]
        public async Task <IActionResult> MyAccount()
        {
            AppUser appUser = await _userManager.Users.FirstOrDefaultAsync(u => u.UserName == User.Identity.Name && !u.isAdmin);

            if (appUser == null) return RedirectToAction("index", "home");

            UserProfileVM userProfileVM = new UserProfileVM
            {
                Member = new UserUpdateVM
                {
                    
                    Name=appUser.Name,
                    SurName=appUser.SurName,
                    UserName = appUser.UserName,
                    Email = appUser.Email,
                    Address=appUser.Address,
                    City=appUser.City,
                    Country=appUser.Country,
                    ZipCode=appUser.ZipCode,
                    PhoneNumber=appUser.PhoneNumber,
                    State=appUser.State,
                    Image=appUser.Image
                    
                    
                },
                Orders = await _context.Orders
                .Include(o => o.OrderItems).ThenInclude(oi => oi.Product)
                .Include(o => o.AppUser)
                .Where(o => !o.IsDeleted && o.AppUserId == appUser.Id).ToListAsync()

            };

            return View(userProfileVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ProfilUpdate(UserUpdateVM member)
        {
            AppUser appUser = await _userManager.Users.FirstOrDefaultAsync(u => u.UserName == User.Identity.Name && !u.isAdmin);

            if (appUser == null) return RedirectToAction("index", "home");
            UserProfileVM userProfileVM = new UserProfileVM
            {
                Member = member
            };
            TempData["ProfileTab"] = "Account";

            if (!ModelState.IsValid)
            {
                return View("MyAccount", userProfileVM);
            }

            if (appUser.NormalizedUserName != member.UserName.ToUpper() && await _userManager.Users.AnyAsync(u => u.NormalizedUserName == member.UserName.ToUpper()))
            {
                ModelState.AddModelError("UserName", "UserName Alreade Exist");

                return View("MyAccount", userProfileVM);
            }

            if (appUser.NormalizedEmail != member.Email.ToUpper() && await _userManager.Users.AnyAsync(u => u.NormalizedEmail == member.Email.ToUpper()))
            {
                ModelState.AddModelError("Email", "Email Alreade Exist");
                return View("MyAccount", userProfileVM);
            }
            if (member.ImageFile!=null)
            {
                if (!member.ImageFile.CheckFileContentType("image/jpeg"))
                {
                    ModelState.AddModelError("ImageFile", "Secilen Seklin Novu Uygun deyil ancaq png secile biler");
                    return View("MyAccount", userProfileVM);
                }

                if (!member.ImageFile.CheckFileSize(3000))
                {
                    ModelState.AddModelError("ImageFile", "Secilen Seklin Olcusu Maksimum 300 Kb Ola Biler");
                    return View("MyAccount", userProfileVM);
                }
                if (appUser.Image!=null)
                {
                    Helper.DeleteFile(_env, appUser.Image, "user", "assets", "img", "userImage");
                }
                

                appUser.Image = member.ImageFile.CreateFile(_env, "user", "assets", "img", "usersImage");
            }

            appUser.Name = member.Name;
            appUser.SurName = member.SurName;
            appUser.UserName = member.UserName;
            appUser.Email = member.Email;
            appUser.Address = member.Address;
            appUser.Country = member.Country;
            appUser.City = member.City;
            appUser.State = member.State;
            appUser.ZipCode = member.ZipCode;
            appUser.PhoneNumber = member.PhoneNumber;

            IdentityResult identityResult = await _userManager.UpdateAsync(appUser);

            if (!identityResult.Succeeded)
            {
                foreach (var item in identityResult.Errors)
                {
                    ModelState.AddModelError("", item.Description);
                }
                return View("MyAccount", userProfileVM);
            }

            if (member.Password != null)
            {
                if (string.IsNullOrWhiteSpace(member.CurrentPassword))
                {
                    ModelState.AddModelError("CurrentPassword", "CurrentPassword Is Requered");
                    return View("MyAccount", userProfileVM);
                }

                if (!await _userManager.CheckPasswordAsync(appUser, member.CurrentPassword))
                {
                    ModelState.AddModelError("CurrentPassword", "CurrentPassword Is InCorrect");
                    return View("MyAccount", userProfileVM);
                }

                string token = await _userManager.GeneratePasswordResetTokenAsync(appUser);
                identityResult = await _userManager.ResetPasswordAsync(appUser, token, member.Password);
                if (!identityResult.Succeeded)
                {
                    foreach (var item in identityResult.Errors)
                    {
                        ModelState.AddModelError("", item.Description);
                    }
                    return View("MyAccount", userProfileVM);
                }
            }

            return RedirectToAction("MyAccount");
        }
        
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginVM login)
        {
            if (!ModelState.IsValid) return View();
            AppUser user = await _userManager.FindByNameAsync(login.UserName);
            if (user == null)
            {
                ModelState.AddModelError("", "Username or Password is wrong");
                return View();
            }
            if (user.IsDeleted)
            {
                ModelState.AddModelError("", "User Deactived");
                return View();
            }
            var signInResult = await _signInManager.PasswordSignInAsync(user, login.Password, login.RememberMe, true);

            if (signInResult.IsLockedOut)
            {
                ModelState.AddModelError("", $"User is blocket wait 5 min");
                return View();
            }

            if (!signInResult.Succeeded)
            {
                ModelState.AddModelError("", "Username or Password is wrong");
                return View();
            }

            if ((await _userManager.GetRolesAsync(user))[0] == Roles.Member.ToString())
            {
                string coockieBasket = HttpContext.Request.Cookies["basket"];

                if (!string.IsNullOrWhiteSpace(coockieBasket))
                {
                    List<BasketVM> basketVMs = JsonConvert.DeserializeObject<List<BasketVM>>(coockieBasket);

                    List<Basket> baskets = new List<Basket>();
                    List<Basket> existedBasket = await _context.Baskets.Where(b => b.AppUserId == user.Id).ToListAsync();
                    foreach (BasketVM basketVM in basketVMs)
                    {
                        if (existedBasket.Any(b => b.ProductId == basketVM.ProductId))
                        {
                            existedBasket.Find(b => b.ProductId == basketVM.ProductId).Count = basketVM.Count;

                        }

                        else
                        {
                            Basket basket = new Basket
                            {
                                AppUserId = user.Id,
                                ProductId = basketVM.ProductId,
                                Count = basketVM.Count,
                                //ColorId=basketVM.Color,
                                //SizeId=basketVM.Size,
                                CreatedAt = DateTime.UtcNow.AddHours(4)
                            };

                            baskets.Add(basket);
                        }


                    }

                    if (baskets.Count > 0)
                    {
                        await _context.Baskets.AddRangeAsync(baskets);
                        await _context.SaveChangesAsync();
                    }
                }
            }

            if ((await _userManager.GetRolesAsync(user))[0] == Roles.Admin.ToString())
            {
                return RedirectToAction("Index", "Dashboard", new { area = "Admin" });
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }


        }


        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterVM register)
        {
            if (!ModelState.IsValid) return View();

            AppUser newUser = new AppUser
            {
                Name = register.Name,
                SurName = register.SurName,
                UserName = register.UserName,
                Email = register.Email

            };

            IdentityResult identityResult = await _userManager.CreateAsync(newUser, register.Password);
            if (!identityResult.Succeeded)
            {
                foreach (IdentityError error in identityResult.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View();

            }
            //await _userManager.AddToRoleAsync(newUser, Roles.Admin.ToString());
            await _userManager.AddToRoleAsync(newUser, Roles.Member.ToString());
            await _signInManager.SignInAsync(newUser, true);

            return RedirectToAction("Index", "Home");
        }
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        #region Create Role
        //public async Task CreateRoles()
        //{
        //    foreach (var role in Enum.GetValues(typeof(Roles)))
        //    {
        //        if (!(await _roleManager.RoleExistsAsync(role.ToString())))
        //        {
        //            await _roleManager.CreateAsync(new IdentityRole { Name = role.ToString() });
        //        }
        //    }
        //}
        #endregion
    }
}
