using Microsoft.AspNetCore.Mvc;
using SantiyeOnMuh.Business.Abstract;
using SantiyeOnMuh.Entity;
using SantiyeOnMuh.WebUI.Models;

namespace SantiyeOnMuh.WebUI.Controllers
{
    public class SirketController : Controller
    {
        // NESNELER ÜZERİNDEKİ İŞLEMLERİ _ OLAN NESNE ÜZERİNDE YAPIP SONRA AKTARIYORUZ - INJECTION
        private ISirketService _sirketService;
        public SirketController(ISirketService sirketService)
        {
            this._sirketService = sirketService;
        }
        public IActionResult SilinmisSirketler()
        {
            ViewBag.Sayfa = "SİLİNMİŞ ŞİRKETLER";

            var sirketViewModel = new SirkerViewListModel()
            {
                Sirkets = _sirketService.GetAll(false)
            };

            return View(sirketViewModel);
        }
        [HttpGet]
        public IActionResult SirketEkleme()
        {
            ViewBag.Sayfa = "YENİ ŞİRKET EKLEME";
            return View();
        }
        [HttpPost]
        public IActionResult SirketEkleme(Sirket s)
        {
            _sirketService.Create(s);
            return RedirectToAction("Index", "Admin");
        }
        [HttpGet]
        public IActionResult SirketGuncelle(int? id)
        {
            ViewBag.Sayfa = "ŞİRKET BİLGİLERİNİ BİLGİLERİNİ GÜNCELLEME";
            if (id == null)
            {
                return NotFound();
            }

            Sirket sirket = _sirketService.GetById((int)id);

            if (sirket == null)
            {
                return NotFound();
            }
            return View(sirket);
        }
        [HttpPost]
        public IActionResult SirketGuncelle(Sirket s)
        {
            var entitySirket = _sirketService.GetById(s.Id);
            if (entitySirket == null)
            {
                return NotFound();
            }
            entitySirket.Ad = s.Ad;
            entitySirket.VergiNo = s.VergiNo;

            _sirketService.Update(entitySirket);
            return RedirectToAction("Index", "Admin");
        }
        [HttpGet]
        public IActionResult SirketSil(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Sirket sirket = _sirketService.GetById((int)id);

            if (sirket == null)
            {
                return NotFound();
            }

            sirket.Durum = false;

            _sirketService.Update(sirket);

            return RedirectToAction("Index", "Admin");
        }
        [HttpGet]
        public IActionResult SirketGeriYukle(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Sirket sirket = _sirketService.GetById((int)id);

            if (sirket == null)
            {
                return NotFound();
            }

            sirket.Durum = false;

            _sirketService.Update(sirket);

            return RedirectToAction("Index", "Admin");
        }
    }
}
