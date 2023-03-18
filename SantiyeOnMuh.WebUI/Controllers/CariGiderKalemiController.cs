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
        public IActionResult CariGiderKalemiEkleme(CariGiderKalemi cariGiderKalemi)
        {
            if (!ModelState.IsValid) { return View(cariGiderKalemi); }

            /* View'larda kullanılan modeller, entitylerin WebUI içindeki kopyaları
             * WebUI ve Entity katmanlarındaki modelleri new'lediğim için
             * CRUD işlemlerinde verileri bir modelden diğerine aktarmak zorundayım.
             * Form tarafında data annotation ve validation kullanımında
             * Sürekli entity katmanına gitmek yerine
             * WebUI katmanındaki modeller üzerinde değişiklik yapıyorum.
             * entity katmanında sadece saf database deseni var.
            */

            ECariGiderKalemi _cariGiderKalemi = new ECariGiderKalemi()
            {
                Ad = cariGiderKalemi.Ad,
                Durum = cariGiderKalemi.Durum,
                Tur = cariGiderKalemi.Tur,
                CariKasas = cariGiderKalemi.CariKasas,
            };

            if (_cariGiderKalemiService.Create(_cariGiderKalemi))
            {
                TempData.Put("message", new AlertMessage()
                {
                    Title = "BAŞARILI",
                    AlertType = "success",
                    Message = $"{_cariGiderKalemi.Ad} GİDER KALEMİ EKLENDİ."
                });

                return RedirectToAction("Index", "Admin");
            };

            TempData.Put("message", new AlertMessage()
            {
                Title = "HATA",
                AlertType = "danger",
                Message = _cariGiderKalemiService.ErrorMessage
            });

            return View(_cariGiderKalemi);

        }

        [HttpGet]
        public IActionResult CariGiderKalemiGuncelle(int? id)
        {
            ViewBag.Sayfa = "CARİ KALEM BİLGİLERİNİ GÜNCELLEME";

            if (id == null) { return NotFound(); }

            ECariGiderKalemi cariGiderKalemi = _cariGiderKalemiService.GetById((int)id);

            CariGiderKalemi _cariGiderKalemi = new CariGiderKalemi()
            {
                Id = cariGiderKalemi.Id,
                Ad = cariGiderKalemi.Ad,
                Durum = cariGiderKalemi.Durum,
                Tur = cariGiderKalemi.Tur,
                CariKasas = cariGiderKalemi.CariKasas,
            };

            if (cariGiderKalemi == null) { return NotFound(); }

            return View(_cariGiderKalemi);
        }

        [HttpPost]
        public IActionResult CariGiderKalemiGuncelle(CariGiderKalemi c)
        {
            if (!ModelState.IsValid) { return View(c); }

            var _giderKalemi = _cariGiderKalemiService.GetById(c.Id);

            if (_giderKalemi == null) { return NotFound(); }

            _giderKalemi.Ad = c.Ad;

            _cariGiderKalemiService.Update(_giderKalemi);

            TempData.Put("message", new AlertMessage()
            {
                Title = "BAŞARILI",
                AlertType = "success",
                Message = $"{c.Ad} GÜNCELLENDİ."
            });

            return RedirectToAction("Index", "Admin");
        }

        [HttpGet]
        public IActionResult CariGiderKalemiSil(int? id)
        {
            if (id == null){return NotFound();}

            ECariGiderKalemi cariGiderKalemi = _cariGiderKalemiService.GetById((int)id);

            if (cariGiderKalemi == null){return NotFound();}

            cariGiderKalemi.Durum = false;

            _cariGiderKalemiService.Update(cariGiderKalemi);

            TempData.Put("message", new AlertMessage()
            {
                Title = "BAŞARILI",
                AlertType = "danger",
                Message = $"{cariGiderKalemi.Ad} SİLİNDİ."
            });

            return RedirectToAction("Index", "Admin");
        }

        [HttpGet]
        public IActionResult CariGiderKalemiGeriYukle(int? id)
        {
            if (id == null) { return NotFound(); }

            ECariGiderKalemi cariGiderKalemi = _cariGiderKalemiService.GetById((int)id);

            if (cariGiderKalemi == null) { return NotFound(); }

            cariGiderKalemi.Durum = true;

            _cariGiderKalemiService.Update(cariGiderKalemi);

            TempData.Put("message", new AlertMessage()
            {
                Title = "BAŞARILI",
                AlertType = "success",
                Message = $"{cariGiderKalemi.Ad} GERİ YÜKLENDİ."
            });

            return RedirectToAction("Index", "Admin");
        }

    }
}
