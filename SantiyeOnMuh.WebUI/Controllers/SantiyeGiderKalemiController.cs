using Microsoft.AspNetCore.Mvc;
using SantiyeOnMuh.Business.Abstract;
using SantiyeOnMuh.Entity;

namespace SantiyeOnMuh.WebUI.Controllers
{
    public class SantiyeGiderKalemiController : Controller
    {
        // NESNELER ÜZERİNDEKİ İŞLEMLERİ _ OLAN NESNE ÜZERİNDE YAPIP SONRA AKTARIYORUZ - INJECTION
        private ISantiyeGiderKalemiService _santiyeGiderKalemiService;
        public SantiyeGiderKalemiController(ISantiyeGiderKalemiService santiyeGiderKalemiService)
        {
            this._santiyeGiderKalemiService = santiyeGiderKalemiService;
        }
        [HttpGet]
        public IActionResult SantiyeGiderKalemiEkleme()
        {
            ViewBag.Sayfa = "ŞANTİYE KASASI İÇİN YENİ GİDER KALEMİ EKLEME";
            return View();
        }
        [HttpPost]
        public IActionResult SantiyeGiderKalemiEkleme(ESantiyeGiderKalemi s)
        {
            _santiyeGiderKalemiService.Create(s);
            return RedirectToAction("Index", "Admin");
        }
        [HttpGet]
        public IActionResult SantiyeGiderKalemiGuncelle(int? id)
        {
            ViewBag.Sayfa = "ŞANTİYE GİDER KALEMİ BİLGİLERİNİ GÜNCELLEME";
            if (id == null)
            {
                return NotFound();
            }

            ESantiyeGiderKalemi santiyeGiderKalemi = _santiyeGiderKalemiService.GetById((int)id);

            if (santiyeGiderKalemi == null)
            {
                return NotFound();
            }
            return View(santiyeGiderKalemi);
        }
        [HttpPost]
        public IActionResult SantiyeGiderKalemiGuncelle(ESantiyeGiderKalemi s)
        {
            var entitySantiyeGiderKalemi = _santiyeGiderKalemiService.GetById(s.Id);
            if (entitySantiyeGiderKalemi == null)
            {
                return NotFound();
            }
            entitySantiyeGiderKalemi.Ad = s.Ad;

            _santiyeGiderKalemiService.Update(entitySantiyeGiderKalemi);
            return RedirectToAction("Index", "Admin");
        }
        [HttpGet]
        public IActionResult SantiyeGiderKalemiSil(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ESantiyeGiderKalemi santiyeGiderKalemi = _santiyeGiderKalemiService.GetById((int)id);

            if (santiyeGiderKalemi == null)
            {
                return NotFound();
            }

            santiyeGiderKalemi.Durum = false;

            _santiyeGiderKalemiService.Update(santiyeGiderKalemi);
            return RedirectToAction("Index", "Admin");
        }
        [HttpGet]
        public IActionResult SantiyeGiderKalemiGeriYukle(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ESantiyeGiderKalemi santiyeGiderKalemi = _santiyeGiderKalemiService.GetById((int)id);

            if (santiyeGiderKalemi == null)
            {
                return NotFound();
            }

            santiyeGiderKalemi.Durum = true;

            _santiyeGiderKalemiService.Update(santiyeGiderKalemi);
            return RedirectToAction("Index", "Admin");
        }
    }
}
