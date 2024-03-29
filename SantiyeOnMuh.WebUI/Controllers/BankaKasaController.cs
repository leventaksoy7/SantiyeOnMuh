﻿using ClosedXML.Excel;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SantiyeOnMuh.Business.Abstract;
using SantiyeOnMuh.Entity;
using SantiyeOnMuh.WebUI.Extensions;
using SantiyeOnMuh.WebUI.Models;
using SantiyeOnMuh.WebUI.Models.Modeller;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace SantiyeOnMuh.WebUI.Controllers
{
    //[ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin,Ofis")]
    public class BankaKasaController : Controller
    {
        // NESNELER ÜZERİNDEKİ İŞLEMLERİ _ OLAN NESNE ÜZERİNDE YAPIP SONRA AKTARIYORUZ - INJECTION
        private IBankaKasaService _bankaKasaService;
        private IBankaHesapService _bankaHesapService;
        private ISantiyeKasaService _santiyeKasaService;
        private ISantiyeService _santiyeService;
        public BankaKasaController(
            IBankaKasaService bankaKasaService,
            IBankaHesapService bankaHesapService,
            ISantiyeService santiyeService,
            ISantiyeKasaService santiyeKasaService)
        {
            this._bankaKasaService = bankaKasaService;
            this._bankaHesapService = bankaHesapService;
            this._santiyeService = santiyeService;
            this._santiyeKasaService = santiyeKasaService;
        }

        public IActionResult BankaKasa(int? bankahesapid, int page = 1)
        {
            ViewBag.Sayfa = "ANA KASA";

            const int pageSize = 10;
            var bankaKasaViewModel = new BankaKasaViewListModel()
            {
                PageInfo = new PageInfo
                {
                    TotalItem = _bankaKasaService.GetCount(bankahesapid, true),
                    CurrentPage = page,
                    ItemPerPage = pageSize,
                    UrlInfo = (int?)bankahesapid
                },

                BankaKasas = _bankaKasaService.GetAll((int?)bankahesapid, true, page, pageSize),
                BankaHesaps = _bankaHesapService.GetAll(true)
            };

            ViewBag.toplamgelir = _bankaKasaService.GetAll((int?)bankahesapid, true).Sum(i => i.Giren);
            ViewBag.toplamgider = _bankaKasaService.GetAll((int?)bankahesapid, true).Sum(i => i.Cikan);

            return View(bankaKasaViewModel);
        }

        [HttpGet]
        public IActionResult BankaKasaEkleme()
        {
            ViewBag.Sayfa = "ANA KASAYA VERİ GİRİŞ";
            ViewBag.bankahesap = _bankaHesapService.GetAll(true);

            return View();
        }

        [HttpPost]
        public IActionResult BankaKasaEkleme(BankaKasa bankaKasa)
        {
            ViewBag.Sayfa = "ANA KASAYA VERİ GİRİŞ";
            ViewBag.bankahesap = _bankaHesapService.GetAll(true);

            if (!ModelState.IsValid) { return View(bankaKasa); }

            /* View'larda kullanılan modeller, entitylerin WebUI içindeki kopyaları
             * WebUI ve Entity katmanlarındaki modelleri new'lediğim için
             * CRUD işlemlerinde verileri bir modelden diğerine aktarmak zorundayım.
             * Form tarafında data annotation ve validation kullanımında
             * Sürekli entity katmanına gitmek yerine
             * WebUI katmanındaki modeller üzerinde değişiklik yapıyorum.
             * entity katmanında sadece saf database deseni var.
            */


            EBankaKasa _bankaKasa = new EBankaKasa()
            {
                Tarih = bankaKasa.Tarih,
                Aciklama = bankaKasa.Aciklama,
                Nitelik = bankaKasa.Nitelik,
                //Giren = bankaKasa.Giren,
                //Cikan = bankaKasa.Cikan,
                #region VİRGÜL VEYA NOKTA KULLANIMININ İKİSİNİN DE SERBEST OLMASINI SAĞLAMAK İÇİN
                Giren = Convert.ToDecimal(bankaKasa.Giren.Replace(".", ",")),
                Cikan = Convert.ToDecimal(bankaKasa.Cikan.Replace(".", ",")),
                #endregion
                Durum = bankaKasa.Durum,
                CekKaynak = bankaKasa.CekKaynak,
                NakitKaynak = bankaKasa.NakitKaynak,
                SantiyeKasaKaynak = bankaKasa.SantiyeKasaKaynak,
                BankaHesapId = bankaKasa.BankaHesapId,
                //BankaHesap = bankaKasa.BankaHesap,
                SistemeGiris = bankaKasa.SistemeGiris,
                SonGuncelleme = bankaKasa.SonGuncelleme
            };

            if (_bankaKasaService.Create(_bankaKasa))
            {
                TempData.Put("message", new AlertMessage()
                {
                    Title = "BAŞARILI",
                    AlertType = "success",
                    Message = $"{_bankaKasa.Aciklama} HESABA EKLENDİ."
                });

                return RedirectToAction("BankaKasa");
            };

            TempData.Put("message", new AlertMessage()
            {
                Title = "HATA",
                AlertType = "danger",
                Message = _bankaKasaService.ErrorMessage
            });

            return View(bankaKasa);
        }

        [HttpGet]
        public IActionResult BankaKasaGuncelle(int? bankakasaid)
        {
            ViewBag.Sayfa = "ANA KASAYA VERİ GÜNCELLEME";
            ViewBag.bankahesap = _bankaHesapService.GetAll(true);

            if (bankakasaid == null) { return NotFound(); }

            EBankaKasa bankaKasa = _bankaKasaService.GetByIdDetay((int)bankakasaid);

            if (bankaKasa == null) { return NotFound(); }

            /* View'larda kullanılan modeller, entitylerin WebUI içindeki kopyaları
             * WebUI ve Entity katmanlarındaki modelleri new'lediğim için
             * CRUD işlemlerinde verileri bir modelden diğerine aktarmak zorundayım.
             * Form tarafında data annotation ve validation kullanımında
             * Sürekli entity katmanına gitmek yerine
             * WebUI katmanındaki modeller üzerinde değişiklik yapıyorum.
             * entity katmanında sadece saf database deseni var.
            */

            BankaKasa _bankaKasa = new BankaKasa()
            {
                Id = bankaKasa.Id,
                Tarih = bankaKasa.Tarih,
                Aciklama = bankaKasa.Aciklama,
                Nitelik = bankaKasa.Nitelik,
                //Giren = bankaKasa.Giren,
                //Cikan = bankaKasa.Cikan,
                #region VİRGÜL VEYA NOKTA KULLANIMININ İKİSİNİN DE SERBEST OLMASINI SAĞLAMAK İÇİN
                Giren = Convert.ToString(bankaKasa.Giren),
                Cikan = Convert.ToString(bankaKasa.Cikan),
                #endregion
                Durum = bankaKasa.Durum,
                CekKaynak = bankaKasa.CekKaynak,
                NakitKaynak = bankaKasa.NakitKaynak,
                SantiyeKasaKaynak = bankaKasa.SantiyeKasaKaynak,
                BankaHesapId = bankaKasa.BankaHesapId,
                BankaHesap = bankaKasa.BankaHesap,
                SistemeGiris = bankaKasa.SistemeGiris,
                SonGuncelleme = bankaKasa.SonGuncelleme
            };

            ViewBag.Giren = bankaKasa.Giren;
            ViewBag.Cikan = bankaKasa.Cikan;

            return View(_bankaKasa);
        }

        [HttpPost]
        public IActionResult BankaKasaGuncelle(BankaKasa bankaKasa)
        {
            ViewBag.Sayfa = "ANA KASAYA VERİ GÜNCELLEME";
            ViewBag.bankahesap = _bankaHesapService.GetAll(true);

            if (!ModelState.IsValid) { return View(bankaKasa); }

            EBankaKasa _bankaKasa = _bankaKasaService.GetByIdDetay(bankaKasa.Id);

            if (_bankaKasa == null){return NotFound();}

            _bankaKasa.Tarih = bankaKasa.Tarih;
            _bankaKasa.Aciklama = bankaKasa.Aciklama;
            _bankaKasa.Nitelik = bankaKasa.Nitelik;
            //_bankaKasa.Giren = bankaKasa.Giren;
            //_bankaKasa.Cikan = bankaKasa.Cikan;
            #region VİRGÜL VEYA NOKTA KULLANIMININ İKİSİNİN DE SERBEST OLMASINI SAĞLAMAK İÇİN
            _bankaKasa.Giren = Convert.ToDecimal(bankaKasa.Giren.Replace(".", ","));
            _bankaKasa.Cikan = Convert.ToDecimal(bankaKasa.Cikan.Replace(".", ","));
            #endregion
            _bankaKasa.SonGuncelleme = System.DateTime.Now;
            _bankaKasa.BankaHesapId = bankaKasa.BankaHesapId;

            _bankaKasaService.Update(_bankaKasa);

            TempData.Put("message", new AlertMessage()
            {
                Title = "BAŞARILI",
                AlertType = "success",
                Message = $"{_bankaKasa.Aciklama} GÜNCELLENDİ."
            });

            return RedirectToAction("BankaKasa");
        }

        [HttpGet]
        public IActionResult BankaKasaDetay(int? bankakasaid)
        {
            ViewBag.Sayfa = "DETAY";

            if (bankakasaid == null) { return NotFound(); }

            EBankaKasa bankaKasa = _bankaKasaService.GetByIdDetay((int)bankakasaid);

            if (bankaKasa == null) { return NotFound(); }

            /* View'larda kullanılan modeller, entitylerin WebUI içindeki kopyaları
             * WebUI ve Entity katmanlarındaki modelleri new'lediğim için
             * CRUD işlemlerinde verileri bir modelden diğerine aktarmak zorundayım.
             * Form tarafında data annotation ve validation kullanımında
             * Sürekli entity katmanına gitmek yerine
             * WebUI katmanındaki modeller üzerinde değişiklik yapıyorum.
             * entity katmanında sadece saf database deseni var.
            */

            var _bankaKasa = new BankaKasa()
            {
                Id=bankaKasa.Id,
                Tarih = bankaKasa.Tarih,
                Aciklama = bankaKasa.Aciklama,
                Nitelik = bankaKasa.Nitelik,
                //Giren = bankaKasa.Giren,
                //Cikan = bankaKasa.Cikan,
                #region VİRGÜL VEYA NOKTA KULLANIMININ İKİSİNİN DE SERBEST OLMASINI SAĞLAMAK İÇİN
                Giren = Convert.ToString(bankaKasa.Giren),
                Cikan = Convert.ToString(bankaKasa.Cikan),
                #endregion
                Durum = bankaKasa.Durum,
                CekKaynak = bankaKasa.CekKaynak,
                NakitKaynak = bankaKasa.NakitKaynak,
                SantiyeKasaKaynak = bankaKasa.SantiyeKasaKaynak,
                BankaHesapId = bankaKasa.BankaHesapId,
                BankaHesap = bankaKasa.BankaHesap,
                SistemeGiris = bankaKasa.SistemeGiris,
                SonGuncelleme = bankaKasa.SonGuncelleme
            };

            ViewBag.BankaHesapAdi = _bankaHesapService.GetById(bankaKasa.BankaHesapId).HesapAdi;

            return View(_bankaKasa);
        }

        [HttpGet]
        public IActionResult BankaKasaSil(int? bankakasaid)
        {
            ViewBag.Sayfa = "ANA KASAYA VERİ GÜNCELLEME";

            if (bankakasaid == null) { return NotFound();}

            EBankaKasa bankaKasa = _bankaKasaService.GetByIdDetay((int)bankakasaid);

            if (bankaKasa == null) { return NotFound();}

            bankaKasa.SonGuncelleme = System.DateTime.Now;

            bankaKasa.Durum = false;

            _bankaKasaService.Update(bankaKasa);

            TempData.Put("message", new AlertMessage()
            {
                Title = "BAŞARILI",
                AlertType = "danger",
                Message = $"{bankaKasa.Aciklama} SİLİNDİ."
            });

            return RedirectToAction("BankaKasa");
        }

        //EXCEL//
        public IActionResult BankaKasaExcel(int? bankahesapid)
        {
            ViewBag.Sayfa = "ANA KASA";

            var bankaKasaViewModel = new BankaKasaViewListModel()
            {
                BankaKasas = _bankaKasaService.GetAll((int?)bankahesapid, true),
            };

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("ANA KASA");
                var currentRow = 1;

                #region Header
                worksheet.Cell(currentRow, 1).Value = "TARİH";
                worksheet.Cell(currentRow, 2).Value = "AÇIKLAMA";
                worksheet.Cell(currentRow, 3).Value = "HESAP ADI";
                worksheet.Cell(currentRow, 4).Value = "NİTELİK";
                worksheet.Cell(currentRow, 5).Value = "GİREN";
                worksheet.Cell(currentRow, 6).Value = "ÇIKAN";
                worksheet.Cell(currentRow, 7).Value = "BAKİYE";

                for (int i = 1; i < 8; i++)
                {
                    worksheet.Cell(currentRow, i).Style.Font.Bold = true;
                    worksheet.Cell(currentRow, i).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                }
                #endregion

                #region Body
                foreach (var bankakasa in bankaKasaViewModel.BankaKasas)
                {
                    currentRow++;
                    worksheet.Cell(currentRow, 1).Value = bankakasa.Tarih;
                    worksheet.Cell(currentRow, 2).Value = bankakasa.Aciklama;
                    worksheet.Cell(currentRow, 3).Value = bankakasa.BankaHesap.BankaAdi;
                    worksheet.Cell(currentRow, 4).Value = bankakasa.Nitelik;
                    worksheet.Cell(currentRow, 5).Value = bankakasa.Giren;
                    worksheet.Cell(currentRow, 6).Value = bankakasa.Cikan;

                    for (int i = 1; i < 8; i++)
                    {
                        worksheet.Cell(currentRow, i).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    }
                }
                #endregion

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();

                    return File(
                        content,
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        "AnaKasa.xlsx"
                        );
                }
            }
        }

        //ARŞİV//
        public IActionResult BankaKasaArsiv(int? bankahesapid, int page = 1)
        {
            ViewBag.Sayfa = "ANA KASA - ARŞİV";

            const int pageSize = 10;
            var bankaKasaViewModel = new BankaKasaViewListModel()
            {
                PageInfo = new PageInfo
                {
                    TotalItem = _bankaKasaService.GetCount(bankahesapid, false),
                    CurrentPage = page,
                    ItemPerPage = pageSize,
                    UrlInfo = (int?)bankahesapid
                },

                BankaKasas = _bankaKasaService.GetAll((int?)bankahesapid, false, page, pageSize),
                BankaHesaps = _bankaHesapService.GetAll(false)
            };

            ViewBag.toplamgelir = _bankaKasaService.GetAll((int?)bankahesapid, false).Sum(i => i.Giren);
            ViewBag.toplamgider = _bankaKasaService.GetAll((int?)bankahesapid, false).Sum(i => i.Cikan);

            return View(bankaKasaViewModel);
        }

        //GERİ YÜKLEME//
        [HttpGet]
        public IActionResult BankaKasaGeriYukle(int? bankakasaid)
        {
            ViewBag.Sayfa = "ANA KASAYA VERİ GÜNCELLEME";

            if (bankakasaid == null) { return NotFound();}

            EBankaKasa bankaKasa = _bankaKasaService.GetByIdDetay((int)bankakasaid);

            if (bankaKasa == null) { return NotFound();}

            bankaKasa.SonGuncelleme = System.DateTime.Now;
            bankaKasa.Durum = true;

            if (bankaKasa.SantiyeKasaKaynak != null)
            {
                ESantiyeKasa _santiyeKasa = _santiyeKasaService.GetById((int)bankaKasa.SantiyeKasaKaynak);
                _santiyeKasa.Durum = true;
                _santiyeKasaService.Update(_santiyeKasa);
            };

            _bankaKasaService.Update(bankaKasa);

            TempData.Put("message", new AlertMessage()
            {
                Title = "BAŞARILI",
                AlertType = "success",
                Message = $"{bankaKasa.Aciklama} GERİ YÜKLENDİ."
            });

            return RedirectToAction("BankaKasa");
        }

        #region BANKALAR ARASI EFT
        [HttpGet]
        public IActionResult BankaKasaEklemeEft()
        {
            ViewBag.Sayfa = "HESAPLAR ARASI PARA TRANSFERİ";
            ViewBag.bankahesap = _bankaHesapService.GetAll(true);

            return View();
        }

        [HttpPost]
        public IActionResult BankaKasaEklemeEFT(BankaKasaEftModel b)
        {
            ViewBag.Sayfa = "HESAPLAR ARASI PARA TRANSFERİ";
            ViewBag.bankahesap = _bankaHesapService.GetAll(true);

            if (!ModelState.IsValid) { return View(b); }

            EBankaKasa EFTGonderenHesap = new EBankaKasa();
            EBankaKasa EFTAlanHesap = new EBankaKasa();

            //OTOMATİK AÇIKLAMA YAZDIRMAK İÇİN BANKA İSİMLERİNİ ÇEKİCEM
            var GonderenBankaHesabi = _bankaHesapService.GetById((int)b.GonderenBanka);
            var AliciBankaHesabi = _bankaHesapService.GetById((int)b.AliciBanka);

            var GonderenHesapAdi = GonderenBankaHesabi.BankaAdi;
            var AliciHesapAdi = AliciBankaHesabi.BankaAdi;

            EFTGonderenHesap.Tarih = b.Tarih;
            EFTGonderenHesap.Aciklama = AliciHesapAdi + " HESABINA GÖNDERİLEN EFT" + " - " + b.Aciklama;
            EFTGonderenHesap.Nitelik = "EFT";
            EFTGonderenHesap.Giren = 0;
            EFTGonderenHesap.Cikan = Convert.ToDecimal(b.Tutar.Replace(".",","));
            EFTGonderenHesap.BankaHesapId = b.GonderenBanka;

            EFTAlanHesap.Tarih = b.Tarih;
            EFTAlanHesap.Aciklama = GonderenHesapAdi + " HESABINDAN GELEN EFT" + " - " + b.Aciklama;
            EFTAlanHesap.Nitelik = "EFT";
            EFTAlanHesap.Giren = Convert.ToDecimal(b.Tutar.Replace(".", ","));
            EFTAlanHesap.Cikan = 0;
            EFTAlanHesap.BankaHesapId = b.AliciBanka;

            _bankaKasaService.Create(EFTGonderenHesap);
            _bankaKasaService.Create(EFTAlanHesap);

            TempData.Put("message", new AlertMessage()
            {
                Title = "BAŞARILI",
                AlertType = "success",
                Message = $"{b.Aciklama} EKLENDİ."
            });

            return RedirectToAction("BankaKasa");
        }
        #endregion

        #region ŞANTİYE KASA EFT İŞLEMLERİ
        [HttpGet]
        public IActionResult BankaKasaEklemeSantiyeEft()
        {
            ViewBag.Sayfa = "ŞANTİYE KASASINA EFT";
            ViewBag.Santiye = _santiyeService.GetAll(true);
            ViewBag.BankaHesap = _bankaHesapService.GetAll(true);

            return View();
        }

        [HttpPost]
        public IActionResult BankaKasaEklemeSantiyeEft(BankaKasaEftSantiyeModel b)
        {
            ViewBag.Sayfa = "ŞANTİYE KASASINA EFT";
            ViewBag.Santiye = _santiyeService.GetAll(true);
            ViewBag.BankaHesap = _bankaHesapService.GetAll(true);

            if (!ModelState.IsValid) { return View(b); }

            //ARA MODELE DAYANARAK BANKA KASA MODELİ TANIMLANDI VE EKLENDİ
            string bankahesapadi = _bankaHesapService.GetById((int)b.BankaHesapId).BankaAdi;
            string santiyeadi = _santiyeService.GetById((int)b.SantiyeId).Ad;

            EBankaKasa entityBankaKasa = new EBankaKasa()
            {
                Tarih = b.Tarih,
                Aciklama = santiyeadi + " ŞANTİYESİNE GÖNDERİLEN EFT" + " - " + b.Aciklama,
                Nitelik = "ŞANTİYE KASASINA EFT",
                Cikan = Convert.ToDecimal(b.Tutar.Replace(".", ",")),
                BankaHesapId = b.BankaHesapId
            };
            _bankaKasaService.Create(entityBankaKasa);

            //ARA MODELE DAYANARAK ŞANTİYE KASA MODELİ TANIMLANDI VE EKLENDİ
            //EKLENMİŞ BANKA KASA MODELİNDEN KAYNAK ID ÇEKİLEREK ŞANTİYE KASASINA YAZILDI
            ESantiyeKasa entitySantiyeKasa = new ESantiyeKasa()
            {
                Tarih = b.Tarih,
                Aciklama = bankahesapadi + " HESABINDAN KASAYA GELEN EFT" + " - " + b.Aciklama,
                Kisi = "OFİS",
                No = "YOK",
                Gelir = Convert.ToDecimal(b.Tutar.Replace(".", ",")),
                SantiyeId = b.SantiyeId,
                SantiyeGiderKalemiId = 1,
                BankaKasaKaynak = entityBankaKasa.Id
            };
            _santiyeKasaService.Create(entitySantiyeKasa);

            //EKLENMİŞ ŞANTİYE KASA MODELİNDEN KAYNAK ID ÇEKİLEREK BANKA KASASI GÜNCELLENDİ YAZILDI
            var entityBankaKasaKaynakGuncelleme = _bankaKasaService.GetById(entityBankaKasa.Id);

            entityBankaKasaKaynakGuncelleme.SantiyeKasaKaynak = entitySantiyeKasa.Id;

            _bankaKasaService.Update(entityBankaKasaKaynakGuncelleme);

            TempData.Put("message", new AlertMessage()
            {
                Title = "BAŞARILI",
                AlertType = "success",
                Message = $"{b.Aciklama} EKLENDİ."
            });

            return RedirectToAction("BankaKasa");
        }

        [HttpGet]
        public IActionResult BankaKasaGuncelleSantiyeEft(int? bankakasasantiyeid)
        {
            ViewBag.Sayfa = "ŞANTİYE KASASINA EFT GUNCELLEME";
            ViewBag.Santiye = _santiyeService.GetAll(true);
            ViewBag.BankaHesap = _bankaHesapService.GetAll(true);

            #region BANKA VE SANTİYE KASA NESNELERİ
            //Gelen ID üzerinden BANKAKASA nesnesini çekiyorum
            if (bankakasasantiyeid == null) { return NotFound(); }
            EBankaKasa bankaKasa = _bankaKasaService.GetByIdDetay((int)bankakasasantiyeid);
            if (bankaKasa == null) { return NotFound(); }
            //BankaKasa nesnesi üzerinden santiyekasakaynak id ile ŞANTİYEKASA nesnesini buluyorum
            if (bankakasasantiyeid == null) { return NotFound(); }
            ESantiyeKasa santiyeKasa = _santiyeKasaService.GetByIdDetay((int)bankaKasa.SantiyeKasaKaynak);
            if (bankaKasa == null) { return NotFound(); }
            #endregion


            BankaKasaEftSantiyeModel bankaKasaEftSantiyeModel = new BankaKasaEftSantiyeModel()
            {
                Tarih = bankaKasa.Tarih,
                Aciklama = bankaKasa.Aciklama,
                Tutar = Convert.ToString(bankaKasa.Cikan),
                BankaHesapId = bankaKasa.BankaHesapId,
                SantiyeId = santiyeKasa.SantiyeId,

                BankaKasaId = bankaKasa.Id,
                SantiyeKasaId = bankaKasa.SantiyeKasaKaynak
            };

            return View(bankaKasaEftSantiyeModel);
        }

        [HttpPost]
        public IActionResult BankaKasaGuncelleSantiyeEft(BankaKasaEftSantiyeModel b)
        {
            ViewBag.Sayfa = "ŞANTİYE KASASINA EFT GUNCELLEME";
            ViewBag.Santiye = _santiyeService.GetAll(true);
            ViewBag.BankaHesap = _bankaHesapService.GetAll(true);

            if (!ModelState.IsValid) { return View(b); }

            #region BANKA HESAP ADI VE ŞANTİYE HESAP ADINI BULUYORUZ
            //ARA MODELE DAYANARAK BANKA KASA MODELİ TANIMLANDI VE EKLENDİ
            string bankahesapadi = _bankaHesapService.GetById((int)b.BankaHesapId).BankaAdi;
            //string santiyeadi = _santiyeService.GetById((int)b.SantiyeId).Ad;
            #endregion

            #region BANKAKASA NESNESİ
            //BULUNDU
            EBankaKasa entityBankaKasa = _bankaKasaService.GetByIdDetay((int)b.BankaKasaId);
            //GÜNCELLENDİ
            entityBankaKasa.Tarih = b.Tarih;
            entityBankaKasa.Aciklama = b.Aciklama;
            entityBankaKasa.Nitelik = "ŞANTİYE KASASINA EFT";
            entityBankaKasa.Cikan = Convert.ToDecimal(b.Tutar.Replace(".", ","));
            entityBankaKasa.BankaHesapId = b.BankaHesapId;
            //GÜNCELLEME SONRASI KAYIT EDİLDİ
            _bankaKasaService.Update(entityBankaKasa);
            #endregion

            #region SANTİYEKASA NESNESİ
            //BULUNDU
            ESantiyeKasa entitySantiyeKasa = _santiyeKasaService.GetByIdDetay((int)b.SantiyeKasaId);
            //GÜNCELLENDİ
            entitySantiyeKasa.Tarih = b.Tarih;
            entitySantiyeKasa.Aciklama = bankahesapadi + " HESABINDAN KASAYA GELEN EFT" + " - " + b.Aciklama;
            entitySantiyeKasa.Kisi = "OFİS";
            entitySantiyeKasa.No = "YOK";
            entitySantiyeKasa.Gelir = Convert.ToDecimal(b.Tutar.Replace(".", ","));
            entitySantiyeKasa.SantiyeId = b.SantiyeId;
            entitySantiyeKasa.SantiyeGiderKalemiId = 1;
            entitySantiyeKasa.BankaKasaKaynak = entityBankaKasa.Id;
            //GÜNCELLEME SONRASI KAYIT EDİLDİ
            _santiyeKasaService.Update(entitySantiyeKasa);
            #endregion

            TempData.Put("message", new AlertMessage()
            {
                Title = "BAŞARILI",
                AlertType = "success",
                Message = $"{b.Aciklama} GÜNCELLEDİ."
            });

            return RedirectToAction("BankaKasa");
        }

        [HttpGet]
        public IActionResult BankaKasaSilSantiyeEft(int? bankakasasantiyeid)
        {
            ViewBag.Sayfa = "ŞANTİYE KASASINA EFT GUNCELLEME";

            #region BANKA VE SANTİYE KASA NESNELERİ
            //Gelen ID üzerinden BANKAKASA nesnesini çekiyorum
            if (bankakasasantiyeid == null) { return NotFound(); }
            EBankaKasa bankaKasa = _bankaKasaService.GetByIdDetay((int)bankakasasantiyeid);
            if (bankaKasa == null) { return NotFound(); }
            //BankaKasa nesnesi üzerinden santiyekasakaynak id ile ŞANTİYEKASA nesnesini buluyorum
            if (bankakasasantiyeid == null) { return NotFound(); }
            ESantiyeKasa santiyeKasa = _santiyeKasaService.GetByIdDetay((int)bankaKasa.SantiyeKasaKaynak);
            if (bankaKasa == null) { return NotFound(); }
            #endregion

            ViewBag.Santiye = _santiyeService.GetById((int)bankaKasa.SantiyeKasaKaynak).Ad;
            ViewBag.BankaHesap = _bankaHesapService.GetById((int)bankaKasa.BankaHesapId).BankaAdi;

            BankaKasaEftSantiyeModel bankaKasaEftSantiyeModel = new BankaKasaEftSantiyeModel()
            {
                Tarih = bankaKasa.Tarih,
                Aciklama = bankaKasa.Aciklama,
                Tutar = Convert.ToString(bankaKasa.Cikan),

                BankaHesapId = bankaKasa.BankaHesapId,
                SantiyeId = santiyeKasa.SantiyeId,

                BankaKasaId = bankakasasantiyeid,
                SantiyeKasaId = bankaKasa.SantiyeKasaKaynak
            };

            return View(bankaKasaEftSantiyeModel);
        }

        [HttpPost]
        public IActionResult BankaKasaSilSantiyeEft(BankaKasaEftSantiyeModel b)
        {

            #region BANKAKASA NESNESİ
            //BULUNDU
            EBankaKasa entityBankaKasa = _bankaKasaService.GetByIdDetay((int)b.BankaKasaId);
            //GÜNCELLENDİ
            entityBankaKasa.SonGuncelleme = System.DateTime.Now;
            entityBankaKasa.Durum = false;
            //GÜNCELLEME SONRASI KAYIT EDİLDİ
            _bankaKasaService.Update(entityBankaKasa);
            #endregion

            #region SANTİYEKASA NESNESİ
            //BULUNDU
            ESantiyeKasa entitySantiyeKasa = _santiyeKasaService.GetByIdDetay((int)b.SantiyeKasaId);
            //GÜNCELLENDİ
            entitySantiyeKasa.SonGuncelleme = System.DateTime.Now;
            entitySantiyeKasa.Durum = false;
            //GÜNCELLEME SONRASI KAYIT EDİLDİ
            _santiyeKasaService.Update(entitySantiyeKasa);
            #endregion

            TempData.Put("message", new AlertMessage()
            {
                Title = "BAŞARILI",
                AlertType = "danger",
                Message = $"{b.Aciklama} SİLİNDİ."
            });

            return RedirectToAction("BankaKasa");
        }
        #endregion
    }
}
