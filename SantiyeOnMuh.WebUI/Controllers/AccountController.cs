using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SantiyeOnMuh.Business.Abstract;
using SantiyeOnMuh.WebUI.Extensions;
using SantiyeOnMuh.WebUI.Identity;
using SantiyeOnMuh.WebUI.Models;
using SantiyeOnMuh.WebUI.Models.Modeller;

namespace SantiyeOnMuh.WebUI.Controllers
{
    [Authorize(Roles = "Admin")]
    //[ValidateAntiForgeryToken] //CROSSIDE ATTACKLAR ICIN -post tokenları control ediyor
    public class AccountController : Controller
    {
        private UserManager<User> _userManager;
        private SignInManager<User> _signInManager;
        private RoleManager<IdentityRole> _roleManager;
        private ISantiyeService _santiyeService;

        public AccountController(UserManager<User> userManager,
                                ISantiyeService santiyeService,
                                SignInManager<User> signInManager,
                                RoleManager<IdentityRole> roleManager)
        {
            this._santiyeService = santiyeService;
            this._userManager = userManager;
            this._signInManager = signInManager;
            this._roleManager = roleManager;
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [AllowAnonymous]
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
                if ((await _userManager.IsInRoleAsync(user, "Admin")) || (await _userManager.IsInRoleAsync(user, "Ofis")))
                {

                    TempData.Put("message", new AlertMessage()
                    {
                        Title = "BAŞARILI",
                        AlertType = "success",
                        Message = $"{user.UserName} GİRİŞ BAŞARILI"
                    });

                    return RedirectToAction("Anasayfa", "AnaSayfa");
                }

                if (await _userManager.IsInRoleAsync(user, "Santiye"))
                {
                    if (user.SantiyeId!=null)
                    {
                        int santiyeid = user.SantiyeId.GetValueOrDefault();

                        TempData.Put("message", new AlertMessage()
                        {
                            Title = "BAŞARILI",
                            AlertType = "success",
                            Message = $"{user.UserName} GİRİŞ BAŞARILI"
                        });

                        return RedirectToAction("SantiyeKasa", "SantiyeKasa", new { @santiyeid = santiyeid });
                    }
                    
                    return RedirectToAction("SantiyeKasa", "SantiyeKasa");
                    //return RedirectToAction("LogOut", "Account");
                    
                }
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

        [AllowAnonymous]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "account");
        }

        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> UserEdit(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            ViewBag.Santiye = _santiyeService.GetAll(true);

            UserDetailsModel model = new UserDetailsModel()
            {
                UserId = user.Id,
                UserName = user.UserName,
                FirstName = user.Ad,
                LastName = user.SoyAd,
                SantiyeId = null
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> UserEdit(UserDetailsModel model)
        {
            if (model.UserId != null)
            {
                var user = await _userManager.FindByIdAsync(model.UserId);

                if (user != null)
                {
                    user.SantiyeId = model.SantiyeId;

                    var result = await _userManager.UpdateAsync(user);

                    TempData.Put("message", new AlertMessage()
                    {
                        Title = "BAŞARILI",
                        AlertType = "success",
                        Message = $"{user.Ad} KULLANICI GÜNCELLENDİ"
                    });

                    return RedirectToAction("RoleList", "Admin");
                }
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> UserDelete(UserDetailsModel model)
        {
            if (model.UserId != null)
            {
                var user = await _userManager.FindByIdAsync(model.UserId);

                if (user != null)
                {
                    user.SantiyeId = model.SantiyeId;

                    var result = await _userManager.DeleteAsync(user);

                    TempData.Put("message", new AlertMessage()
                    {
                        Title = "BAŞARILI",
                        AlertType = "danger",
                        Message = $"{user.Ad} KULLANICI SİLİNDİ"
                    });

                    return RedirectToAction("RoleList", "Admin");
                }
            }

            TempData.Put("message", new AlertMessage()
            {
                Title = "HATA",
                AlertType = "danger",
                Message = "KULLANICI SİLİNEMEDİ"
            });

            return RedirectToAction("RoleList", "Admin");
        }
    }
}
