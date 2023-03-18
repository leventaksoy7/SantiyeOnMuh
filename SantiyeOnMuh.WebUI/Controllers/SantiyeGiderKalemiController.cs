using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SantiyeOnMuh.Business.Abstract;
using SantiyeOnMuh.Entity;
using SantiyeOnMuh.WebUI.Extensions;
using SantiyeOnMuh.WebUI.Models;
using SantiyeOnMuh.WebUI.Models.Modeller;
using System.ComponentModel.DataAnnotations;

namespace SantiyeOnMuh.WebUI.Controllers
{
    //[ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin")]
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
        public IActionResult SantiyeGiderKalemiEkleme(SantiyeGiderKalemi santiyeGiderKalemi)
        {
            if (!ModelState.IsValid) { return View(santiyeGiderKalemi); }

            ESantiyeGiderKalemi _santiyeGiderKalemi = new ESantiyeGiderKalemi()
            {
                Ad = santiyeGiderKalemi.Ad,
                Durum = santiyeGiderKalemi.Durum,
                Tur = santiyeGiderKalemi.Tur,
                SantiyeKasas = santiyeGiderKalemi.SantiyeKasas,
            };

            if (_santiyeGiderKalemiService.Create(_santiyeGiderKalemi))
            {
                TempData.Put("message", new AlertMessage()
                {
                    Title = "BAŞARILI",
                    AlertType = "success",
                    Message = $"{_santiyeGiderKalemi.Ad} GİDER KALEMİ EKLENDİ."
                });

                return RedirectToAction("Index", "Admin");
            };

            TempData.Put("message", new AlertMessage()
            {
                Title = "HATA",
                AlertType = "danger",
                Message = _santiyeGiderKalemiService.ErrorMessage
            });

            return View(santiyeGiderKalemi);
        }

        [HttpGet]
        public IActionResult SantiyeGiderKalemiGuncelle(int? id)
        {
            ViewBag.Sayfa = "ŞANTİYE GİDER KALEMİ BİLGİLERİNİ GÜNCELLEME";

            if (id == null){return NotFound();}

            ESantiyeGiderKalemi santiyeGiderKalemi = _santiyeGiderKalemiService.GetById((int)id);

            if (santiyeGiderKalemi == null){return NotFound();}

            SantiyeGiderKalemi _santiyeGiderKalemi = new SantiyeGiderKalemi()
            {
                Id = santiyeGiderKalemi.Id,
                Ad = santiyeGiderKalemi.Ad,
                Durum = santiyeGiderKalemi.Durum,
                Tur = santiyeGiderKalemi.Tur,
                SantiyeKasas = santiyeGiderKalemi.SantiyeKasas,
            };

            return View(_santiyeGiderKalemi);
        }

        [HttpPost]
        public IActionResult SantiyeGiderKalemiGuncelle(SantiyeGiderKalemi santiyeGiderKalemi)
        {
            if (!ModelState.IsValid) { return View(santiyeGiderKalemi); }

            ESantiyeGiderKalemi _santiyeGiderKalemi = _santiyeGiderKalemiService.GetById(santiyeGiderKalemi.Id);

            if (_santiyeGiderKalemi == null){return NotFound();}

            _santiyeGiderKalemi.Ad = santiyeGiderKalemi.Ad;

            _santiyeGiderKalemiService.Update(_santiyeGiderKalemi);

            TempData.Put("message", new AlertMessage()
            {
                Title = "BAŞARILI",
                AlertType = "success",
                Message = $"{_santiyeGiderKalemi.Ad} GİDER KALEMİ GÜNCELLENDİ."
            });

            return RedirectToAction("Index", "Admin");
        }

        [HttpGet]
        public IActionResult SantiyeGiderKalemiSil(int? id)
        {
            if (id == null){return NotFound();}

            ESantiyeGiderKalemi santiyeGiderKalemi = _santiyeGiderKalemiService.GetById((int)id);

            if (santiyeGiderKalemi == null){return NotFound();}

            santiyeGiderKalemi.Durum = false;

            _santiyeGiderKalemiService.Update(santiyeGiderKalemi);

            TempData.Put("message", new AlertMessage()
            {
                Title = "BAŞARILI",
                AlertType = "success",
                Message = $"{santiyeGiderKalemi.Ad} GİDER KALEMİ SİLİNDİ."
            });

            return RedirectToAction("Index", "Admin");
        }

        [HttpGet]
        public IActionResult SantiyeGiderKalemiGeriYukle(int? id)
        {
            if (id == null){return NotFound();}

            ESantiyeGiderKalemi santiyeGiderKalemi = _santiyeGiderKalemiService.GetById((int)id);

            if (santiyeGiderKalemi == null){return NotFound();}

            santiyeGiderKalemi.Durum = true;

            _santiyeGiderKalemiService.Update(santiyeGiderKalemi);

            TempData.Put("message", new AlertMessage()
            {
                Title = "BAŞARILI",
                AlertType = "success",
                Message = $"{santiyeGiderKalemi.Ad} GİDER KALEMİ GERİ EKLENDİ."
            });

            return RedirectToAction("Index", "Admin");
        }
    }
}
