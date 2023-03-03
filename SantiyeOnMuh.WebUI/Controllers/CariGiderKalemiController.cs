using Microsoft.AspNetCore.Mvc;
using SantiyeOnMuh.Business.Abstract;
using SantiyeOnMuh.Entity;
using SantiyeOnMuh.WebUI.Models.Modeller;
using System.ComponentModel.DataAnnotations;

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
        public IActionResult CariGiderKalemiEkleme(CariGiderKalemi cariGiderKalemi)
        {
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

            _cariGiderKalemiService.Create(_cariGiderKalemi);

            return RedirectToAction("Index", "Admin");
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
            var _giderKalemi = _cariGiderKalemiService.GetById(c.Id);

            if (_giderKalemi == null)
            {
                return NotFound();
            }
            _giderKalemi.Ad = c.Ad;

            _cariGiderKalemiService.Update(_giderKalemi);

            return RedirectToAction("Index", "Admin");
        }
        [HttpGet]
        public IActionResult CariGKSil(int? id)
        {
            if (id == null){return NotFound();}

            ECariGiderKalemi cariGiderKalemi = _cariGiderKalemiService.GetById((int)id);

            if (cariGiderKalemi == null){return NotFound();}

            cariGiderKalemi.Durum = false;

            _cariGiderKalemiService.Update(cariGiderKalemi);

            return RedirectToAction("Index", "Admin");
        }
        [HttpGet]
        public IActionResult CariGKGeriYukle(int? id)
        {
            if (id == null){return NotFound();}

            ECariGiderKalemi cariGiderKalemi = _cariGiderKalemiService.GetById((int)id);

            if (cariGiderKalemi == null){return NotFound();}

            cariGiderKalemi.Durum = true;

            _cariGiderKalemiService.Update(cariGiderKalemi);

            return RedirectToAction("Index", "Admin");
        }
    }
}
