﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SantiyeOnMuh.Business.Abstract;
using SantiyeOnMuh.WebUI.Extensions;
using SantiyeOnMuh.WebUI.Identity;
using SantiyeOnMuh.WebUI.Models;

namespace SantiyeOnMuh.WebUI.Controllers
{
    //[Authorize(Roles ="Admin")]
    //[ValidateAntiForgeryToken] //CROSSIDE ATTACKLAR ICIN -post tokenları control ediyor
    public class AccountController : Controller
    {
        private UserManager<User> _userManager;
        private SignInManager<User> _signInManager;

        public AccountController(UserManager<User> userManager,
                                SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (!ModelState.IsValid) { return View(model); }

            var user = await _userManager.FindByNameAsync(model.UserName);

            if (user == null) 
            { 
                TempData.Put("message", new AlertMessage()
                {
                    Title = "HATA",
                    AlertType = "danger",
                    Message = "GİRİŞ BAŞARISIZ"
                });

                return View(model); 
            }

            var result = await _signInManager.PasswordSignInAsync(user, model.Password,false,true); //f=cookie silme t=user block

            if (result.Succeeded) 
            { 

                TempData.Put("message", new AlertMessage()
                {
                    Title = "BAŞARILI",
                    AlertType = "success",
                    Message = $"{user.UserName} GİRİŞ BAŞARILI"
                });

                return RedirectToAction("Anasayfa", "AnaSayfa");
            }

            TempData.Put("message", new AlertMessage()
            {
                Title = "HATA",
                AlertType = "danger",
                Message = "GİRİŞ BAŞARISIZ"
            });

            return View();
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if (!ModelState.IsValid) { return View(model); }

            var user = new User()
            {
                Ad = model.FirstName,
                SoyAd = model.LastName,

                UserName = model.UserName,
                //Email = model.Email,
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded) 
            {
                TempData.Put("message", new AlertMessage()
                {
                    Title = "BAŞARILI",
                    AlertType = "success",
                    Message = $"{user.Ad} KULLANICI ADI EKLENDİ"
                });
                return RedirectToAction("Index", "Admin"); 
            }

            TempData.Put("message", new AlertMessage()
            {
                Title = "HATA",
                AlertType = "danger",
                Message = "BİLİNMEYEN BİR HATA OLUŞTU, TEKRAR DENEYİNİZ."
            });

            ModelState.AddModelError("", "BİLİNMEYEN BİR HATA OLUŞTU, TEKRAR DENEYİNİZ");
            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "account");
        }
    }
}
