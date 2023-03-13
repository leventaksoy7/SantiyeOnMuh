using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SantiyeOnMuh.Business.Abstract;
using SantiyeOnMuh.Entity;
using SantiyeOnMuh.WebUI.Models;
using SantiyeOnMuh.WebUI.Models.Modeller;
using System.ComponentModel.DataAnnotations;

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
                CreateMessage($"{_santiyeGiderKalemi.Ad} GİDER KALEMİ EKLENDİ.", "success");

                return RedirectToAction("Index", "Admin");
            };

            CreateMessage(_santiyeGiderKalemiService.ErrorMessage, "danger");

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

            return RedirectToAction("Index", "Admin");
        }




        private void CreateMessage(string message, string alertType)
        {
            AlertMessage msg = new AlertMessage()
            {
                //Message = $"{_bankaHesap.HesapAdi} HESABI AÇILDI.",
                Message = message,
                AlertType = alertType
            };

            TempData["message"] = JsonConvert.SerializeObject(msg);
        }
    }
}
