using DocumentFormat.OpenXml.Vml.Spreadsheet;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SantiyeOnMuh.Business.Abstract;
using SantiyeOnMuh.Entity;
using SantiyeOnMuh.WebUI.Models;
using SantiyeOnMuh.WebUI.Models.Modeller;
using System.ComponentModel.DataAnnotations;

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
                Santiyes = _santiyeService.GetAll(Convert.ToBoolean(true)),
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

        //ARŞİV
        public IActionResult IndexArsiv()
        {
            ViewBag.Sayfa = "ŞANTİYELER";

            var santiyeViewModel = new SantiyeViewListModel()
            {
                Santiyes = _santiyeService.GetAll(false),
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
        public IActionResult SantiyeEkleme(Santiye santiye)
        {
            if (!ModelState.IsValid) { return View(santiye); }

            ESantiye _santiye = new ESantiye()
            {
                Ad = santiye.Ad,
                Adres = santiye.Adres,
                Durum = santiye.Durum,
                SantiyeKasas = santiye.SantiyeKasas,
                CariHesaps = santiye.CariHesaps,
            };

            if (_santiyeService.Create(_santiye))
            {
                AlertMessage msg = new AlertMessage()
                {
                    Message = $"{_santiye.Ad} HESABI AÇILDI.",
                    AlertType = "success"
                };

                TempData["message"] = JsonConvert.SerializeObject(msg);

                return RedirectToAction("Index");
            };
            return View(santiye);
        }

        [HttpGet]
        public IActionResult SantiyeGuncelle(int? id)
        {
            ViewBag.Sayfa = "ŞANTİYE BİLGİLERİNİ GÜNCELLEME";

            if (id == null) { return NotFound(); }

            ESantiye santiye = _santiyeService.GetById((int)id);

            if (santiye == null) { return NotFound(); }

            Santiye _santiye = new Santiye()
            {
                Id = santiye.Id,
                Ad = santiye.Ad,
                Adres = santiye.Adres,
                Durum = santiye.Durum,
                SantiyeKasas = santiye.SantiyeKasas,
                CariHesaps = santiye.CariHesaps,
            };

            return View(_santiye);
        }

        [HttpPost]
        public IActionResult SantiyeGuncelle(Santiye santiye)
        {
            if (!ModelState.IsValid) { return View(santiye); }

            ESantiye _santiye = _santiyeService.GetById(santiye.Id);

            if (_santiye == null){return NotFound();}

            _santiye.Ad = santiye.Ad;
            _santiye.Adres = santiye.Adres;

            _santiyeService.Update(_santiye);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult SantiyeSil(int? id)
        {
            ViewBag.Sayfa = "ŞANTİYEYİ SİL";

            if (id == null) {return NotFound();}

            ESantiye santiye = _santiyeService.GetById((int)id);

            if (santiye == null) { return NotFound();}

            Santiye _santiye = new Santiye()
            {
                Id= santiye.Id,
                Ad = santiye.Ad,
                Adres = santiye.Adres,
                Durum = santiye.Durum,
                SantiyeKasas = santiye.SantiyeKasas,
                CariHesaps = santiye.CariHesaps,
            };

            return View(_santiye);
        }

        [HttpPost]
        public IActionResult SantiyeSil(Santiye santiye)
        {
            ESantiye _santiye = _santiyeService.GetById(santiye.Id);

            if (_santiye == null) { return NotFound(); }

            _santiye.Durum = false;

            _santiyeService.Update(_santiye);

            return RedirectToAction("Index");
        }

        
        public IActionResult SantiyeGeriYukle(int? id)
        {
            if (id == null) { return NotFound(); }

            ESantiye santiye = _santiyeService.GetById((int)id);

            if (santiye == null) { return NotFound(); }

            santiye.Durum = true;

            _santiyeService.Update(santiye);

            return RedirectToAction("Index");
        }
    }
}
