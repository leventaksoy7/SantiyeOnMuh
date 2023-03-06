using Microsoft.AspNetCore.Mvc;
using SantiyeOnMuh.Business.Abstract;
using SantiyeOnMuh.WebUI.Models;

namespace SantiyeOnMuh.WebUI.Controllers
{
    public class AdminController : Controller
    {
        private ISantiyeGiderKalemiService _santiyeGiderKalemiService;
        private ICariGiderKalemiService _cariGiderKalemiService;
        private IBankaHesapService _bankaHesapService;
        private ISirketService _sirketService;
        public AdminController(

            ISantiyeGiderKalemiService santiyeGiderKalemiService,
            ICariGiderKalemiService cariGiderKalemiService,
            IBankaHesapService bankaHesapService,
            ISirketService sirketService)
        {

            this._santiyeGiderKalemiService = santiyeGiderKalemiService;
            this._cariGiderKalemiService = cariGiderKalemiService;
            this._bankaHesapService = bankaHesapService;
            this._sirketService = sirketService;
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
