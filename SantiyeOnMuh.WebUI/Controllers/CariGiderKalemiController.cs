using Microsoft.AspNetCore.Mvc;
using SantiyeOnMuh.Business.Abstract;
using SantiyeOnMuh.Entity;

namespace SantiyeOnMuh.WebUI.Controllers
{
    public class CariGiderKalemiController : Controller
    {
        // NESNELER ÜZERİNDEKİ İŞLEMLERİ _ OLAN NESNE ÜZERİNDE YAPIP SONRA AKTARIYORUZ - INJECTION
        private ICariGiderKalemiService _cariGiderKalemiService;
        public CariGiderKalemiController(ICariGiderKalemiService cariGiderKalemiService)
        {
            this._cariGiderKalemiService = cariGiderKalemiService;
        }
        [HttpGet]
        public IActionResult CariGiderKalemiEkleme()
        {
            ViewBag.Sayfa = "CARİ HESAPLAR İÇİN YENİ GİDER KALEMİ EKLEME";
            return View();
        }
        [HttpPost]
        public IActionResult CariGiderKalemiEkleme(CariGiderKalemi c)
        {
            _cariGiderKalemiService.Create(c);
            return RedirectToAction("Index", "Admin");
        }
        [HttpGet]
        public IActionResult CariGiderKalemiGuncelle(int? id)
        {
            ViewBag.Sayfa = "CARİ KALEM BİLGİLERİNİ GÜNCELLEME";

            if (id == null) { return NotFound(); }
            CariGiderKalemi cariGiderKalemi = _cariGiderKalemiService.GetById((int)id);
            if (cariGiderKalemi == null) { return NotFound(); }

            return View(cariGiderKalemi);
        }
        [HttpPost]
        public IActionResult CariGiderKalemiGuncelle(CariGiderKalemi c)
        {
            var entityGiderKalemi = _cariGiderKalemiService.GetById(c.Id);
            if (entityGiderKalemi == null)
            {
                return NotFound();
            }
            entityGiderKalemi.Ad = c.Ad;

            _cariGiderKalemiService.Update(entityGiderKalemi);
            return RedirectToAction("Index", "Admin");
        }
        [HttpGet]
        public IActionResult CariGKSil(int? id)
        {

            if (id == null)
            {
                return NotFound();
            }

            CariGiderKalemi cariGiderKalemi = _cariGiderKalemiService.GetById((int)id);

            if (cariGiderKalemi == null)
            {
                return NotFound();
            }

            cariGiderKalemi.Durum = false;

            _cariGiderKalemiService.Update(cariGiderKalemi);

            return RedirectToAction("Index", "Admin");
        }
        [HttpGet]
        public IActionResult CariGKGeriYukle(int? id)
        {

            if (id == null)
            {
                return NotFound();
            }

            CariGiderKalemi cariGiderKalemi = _cariGiderKalemiService.GetById((int)id);

            if (cariGiderKalemi == null)
            {
                return NotFound();
            }

            cariGiderKalemi.Durum = true;

            _cariGiderKalemiService.Update(cariGiderKalemi);

            return RedirectToAction("Index", "Admin");
        }
    }
}
