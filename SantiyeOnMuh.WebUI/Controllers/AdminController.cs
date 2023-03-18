using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SantiyeOnMuh.Business.Abstract;
using SantiyeOnMuh.WebUI.Identity;
using SantiyeOnMuh.WebUI.Models;

namespace SantiyeOnMuh.WebUI.Controllers
{
    [Authorize(Roles = "Admin")]
    //[ValidateAntiForgeryToken]
    public class AdminController : Controller
    {
        private ISantiyeGiderKalemiService _santiyeGiderKalemiService;
        private ICariGiderKalemiService _cariGiderKalemiService;
        private IBankaHesapService _bankaHesapService;
        private ISirketService _sirketService;
        private RoleManager<IdentityRole> _roleManager;
        private UserManager<User> _userManager;
        public AdminController(

            ISantiyeGiderKalemiService santiyeGiderKalemiService,
            ICariGiderKalemiService cariGiderKalemiService,
            IBankaHesapService bankaHesapService,
            ISirketService sirketService,
            RoleManager<IdentityRole> roleManager,
            UserManager<User> userManager)
        {
            this._santiyeGiderKalemiService = santiyeGiderKalemiService;
            this._cariGiderKalemiService = cariGiderKalemiService;
            this._bankaHesapService = bankaHesapService;
            this._sirketService = sirketService;
            this._roleManager = roleManager;
            this._userManager = userManager;
        }

        public IActionResult UserList()
        {
            return View(_userManager.Users);
        }

        //public async Task<IActionResult> UserEdit(string id)
        //{
        //    var user = await _userManager.FindByIdAsync(id);

        //    if (user != null)
        //    {
        //        var selectedRoles = await _userManager.GetRolesAsync(user);
        //        var roles = _roleManager.Roles.Select(i => i.Name);

        //        ViewBag.Roles = roles;

        //        return View(new UserDetailsModel()
        //        {
        //            UserId = user.Id,
        //            UserName = user.UserName,
        //            FirstName = user.Ad,
        //            LastName = user.SoyAd,
        //            SantiyeId = user.SantiyeId,
        //        });
        //    }

        //    return View();
        //}

        public IActionResult RoleList() 
        { 
            return View(_roleManager.Roles); 
        }

        [HttpGet]
        public async Task<IActionResult> RoleEdit(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);

            var members = new List<User>();
            var nonmembers = new List<User>();

            foreach (var user in _userManager.Users.ToList())
            {
                //--AYNI ŞEY FARKLI YAZIMINI DA ÖRNEK OLSUN DİYE BIRAKIYORUM---
                
                var list = await _userManager.IsInRoleAsync(user, role.Name)?members:nonmembers;
                list.Add(user);

                //if (await _userManager.IsInRoleAsync(user, role.Name))
                //{
                //    members.Add(user);
                //}
                //else
                //{
                //    nonmembers.Add(user);
                //}
            }

            var model = new RoleDetails()
            {
                Role = role,
                Members = members,
                Nonmembers = nonmembers
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> RoleEdit(RoleEditModel model)
        {
            if (ModelState.IsValid)
            {
                foreach(var userId in model.IdsToAdd ?? new string[] { })
                {
                    var user = await _userManager.FindByIdAsync(userId);
                    if (user != null)
                    {
                        var result = await _userManager.AddToRoleAsync(user,model.RoleName);
                        if (!result.Succeeded)
                        {
                            foreach(var error in result.Errors)
                            {
                                ModelState.AddModelError("",error.Description);
                            }
                        }
                    }
                }

                foreach (var userId in model.IdsToDelete ?? new string[] { })
                {
                    var user = await _userManager.FindByIdAsync(userId);
                    if (user != null)
                    {
                        var result = await _userManager.RemoveFromRoleAsync(user, model.RoleName);
                        if (!result.Succeeded)
                        {
                            foreach (var error in result.Errors)
                            {
                                ModelState.AddModelError("", error.Description);
                            }
                        }
                    }
                }
                //return View("RoleEdit","Admin");
            }
            return RedirectToAction("RoleList", "Admin");
        }

        [HttpGet]
        public IActionResult RoleCreate() 
        { 
            return View(); 
        }

        [HttpPost]
        public async Task<IActionResult> RoleCreate(RoleModel roleModel) 
        {
            if(ModelState.IsValid) 
            { 
                var result = await _roleManager.CreateAsync(new IdentityRole(roleModel.Name));
                if(result.Succeeded)
                {
                    return RedirectToAction("RoleList", "Admin");
                }

                foreach(var error in result.Errors)
                {
                    ModelState.AddModelError("",error.Description);
                }

                return View(roleModel);
            }
            return View(roleModel); 
        }

        public IActionResult Index()
        {
            ViewBag.Sayfa = "ADMİN PANELİ SİLİNMİŞLER LİSTESİ";
            var adminViewModel = new AdminViewListModel()
            {
                SantiyeGiderKalemis = _santiyeGiderKalemiService.GetAll(true, true),
                CariGiderKalemis = _cariGiderKalemiService.GetAll(true, true),
                BankaHesaps = _bankaHesapService.GetAll(true),
                Sirkets = _sirketService.GetAll(true),
            };
            return View(adminViewModel);
        }

        public IActionResult Arsiv()
        {
            ViewBag.Sayfa = "ADMİN PANELİ SİLİNMİŞLER LİSTESİ";
            var adminViewModel = new AdminViewListModel()
            {
                SantiyeGiderKalemis = _santiyeGiderKalemiService.GetAll(false),
                CariGiderKalemis = _cariGiderKalemiService.GetAll(false),
                BankaHesaps = _bankaHesapService.GetAll(false),
                Sirkets = _sirketService.GetAll(false),
            };
            return View(adminViewModel);
        }
    }
}
