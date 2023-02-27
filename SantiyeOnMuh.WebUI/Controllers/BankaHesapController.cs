using Microsoft.AspNetCore.Mvc;
using SantiyeOnMuh.Business.Abstract;
using SantiyeOnMuh.Entity;

namespace SantiyeOnMuh.WebUI.Controllers
{
    public class BankaHesapController : Controller
    {
        // NESNELER ÜZERİNDEKİ İŞLEMLERİ _ OLAN NESNE ÜZERİNDE YAPIP SONRA AKTARIYORUZ - INJECTION
        private IBankaHesapService _bankaHesapService;
        public BankaHesapController(IBankaHesapService bankaHesapService)
        {
            this._bankaHesapService = bankaHesapService;
        }
        [HttpGet]
        public IActionResult BankaHesapEkleme()
        {
            ViewBag.Sayfa = "YENİ BANKA HESABI EKLEME";
            return View();
        }
        [HttpPost]
        public IActionResult BankaHesapEkleme(BankaHesap b)
        {
            _bankaHesapService.Create(b);
            return RedirectToAction("Index", "Admin");
        }
        [HttpGet]
        public IActionResult BankaHesapGuncelle(int? id)
        {
            ViewBag.Sayfa = "BANKA HESAP BİLGİLERİNİ GÜNCELLEME";
            if (id == null)
            {
                return NotFound();
            }

            BankaHesap bankaHesap = _bankaHesapService.GetById((int)id);

            if (bankaHesap == null)
            {
                return NotFound();
            }
            return View(bankaHesap);
        }
        [HttpPost]
        public IActionResult BankaHesapGuncelle(BankaHesap b)
        {
            var entityBankaHesap = _bankaHesapService.GetById(b.Id);

            if (entityBankaHesap == null)
            {
                return NotFound();
            }
            entityBankaHesap.HesapAd = b.HesapAd;
            entityBankaHesap.BankaAd = b.BankaAd;
            entityBankaHesap.HesapNo = b.HesapNo;
            entityBankaHesap.Iban = b.Iban;

            _bankaHesapService.Update(entityBankaHesap);
            return RedirectToAction("Index", "Admin");
        }
        [HttpGet]
        public IActionResult BankaHesapSil(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            BankaHesap bankaHesap = _bankaHesapService.GetById((int)id);

            if (bankaHesap == null)
            {
                return NotFound();
            }

            bankaHesap.Durum = false;

            _bankaHesapService.Update(bankaHesap);

            return RedirectToAction("Index", "Admin");
        }
        [HttpGet]
        public IActionResult BankaHesapGeriYukle(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            BankaHesap bankaHesap = _bankaHesapService.GetById((int)id);

            if (bankaHesap == null)
            {
                return NotFound();
            }

            bankaHesap.Durum = true;

            _bankaHesapService.Update(bankaHesap);

            return RedirectToAction("Index", "Admin");
        }
    }
}
