using Microsoft.AspNetCore.Mvc;
using SantiyeOnMuh.Business.Abstract;
using SantiyeOnMuh.Entity;
using SantiyeOnMuh.WebUI.Models.Modeller;

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

                _bankaHesapService.Create(_bankaHesap);

                return RedirectToAction("Index", "Admin");
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

            if (!ModelState.IsValid) { return View(bankaHesap); }

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

            return RedirectToAction("Index", "Admin");
            
        }
        [HttpGet]
        public IActionResult BankaHesapSil(int? id)
        {
            if (id == null){return NotFound();}

            EBankaHesap bankaHesap = _bankaHesapService.GetById((int)id);

            if (bankaHesap == null){return NotFound();}

            bankaHesap.Durum = false;

            _bankaHesapService.Update(bankaHesap);

            return RedirectToAction("Index", "Admin");
        }
        [HttpGet]
        public IActionResult BankaHesapGeriYukle(int? id)
        {
            if (id == null){return NotFound();}

            EBankaHesap bankaHesap = _bankaHesapService.GetById((int)id);

            if (bankaHesap == null){return NotFound();}

            bankaHesap.Durum = true;

            _bankaHesapService.Update(bankaHesap);

            return RedirectToAction("Index", "Admin");
        }
    }
}
