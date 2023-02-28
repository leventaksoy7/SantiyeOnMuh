using Microsoft.AspNetCore.Mvc;
using SantiyeOnMuh.Business.Abstract;
using SantiyeOnMuh.Entity;
using SantiyeOnMuh.WebUI.Models;

namespace SantiyeOnMuh.WebUI.Controllers
{
    public class SantiyeController : Controller
    {
        // _ olan nesnemizle artık işlemlerimizi gerçekleştireceğiz
        private ISantiyeService _santiyeService;
        private ISantiyeKasaService _santiyeKasaService;

        public SantiyeController(ISantiyeService santiyeService, ISantiyeKasaService santiyeKasaService)
        {
            this._santiyeService = santiyeService;
            this._santiyeKasaService = santiyeKasaService;

        }
        public IActionResult Index()
        {
            ViewBag.Sayfa = "ŞANTİYELER";

            var santiyeViewModel = new SantiyeViewListModel()
            {
                Santiyes = _santiyeService.GetAll(true),
                SantiyeKasas = _santiyeKasaService.GetAll(null, null, true),
            };

            var aktifsantiyesayisi = santiyeViewModel.Santiyes.Count();

            decimal?[] gelir = new decimal?[aktifsantiyesayisi];
            decimal?[] gider = new decimal?[aktifsantiyesayisi];
            decimal?[] netbakiye = new decimal?[aktifsantiyesayisi];

            int sayi = 0;

            foreach (var santiye in santiyeViewModel.Santiyes)
            {
                gelir[sayi] = (decimal?)_santiyeKasaService.GetAll((int)santiye.Id, null, true).Sum(i => i.Gelir);
                gider[sayi] = (decimal?)_santiyeKasaService.GetAll((int)santiye.Id, null, true).Sum(i => i.Gider);
                netbakiye[sayi] = gelir[sayi] - gider[sayi];
                sayi = sayi + 1;
            }

            ViewBag.Net = netbakiye;

            return View(santiyeViewModel);
        }
        [HttpGet]
        public IActionResult SantiyeEkleme()
        {
            ViewBag.Sayfa = "YENİ ŞANTİYE EKLEME";
            return View();
        }
        [HttpPost]
        public IActionResult SantiyeEkleme(Santiye s)
        {
            _santiyeService.Create(s);
            return RedirectToAction("Index");
        }
        [HttpGet]
        public IActionResult SantiyeGuncelle(int? id)
        {
            ViewBag.Sayfa = "ŞANTİYE BİLGİLERİNİ GÜNCELLEME";
            if (id == null)
            {
                return NotFound();
            }
            Santiye santiye = _santiyeService.GetById((int)id);
            if (santiye == null)
            {
                return NotFound();
            }
            return View(santiye);
        }
        [HttpPost]
        public IActionResult SantiyeGuncelle(Santiye s)
        {
            var entitySantiye = _santiyeService.GetById(s.Id);
            if (entitySantiye == null)
            {
                return NotFound();
            }
            entitySantiye.Ad = s.Ad;
            entitySantiye.Adres = s.Adres;

            _santiyeService.Update(entitySantiye);
            return RedirectToAction("Index");
        }
        [HttpGet]
        public IActionResult SantiyeSil(int? id)
        {
            ViewBag.Sayfa = "ŞANTİYEYİ SİL";

            if (id == null) { return NotFound(); }

            Santiye santiye = _santiyeService.GetById((int)id);

            if (santiye == null) { return NotFound(); }

            return View(santiye);
        }
        [HttpPost]
        public IActionResult SantiyeSil(Santiye s)
        {
            var entity = _santiyeService.GetById(s.Id);

            if (entity == null) { return NotFound(); }

            entity.Durum = false;

            _santiyeService.Update(entity);
            return RedirectToAction("Index");
        }

        //ARŞİV
        public IActionResult IndexArsiv()
        {
            ViewBag.Sayfa = "ŞANTİYELER";

            var santiyeViewModel = new SantiyeViewListModel()
            {
                Santiyes = _santiyeService.GetAll(),
                SantiyeKasas = _santiyeKasaService.GetAll(null, null, false),
            };

            var aktifsantiyesayisi = santiyeViewModel.Santiyes.Count();

            decimal?[] gelir = new decimal?[aktifsantiyesayisi];
            decimal?[] gider = new decimal?[aktifsantiyesayisi];
            decimal?[] netbakiye = new decimal?[aktifsantiyesayisi];

            int sayi = 0;

            foreach (var santiye in santiyeViewModel.Santiyes)
            {
                gelir[sayi] = (decimal?)_santiyeKasaService.GetAll((int)santiye.Id, null, false).Sum(i => i.Gelir);
                gider[sayi] = (decimal?)_santiyeKasaService.GetAll((int)santiye.Id, null, false).Sum(i => i.Gider);
                netbakiye[sayi] = gelir[sayi] - gider[sayi];
                sayi = sayi + 1;
            }

            ViewBag.Net = netbakiye;

            return View(santiyeViewModel);
        }
    }
}
