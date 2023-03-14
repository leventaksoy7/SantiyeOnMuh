using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SantiyeOnMuh.Business.Abstract;
using SantiyeOnMuh.Entity;
using SantiyeOnMuh.WebUI.Extensions;
using SantiyeOnMuh.WebUI.Models;
using SantiyeOnMuh.WebUI.Models.Modeller;

namespace SantiyeOnMuh.WebUI.Controllers
{
    [ValidateAntiForgeryToken]
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
        public IActionResult BankaHesapEkleme(BankaHesap bankaHesap)
        {
            if (ModelState.IsValid)
            {

                /* View'larda kullanılan modeller, entitylerin WebUI içindeki kopyaları
                 * WebUI ve Entity katmanlarındaki modelleri new'lediğim için
                 * CRUD işlemlerinde verileri bir modelden diğerine aktarmak zorundayım.
                 * Form tarafında data annotation ve validation kullanımında
                 * Sürekli entity katmanına gitmek yerine
                 * WebUI katmanındaki modeller üzerinde değişiklik yapıyorum.
                 * entity katmanında sadece saf database deseni var.
                */

                EBankaHesap _bankaHesap = new EBankaHesap()
                {
                    BankaAdi = bankaHesap.BankaAdi,
                    HesapAdi = bankaHesap.HesapAdi,
                    HesapNo = bankaHesap.HesapNo,
                    IbanNo = bankaHesap.IbanNo,
                    Durum = bankaHesap.Durum,
                    Ceks = bankaHesap.Ceks,
                    Nakits = bankaHesap.Nakits,
                    BankaKasas = bankaHesap.BankaKasas,
                };

               if(_bankaHesapService.Create(_bankaHesap))
                {
                    TempData.Put("message", new AlertMessage()
                    {
                        Title = "BAŞARILI",
                        AlertType = "success",
                        Message = $"{_bankaHesap.HesapAdi} HESABI AÇILDI."
                    });

                    return RedirectToAction("Index", "Admin");
                };

                TempData.Put("message", new AlertMessage()
                {
                    Title = "HATA",
                    AlertType = "danger",
                    Message = _bankaHesapService.ErrorMessage
                });

                return View(bankaHesap);   
            }
            return View(bankaHesap);
        }

        [HttpGet]
        public IActionResult BankaHesapGuncelle(int? id)
        {
            ViewBag.Sayfa = "BANKA HESAP BİLGİLERİNİ GÜNCELLEME";
            if (id == null){return NotFound();}

            EBankaHesap bankaHesap = _bankaHesapService.GetById((int)id);

            if (bankaHesap == null){return NotFound();}

            /* View'larda kullanılan modeller, entitylerin WebUI içindeki kopyaları
             * WebUI ve Entity katmanlarındaki modelleri new'lediğim için
             * CRUD işlemlerinde verileri bir modelden diğerine aktarmak zorundayım.
             * Form tarafında data annotation ve validation kullanımında
             * Sürekli entity katmanına gitmek yerine
             * WebUI katmanındaki modeller üzerinde değişiklik yapıyorum.
             * entity katmanında sadece saf database deseni var.
            */

            var _bankaHesap = new BankaHesap()
            {
                Id=bankaHesap.Id,
                BankaAdi = bankaHesap.BankaAdi,
                HesapAdi = bankaHesap.HesapAdi,
                HesapNo = bankaHesap.HesapNo,
                IbanNo = bankaHesap.IbanNo,
                Durum = bankaHesap.Durum,
                Ceks = bankaHesap.Ceks,
                Nakits = bankaHesap.Nakits,
                BankaKasas = bankaHesap.BankaKasas,
            };

            return View(_bankaHesap);
        }

        [HttpPost]
        public IActionResult BankaHesapGuncelle(BankaHesap bankaHesap)
        {

            if (!ModelState.IsValid) 
            {
                TempData.Put("message", new AlertMessage()
                {
                    Title = "HATA",
                    AlertType = "danger",
                    Message = "BİR HATA OLUŞTU"
                });

                return View(bankaHesap); 
            }

            EBankaHesap _bankaHesap = _bankaHesapService.GetById(bankaHesap.Id);

            if (_bankaHesap == null)
            {
                return NotFound();
            }

            _bankaHesap.HesapAdi = bankaHesap.HesapAdi;
            _bankaHesap.BankaAdi = bankaHesap.BankaAdi;
            _bankaHesap.HesapNo = bankaHesap.HesapNo;
            _bankaHesap.IbanNo = bankaHesap.IbanNo;

            _bankaHesapService.Update(_bankaHesap);

            TempData.Put("message", new AlertMessage()
            {
                Title = "BAŞARILI",
                AlertType = "success",
                Message = $"{_bankaHesap.HesapAdi} GÜNCELLENDİ"
            });

            return RedirectToAction("Index", "Admin");
            
        }

        [HttpGet]
        public IActionResult BankaHesapSil(int? id)
        {
            if (id == null) { return NotFound(); }

            EBankaHesap bankaHesap = _bankaHesapService.GetById((int)id);

            if (bankaHesap == null) {return NotFound();}

            bankaHesap.Durum = false;

            _bankaHesapService.Update(bankaHesap);

            TempData.Put("message", new AlertMessage()
            {
                Title = "BAŞARILI",
                AlertType = "danger",
                Message = $"{bankaHesap.HesapAdi} SİLİNDİ"
            });

            return RedirectToAction("Index", "Admin");
        }

        [HttpGet]
        public IActionResult BankaHesapGeriYukle(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            EBankaHesap bankaHesap = _bankaHesapService.GetById((int)id);

            if (bankaHesap == null){return NotFound();}

            bankaHesap.Durum = true;

            _bankaHesapService.Update(bankaHesap);

            TempData.Put("message", new AlertMessage()
            {
                Title = "BAŞARILI",
                AlertType = "success",
                Message = $"{bankaHesap.HesapAdi} GERİ YÜKLENDİ"
            });

            return RedirectToAction("Index", "Admin");
        }

    }
}
