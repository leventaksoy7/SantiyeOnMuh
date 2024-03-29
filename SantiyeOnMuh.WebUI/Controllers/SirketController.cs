﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SantiyeOnMuh.Business.Abstract;
using SantiyeOnMuh.Entity;
using SantiyeOnMuh.WebUI.Extensions;
using SantiyeOnMuh.WebUI.Models;
using SantiyeOnMuh.WebUI.Models.Modeller;

namespace SantiyeOnMuh.WebUI.Controllers
{
    //[ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin")]
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
        public IActionResult SirketEkleme(Sirket sirket)
        {
            if (!ModelState.IsValid) { return View(sirket); }

            ESirket _sirket = new ESirket()
            {
                Ad=sirket.Ad,
                VergiNo=sirket.VergiNo,
                Durum=sirket.Durum,
                Ceks=sirket.Ceks,
                Nakits=sirket.Nakits,
            };

            if (_sirketService.Create(_sirket))
            {

                TempData.Put("message", new AlertMessage()
                {
                    Title = "BAŞARILI",
                    AlertType = "success",
                    Message = $"{_sirket.Ad} ŞİRKETİ AÇILDI."
                });

                return RedirectToAction("Index", "Admin");
            };

            TempData.Put("message", new AlertMessage()
            {
                Title = "HATA",
                AlertType = "danger",
                Message = _sirketService.ErrorMessage
            });

            return View(sirket);
        }

        [HttpGet]
        public IActionResult SirketGuncelle(int? id)
        {
            ViewBag.Sayfa = "ŞİRKET BİLGİLERİNİ BİLGİLERİNİ GÜNCELLEME";

            if (id == null){return NotFound();}

            ESirket sirket = _sirketService.GetById((int)id);

            if (sirket == null){return NotFound();}

            Sirket _sirket = new Sirket()
            {
                Id = sirket.Id,
                Ad = sirket.Ad,
                VergiNo = sirket.VergiNo,
                Durum = sirket.Durum,
                Ceks = sirket.Ceks,
                Nakits = sirket.Nakits,
            };

            return View(_sirket);
        }

        [HttpPost]
        public IActionResult SirketGuncelle(Sirket sirket)
        {
            if (!ModelState.IsValid) { return View(sirket); }

            var _sirket = _sirketService.GetById(sirket.Id);

            if (_sirket == null){return NotFound();}

            _sirket.Ad = sirket.Ad;
            _sirket.VergiNo = sirket.VergiNo;

            _sirketService.Update(_sirket);

            TempData.Put("message", new AlertMessage()
            {
                Title = "BAŞARILI",
                AlertType = "success",
                Message = $"{_sirket.Ad} ŞİRKETİ GÜNCELLENDİ."
            });

            return RedirectToAction("Index", "Admin");
        }

        [HttpGet]
        public IActionResult SirketSil(int? id)
        {
            if (id == null){return NotFound();}

            ESirket sirket = _sirketService.GetById((int)id);

            if (sirket == null){ return NotFound();}

            sirket.Durum = false;

            _sirketService.Update(sirket);

            TempData.Put("message", new AlertMessage()
            {
                Title = "BAŞARILI",
                AlertType = "success",
                Message = $"{sirket.Ad} ŞİRKETİ SİLİNDİ."
            });

            return RedirectToAction("Index", "Admin");
        }

        [HttpGet]
        public IActionResult SirketGeriYukle(int? id)
        {
            if (id == null){return NotFound();}

            ESirket sirket = _sirketService.GetById((int)id);

            if (sirket == null){return NotFound();}

            sirket.Durum = true;

            _sirketService.Update(sirket);

            TempData.Put("message", new AlertMessage()
            {
                Title = "BAŞARILI",
                AlertType = "success",
                Message = $"{sirket.Ad} ŞİRKETİ GERİ EKLENDİ."
            });

            return RedirectToAction("Index", "Admin");
        }
    }
}
