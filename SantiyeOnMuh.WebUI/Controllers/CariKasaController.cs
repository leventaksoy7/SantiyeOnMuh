﻿using ClosedXML.Excel;
using DocumentFormat.OpenXml.Drawing;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SantiyeOnMuh.Business.Abstract;
using SantiyeOnMuh.Entity;
using SantiyeOnMuh.WebUI.Extensions;
using SantiyeOnMuh.WebUI.Identity;
using SantiyeOnMuh.WebUI.Models;
using SantiyeOnMuh.WebUI.Models.Modeller;
using System.ComponentModel.DataAnnotations;

namespace SantiyeOnMuh.WebUI.Controllers
{
    //[ValidateAntiForgeryToken]
    //[Authorize(Roles = "Admin,Ofis,Santiye")]
    public class CariKasaController : Controller
    {
        // NESNELER ÜZERİNDEKİ İŞLEMLERİ _ OLAN NESNE ÜZERİNDE YAPIP SONRA AKTARIYORUZ - INJECTION
        private ICariKasaService _cariKasaService;
        private ICariHesapService _cariHesapService;
        private ICariGiderKalemiService _cariGiderKalemiService;
        private ISantiyeService _cariService;
        private UserManager<User> _userManager;
        public CariKasaController(
            UserManager<User> userManager,
            ICariKasaService cariKasaService,
            ICariHesapService cariHesapService,
            ICariGiderKalemiService cariGiderKalemiService,
            ISantiyeService cariService)
        {
            this._userManager = userManager;
            this._cariKasaService = cariKasaService;
            this._cariHesapService = cariHesapService;
            this._cariGiderKalemiService = cariGiderKalemiService;
            this._cariService = cariService;
        }

        [Authorize(Roles = "Admin,Ofis,Santiye")]
        public async Task<IActionResult> CariKasa(int carihesapid, int? gkid, int page = 1)
        {
            const int pageSize = 10;

            var cariKasaViewModel = new CariKasaViewListModel()
            {
                PageInfo = new PageInfo
                {
                    TotalItem = _cariKasaService.GetCount((int)carihesapid, (int?)gkid, true),
                    CurrentPage = page,
                    ItemPerPage = pageSize,
                    UrlInfo = (int?)carihesapid
                },

                CariKasas = _cariKasaService.GetAll((int)carihesapid, (int?)gkid, true, page, pageSize),
                CariGiderKalemis = _cariGiderKalemiService.GetAll(true, true),
                CariHesap = _cariHesapService.GetById((int)carihesapid)
            };

            #region
            var user = await _userManager.GetUserAsync(HttpContext.User);

            if (await _userManager.IsInRoleAsync(user, "Santiye"))
            {
                var santiyeid = cariKasaViewModel.CariHesap.SantiyeId;
                if (santiyeid != user.SantiyeId)
                {
                    return RedirectToAction("LogOut", "Account");
                }
            }
            #endregion

            //gider kalemi olup olmadığını kontrol ediyoruz, ona göre alt taraftaki toplamın şekli değişecek
            //BAŞLIK

            ViewBag.Sayfa = cariKasaViewModel.CariHesap.Ad + " CARİ HESABI";
            ViewBag.gk = gkid;

            ViewBag.toplamgider = _cariKasaService.GetAll((int)carihesapid, (int?)gkid, true).Sum(i => i.Borc);
            ViewBag.toplamgelir = _cariKasaService.GetAll((int)carihesapid, (int?)gkid, true).Sum(i => i.Alacak);

            return View(cariKasaViewModel);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult CariKasaArsiv(int carihesapid, int? gkid, int page = 1)
        {
            const int pageSize = 10;

            var cariKasaViewModel = new CariKasaViewListModel()
            {
                PageInfo = new PageInfo
                {
                    TotalItem = _cariKasaService.GetCount((int)carihesapid, (int?)gkid, false),
                    CurrentPage = page,
                    ItemPerPage = pageSize,
                    UrlInfo = (int?)carihesapid
                },

                CariKasas = _cariKasaService.GetAll((int)carihesapid, (int?)gkid, false, page, pageSize),
                CariGiderKalemis = _cariGiderKalemiService.GetAll(true, true),
                CariHesap = _cariHesapService.GetById((int)carihesapid)
            };

            //gider kalemi olup olmadığını kontrol ediyoruz, ona göre alt taraftaki toplamın şekli değişecek
            //BAŞLIK

            ViewBag.Sayfa = cariKasaViewModel.CariHesap.Ad + " CARİ HESABI";

            ViewBag.gk = gkid;

            ViewBag.toplamgider = _cariKasaService.GetAll((int)carihesapid, (int?)gkid, false).Sum(i => i.Borc);
            ViewBag.toplamgelir = _cariKasaService.GetAll((int)carihesapid, (int?)gkid, false).Sum(i => i.Alacak);

            return View(cariKasaViewModel);
        }

        //[HttpGet]
        //public async Task<IActionResult> CariKasaEkleme(int? carihesapid)
        //{
        //    ViewBag.Sayfa = "CARİ HESABA FATURA GİRİŞİ";
        //    ViewBag.CariHesap = _cariHesapService.GetAll(null, true);
        //    ViewBag.CariGK = _cariGiderKalemiService.GetAll(true, true);
        //    ViewBag.carihesapid = carihesapid;
        //    #region
        //    var user = await _userManager.GetUserAsync(HttpContext.User);

        //    if (await _userManager.IsInRoleAsync(user, "Santiye") && carihesapid!=null)
        //    {
        //        var cariHesap = _cariHesapService.GetById((int)carihesapid);
        //        var santiyeid = cariHesap.SantiyeId;

        //        if (santiyeid != user.SantiyeId)
        //        {
        //            return RedirectToAction("LogOut", "Account");
        //        }
        //    }
        //    #endregion

        //    return View(new CariKasa());
        //}

        //[HttpPost]
        //public async Task<IActionResult> CariKasaEkleme(CariKasa cariKasa, IFormFile? file)
        //{
        //    #region
        //    var user = await _userManager.GetUserAsync(HttpContext.User);

        //    if (await _userManager.IsInRoleAsync(user, "Santiye"))
        //    {
        //        var cariHesap = _cariHesapService.GetById((int)cariKasa.CariHesapId);
        //        var santiyeid = cariHesap.SantiyeId;

        //        if (santiyeid != user.SantiyeId)
        //        {
        //            return RedirectToAction("LogOut", "Account");
        //        }
        //    }
        //    #endregion


        //    if (!ModelState.IsValid) { return View(cariKasa); }

        //    #region  RESİM VS. EKLENMEMİŞSE SAYFAYA GERİ GİDİYOR, GERİ GİDİLEN SAYFANIN İHTİYACI OLAN BİLGİLER
        //    ViewBag.Sayfa = "CARİ HESABA FATURA GİRİŞİ";
        //    ViewBag.CariHesap = _cariHesapService.GetAll(null, true);
        //    ViewBag.CariGK = _cariGiderKalemiService.GetAll(true, true);
        //    //ÜSTTEKİ SIFIRDAN EKLEMENİN BİREBİR AYNISI, GEREKLİ BİLGİLER
        //    #endregion
        //    #region RESİM EKLEME BÖLÜMÜ
        //    if (file != null)
        //    {
        //        var extension = System.IO.Path.GetExtension(file.FileName);

        //        if (extension == ".jpg" || extension == ".png" || extension == ".pdf")
        //        {
        //            var cariKasaName = string.Format($"{cariKasa.Aciklama}{"-"}{Guid.NewGuid()}{extension}");
        //            cariKasa.ImgUrl = cariKasaName;
        //            var path = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\CariKasaResim", cariKasaName);

        //            using (var stream = new FileStream(path, FileMode.Create))
        //            {
        //                await file.CopyToAsync(stream);
        //            }
        //        }
        //        else { return View(cariKasa); }
        //    }
        //    else { return View(cariKasa); }
        //    #endregion

        //    ECariKasa _cariKasa = new ECariKasa()
        //    {
        //        Tarih = cariKasa.Tarih,
        //        Aciklama = cariKasa.Aciklama,
        //        #region VİRGÜL VEYA NOKTA KULLANIMININ İKİSİNİN DE SERBEST OLMASINI SAĞLAMAK İÇİN
        //        Miktar = Convert.ToDecimal(cariKasa.Miktar.Replace(".", ",")),
        //        BirimFiyat = Convert.ToDecimal(cariKasa.BirimFiyat.Replace(".", ",")),
        //        Borc = Convert.ToDecimal(cariKasa.Borc.Replace(".", ",")),
        //        Alacak = Convert.ToDecimal(cariKasa.Alacak.Replace(".", ",")),
        //        #endregion
        //        ImgUrl = cariKasa.ImgUrl,
        //        CekKaynak = cariKasa.CekKaynak,
        //        NakitKaynak = cariKasa.NakitKaynak,
        //        SistemeGiris = cariKasa.SistemeGiris,
        //        SonGuncelleme = cariKasa.SonGuncelleme,
        //        Durum = cariKasa.Durum,
        //        CariGiderKalemiId = cariKasa.CariGiderKalemiId,
        //        CariGiderKalemi = cariKasa.CariGiderKalemi,
        //        CariHesapId = cariKasa.CariHesapId,
        //        CariHesap = cariKasa.CariHesap,
        //    };

        //    if (_cariKasaService.Create(_cariKasa))
        //    {
        //        TempData.Put("message", new AlertMessage()
        //        {
        //            Title = "BAŞARILI",
        //            AlertType = "success",
        //            Message = $"{_cariKasa.Aciklama} KASAYA EKLENDİ."
        //        });

        //        return RedirectToAction("CariKasa", "CariKasa", new { carihesapid = cariKasa.CariHesap.Id });
        //    };

        //    TempData.Put("message", new AlertMessage()
        //    {
        //        Title = "HATA",
        //        AlertType = "danger",
        //        Message = _cariKasaService.ErrorMessage
        //    });

        //    return View(_cariKasa);
        //}

        [Authorize(Roles = "Admin,Ofis,Santiye")]
        [HttpGet]
        public IActionResult CariKasaDetay(int? carikasaid)
        {
            ViewBag.Sayfa = "FATURA DETAYI";
            if (carikasaid == null){ return NotFound(); }

            ECariKasa cariKasa = _cariKasaService.GetById((int)carikasaid);

            if (cariKasa == null){ return NotFound();}

            CariKasa _cariKasa = new CariKasa()
            {
                Id = cariKasa.Id,
                Tarih = cariKasa.Tarih,
                Aciklama = cariKasa.Aciklama,
                //Miktar = cariKasa.Miktar,
                //BirimFiyat = cariKasa.BirimFiyat,
                //Borc = cariKasa.Borc,
                //Alacak = cariKasa.Alacak,
                #region VİRGÜL VEYA NOKTA KULLANIMININ İKİSİNİN DE SERBEST OLMASINI SAĞLAMAK İÇİN
                Miktar = Convert.ToString(cariKasa.Miktar),
                BirimFiyat = Convert.ToString(cariKasa.BirimFiyat),
                Borc = Convert.ToString(cariKasa.Borc),
                Alacak = Convert.ToString(cariKasa.Alacak),
                #endregion
                ImgUrl = cariKasa.ImgUrl,
                CekKaynak = cariKasa.CekKaynak,
                NakitKaynak = cariKasa.NakitKaynak,
                SistemeGiris = cariKasa.SistemeGiris,
                SonGuncelleme = cariKasa.SonGuncelleme,
                Durum = cariKasa.Durum,
                CariGiderKalemiId = cariKasa.CariGiderKalemiId,
                CariGiderKalemi = cariKasa.CariGiderKalemi,
                CariHesapId = cariKasa.CariHesapId,
                CariHesap = cariKasa.CariHesap,
            };

            return View(_cariKasa);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult CariKasaSil(int? carikasaid)
        {
            ViewBag.Sayfa = "FATURAYI SİL";

            if (carikasaid == null){return NotFound();}

            ECariKasa cariKasa = _cariKasaService.GetByIdDetay((int)carikasaid);

            if (cariKasa == null){return NotFound();}

            CariKasa _cariKasa = new CariKasa()
            {
                Id = cariKasa.Id,
                Tarih = cariKasa.Tarih,
                Aciklama = cariKasa.Aciklama,
                //Miktar = cariKasa.Miktar,
                //BirimFiyat = cariKasa.BirimFiyat,
                //Borc = cariKasa.Borc,
                //Alacak = cariKasa.Alacak,
                #region VİRGÜL VEYA NOKTA KULLANIMININ İKİSİNİN DE SERBEST OLMASINI SAĞLAMAK İÇİN
                Miktar = Convert.ToString(cariKasa.Miktar),
                BirimFiyat = Convert.ToString(cariKasa.BirimFiyat),
                Borc = Convert.ToString(cariKasa.Borc),
                Alacak = Convert.ToString(cariKasa.Alacak),
                #endregion
                ImgUrl = cariKasa.ImgUrl,
                CekKaynak = cariKasa.CekKaynak,
                NakitKaynak = cariKasa.NakitKaynak,
                SistemeGiris = cariKasa.SistemeGiris,
                SonGuncelleme = cariKasa.SonGuncelleme,
                Durum = cariKasa.Durum,
                CariGiderKalemiId = cariKasa.CariGiderKalemiId,
                CariGiderKalemi = cariKasa.CariGiderKalemi,
                CariHesapId = cariKasa.CariHesapId,
                CariHesap = cariKasa.CariHesap,
            };

            return View(_cariKasa);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult CariKasaSil(CariKasa cariKasa)
        {
            ECariKasa _cariKasa = _cariKasaService.GetByIdDetay(cariKasa.Id);

            if (_cariKasa == null){return NotFound();}

            _cariKasa.SonGuncelleme = System.DateTime.Now;
            _cariKasa.Durum = false;

            _cariKasaService.Update(_cariKasa);

            TempData.Put("message", new AlertMessage()
            {
                Title = "BAŞARILI",
                AlertType = "danger",
                Message = $"{_cariKasa.Aciklama} SİLİNDİ."

            });

            return RedirectToAction("CariKasa", "CariKasa", new { carihesapid = _cariKasa.CariHesapId });
        }

        [Authorize(Roles = "Admin,Ofis,Santiye")]
        //EXCEL
        public async Task<IActionResult> CariKasaExcel(int carihesapid, int? gkid)
        {
            #region
            var user = await _userManager.GetUserAsync(HttpContext.User);

            if (await _userManager.IsInRoleAsync(user, "Santiye"))
            {
                var cariHesap = _cariHesapService.GetById(carihesapid);
                var santiyeid = cariHesap.SantiyeId;

                if (santiyeid != user.SantiyeId)
                {
                    return RedirectToAction("LogOut", "Account");
                }
            }
            #endregion

            var cariKasaViewModel = new CariKasaViewListModel()
            {
                CariKasas = _cariKasaService.GetAll((int)carihesapid, (int?)gkid, true),
                CariGiderKalemis = _cariGiderKalemiService.GetAll(true, true),
                CariHesap = _cariHesapService.GetById((int)carihesapid)
            };

            decimal toplamgider = cariKasaViewModel.CariKasas.Sum(i => i.Borc);
            decimal toplamgelir = cariKasaViewModel.CariKasas.Sum(i => i.Alacak);


            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("CARİ");
                var currentRow = 7;
                decimal bakiye = 0;

                #region Hesap Bilgileri
                var carihesap = cariKasaViewModel.CariHesap;

                worksheet.Cell(2, 1).Value = "FİRMA ADI:";
                worksheet.Cell(2, 2).Value = carihesap.Ad;

                worksheet.Cell(3, 1).Value = "ADRESİ";
                worksheet.Cell(3, 2).Value = carihesap.Adres;

                worksheet.Cell(4, 1).Value = "TELEFON:";
                worksheet.Cell(4, 2).Value = carihesap.Telefon;

                worksheet.Cell(5, 1).Value = "VERGİ NO:";
                worksheet.Cell(5, 2).Value = carihesap.VergiNo;

                worksheet.Cell(2, 4).Value = "İLGİLİ KİŞİ:";
                worksheet.Cell(2, 5).Value = carihesap.IlgiliKisi;

                worksheet.Cell(3, 4).Value = "TELEFON";
                worksheet.Cell(3, 5).Value = carihesap.IlgiliKisiTelefon;

                worksheet.Cell(4, 4).Value = "ÖDEME:";
                worksheet.Cell(4, 5).Value = carihesap.Odeme;

                worksheet.Cell(5, 4).Value = "VADE:";
                worksheet.Cell(5, 5).Value = carihesap.Vade;

                for (int i = 2; i < 6; i++)
                {
                    worksheet.Cell(i, 1).Style.Font.Bold = true;
                    worksheet.Cell(i, 1).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    worksheet.Cell(i, 2).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

                    worksheet.Cell(i, 4).Style.Font.Bold = true;
                    worksheet.Cell(i, 4).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    worksheet.Cell(i, 5).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                }
                #endregion

                #region Header
                worksheet.Cell(currentRow, 1).Value = "TARİH";
                worksheet.Cell(currentRow, 2).Value = "AÇIKLAMA";
                worksheet.Cell(currentRow, 3).Value = "MİKTAR";
                worksheet.Cell(currentRow, 4).Value = "BİRİM FİYAT";
                worksheet.Cell(currentRow, 5).Value = "BORÇ";
                worksheet.Cell(currentRow, 6).Value = "ALACAK";
                worksheet.Cell(currentRow, 7).Value = "BAKİYE";

                for (int i = 1; i < 8; i++)
                {
                    worksheet.Cell(currentRow, i).Style.Font.Bold = true;
                    worksheet.Cell(currentRow, i).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                }
                #endregion

                #region Body
                foreach (var carikasa in cariKasaViewModel.CariKasas)
                {
                    currentRow++;
                    bakiye = bakiye - carikasa.Borc + carikasa.Alacak;
                    worksheet.Cell(currentRow, 1).Value = carikasa.Tarih;
                    worksheet.Cell(currentRow, 2).Value = carikasa.Aciklama;
                    worksheet.Cell(currentRow, 3).Value = carikasa.Miktar;
                    worksheet.Cell(currentRow, 4).Value = carikasa.BirimFiyat;
                    worksheet.Cell(currentRow, 5).Value = carikasa.Borc;
                    worksheet.Cell(currentRow, 6).Value = carikasa.Alacak;
                    worksheet.Cell(currentRow, 7).Value = bakiye;

                    for (int i = 1; i < 8; i++)
                    {
                        worksheet.Cell(currentRow, i).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    }
                }
                #endregion

                #region SON SATIR

                for (int i = 1; i < 8; i++)
                {
                    worksheet.Cell(currentRow + 1, i).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    worksheet.Cell(currentRow + 1, i).Style.Font.Bold = true;
                }

                worksheet.Cell(currentRow + 1, 2).Value = "TOPLAM";
                worksheet.Cell(currentRow + 1, 5).Value = toplamgider;
                worksheet.Cell(currentRow + 1, 6).Value = toplamgelir;
                worksheet.Cell(currentRow + 1, 7).Value = toplamgelir - toplamgider;
                #endregion

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();

                    return File(
                        content,
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        "Cari.xlsx"
                        );
                }
            }
        }

        //GERİ YÜKLE
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult CariKasaGeriYukle(int? carikasaid)
        {
            if (carikasaid == null){return NotFound();}

            ECariKasa cariKasa = _cariKasaService.GetByIdDetay((int)carikasaid);

            if (cariKasa == null){return NotFound();}

            cariKasa.SonGuncelleme = System.DateTime.Now;
            cariKasa.Durum = true;

            _cariKasaService.Update(cariKasa);

            return RedirectToAction("CariKasa", "CariKasa", new { carihesapid = cariKasa.CariHesapId });
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult CariKasaGeriYukle(CariKasa cariKasa)
        {
            if (!ModelState.IsValid) { return View(cariKasa); }

            ECariKasa _cariKasa = _cariKasaService.GetByIdDetay(cariKasa.Id);

            if (_cariKasa == null){return NotFound();}

            _cariKasa.SonGuncelleme = System.DateTime.Now;
            _cariKasa.Durum = true;

            _cariKasaService.Update(_cariKasa);

            TempData.Put("message", new AlertMessage()
            {
                Title = "BAŞARILI",
                AlertType = "danger",
                Message = $"{_cariKasa.Aciklama} KASAYA GERİ EKLENDİ."

            });

            return RedirectToAction("CariKasa", "CariKasa", new { carihesapid = _cariKasa.CariHesapId });
        }

        #region CARİDEN
        [HttpGet]
        [Authorize(Roles = "Admin,Ofis,Santiye")]
        public async Task<IActionResult> CariKasaFaturaEklemeFromCari(int carihesapid)
        {
            ViewBag.Sayfa = _cariHesapService.GetById(carihesapid).Ad + " FİRMA CARİSİNE FATURA GİRİŞİ";
            ViewBag.CariGK = _cariGiderKalemiService.GetAll(true, true);
            ViewBag.CariHesapId = carihesapid;

            #region
            var user = await _userManager.GetUserAsync(HttpContext.User);

            if (await _userManager.IsInRoleAsync(user, "Santiye"))
            {
                var cariHesap = _cariHesapService.GetById(carihesapid);
                var santiyeid = cariHesap.SantiyeId;

                if (santiyeid != user.SantiyeId)
                {
                    return RedirectToAction("LogOut", "Account");
                }
            }
            #endregion

            return View(new CariKasa());
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Ofis,Santiye")]
        public async Task<IActionResult> CariKasaFaturaEklemeFromCari(CariKasa cariKasa, IFormFile? file)
        {
            #region SAYFAYA GERİ GİDERSE, GİDİLEN SAYFANIN İHTİYACI OLAN BİLGİLER
            ViewBag.Sayfa = _cariHesapService.GetById(cariKasa.CariHesapId).Ad + " FİRMA CARİSİNE FATURA GİRİŞİ";
            ViewBag.CariGK = _cariGiderKalemiService.GetAll(true, true);

            ViewBag.CariHesapId = cariKasa.CariHesapId;
            //ÜSTTEKİ SIFIRDAN EKLEMENİN BİREBİR AYNISI, GEREKLİ BİLGİLER
            #endregion

            #region
            var user = await _userManager.GetUserAsync(HttpContext.User);

            if (await _userManager.IsInRoleAsync(user, "Santiye"))
            {
                var cariHesap = _cariHesapService.GetById((int)cariKasa.CariHesapId);
                var santiyeid = cariHesap.SantiyeId;

                if (santiyeid != user.SantiyeId)
                {
                    return RedirectToAction("LogOut", "Account");
                }
            }
            #endregion

            if (!ModelState.IsValid) { return View(cariKasa); }

            #region RESİM EKLEME BÖLÜMÜ
            if (file != null)
            {
                var extension = System.IO.Path.GetExtension(file.FileName);

                if (extension == ".jpg" || extension == ".png" || extension == ".pdf")
                {
                    var cariKasaName = string.Format($"{cariKasa.Aciklama}{"-"}{Guid.NewGuid()}{extension}");
                    cariKasa.ImgUrl = cariKasaName;
                    var path = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\CariKasaResim", cariKasaName);

                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }
                }
                else { return View(cariKasa); }
            }
            //else { return View(cariKasa); }
            #endregion

            ECariKasa _cariKasa = new ECariKasa()
            {
                Id = cariKasa.Id,
                Tarih = cariKasa.Tarih,
                Aciklama = cariKasa.Aciklama,
                #region VİRGÜL VEYA NOKTA KULLANIMININ İKİSİNİN DE SERBEST OLMASINI SAĞLAMAK İÇİN
                Miktar = Convert.ToDecimal(cariKasa.Miktar.Replace(".", ",")),
                BirimFiyat = Convert.ToDecimal(cariKasa.BirimFiyat.Replace(".", ",")),
                Borc = Convert.ToDecimal(cariKasa.Borc.Replace(".", ",")),
                Alacak = Convert.ToDecimal(cariKasa.Alacak.Replace(".", ",")),
                #endregion
                ImgUrl = cariKasa.ImgUrl,
                CekKaynak = cariKasa.CekKaynak,
                NakitKaynak = cariKasa.NakitKaynak,
                SistemeGiris = cariKasa.SistemeGiris,
                SonGuncelleme = cariKasa.SonGuncelleme,
                Durum = cariKasa.Durum,
                CariGiderKalemiId = cariKasa.CariGiderKalemiId,
                CariGiderKalemi = cariKasa.CariGiderKalemi,
                CariHesapId = cariKasa.CariHesapId,
                CariHesap = cariKasa.CariHesap,
            };

            if (_cariKasaService.Create(_cariKasa))
            {
                TempData.Put("message", new AlertMessage()
                {
                    Title = "BAŞARILI",
                    AlertType = "success",
                    Message = $"{_cariKasa.Aciklama} KASAYA EKLENDİ."

                });

                return RedirectToAction("CariKasa", "CariKasa", new { carihesapid = cariKasa.CariHesap.Id });
            };

            TempData.Put("message", new AlertMessage()
            {
                Title = "HATA",
                AlertType = "danger",
                Message = _cariKasaService.ErrorMessage

            });

            return View(_cariKasa);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult CariKasaFaturaGuncelleFromCari(int? carikasaid)
        {
            ViewBag.Sayfa = "FATURA BİLGİLERİNİ GÜNCELLEME";
            ViewBag.CariGK = _cariGiderKalemiService.GetAll(true, true);

            if (carikasaid == null){return NotFound();}

            ECariKasa cariKasa = _cariKasaService.GetByIdDetay((int)carikasaid);

            if (cariKasa == null){return NotFound();}

            CariKasa _cariKasa = new CariKasa()
            {
                Id = cariKasa.Id,
                Tarih = cariKasa.Tarih,
                Aciklama = cariKasa.Aciklama,
                //Miktar = cariKasa.Miktar,
                //BirimFiyat = cariKasa.BirimFiyat,
                //Borc = cariKasa.Borc,
                //Alacak = cariKasa.Alacak,
                #region VİRGÜL VEYA NOKTA KULLANIMININ İKİSİNİN DE SERBEST OLMASINI SAĞLAMAK İÇİN
                Miktar = Convert.ToString(cariKasa.Miktar),
                BirimFiyat = Convert.ToString(cariKasa.BirimFiyat),
                Borc = Convert.ToString(cariKasa.Borc),
                Alacak = Convert.ToString(cariKasa.Alacak),
                #endregion
                ImgUrl = cariKasa.ImgUrl,
                CekKaynak = cariKasa.CekKaynak,
                NakitKaynak = cariKasa.NakitKaynak,
                SistemeGiris = cariKasa.SistemeGiris,
                SonGuncelleme = cariKasa.SonGuncelleme,
                Durum = cariKasa.Durum,
                CariGiderKalemiId = cariKasa.CariGiderKalemiId,
                CariGiderKalemi = cariKasa.CariGiderKalemi,
                CariHesapId = cariKasa.CariHesapId,
                CariHesap = cariKasa.CariHesap,
            };

            return View(_cariKasa);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult CariKasaFaturaGuncelleFromCari(CariKasa cariKasa)
        {
            ViewBag.Sayfa = "FATURA BİLGİLERİNİ GÜNCELLEME";
            ViewBag.CariGK = _cariGiderKalemiService.GetAll(true, true);

            if (!ModelState.IsValid) { return View(cariKasa); }

            ECariKasa _cariKasa = _cariKasaService.GetById(cariKasa.Id);

            if (_cariKasa == null){return NotFound();}

            _cariKasa.Tarih = cariKasa.Tarih;
            _cariKasa.Aciklama = cariKasa.Aciklama;
            //_cariKasa.Miktar = cariKasa.Miktar;
            //_cariKasa.BirimFiyat = cariKasa.BirimFiyat;
            //_cariKasa.Alacak = cariKasa.Alacak;
            #region VİRGÜL VEYA NOKTA KULLANIMININ İKİSİNİN DE SERBEST OLMASINI SAĞLAMAK İÇİN
            _cariKasa.Miktar = Convert.ToDecimal(cariKasa.Miktar.Replace(".", ","));
            _cariKasa.BirimFiyat = Convert.ToDecimal(cariKasa.BirimFiyat.Replace(".", ","));
            _cariKasa.Borc = Convert.ToDecimal(cariKasa.Borc.Replace(".", ","));
            _cariKasa.Alacak = Convert.ToDecimal(cariKasa.Alacak.Replace(".", ","));
            #endregion
            _cariKasa.ImgUrl = cariKasa.ImgUrl;
            _cariKasa.SonGuncelleme = System.DateTime.Today;

            _cariKasa.CariGiderKalemiId = cariKasa.CariGiderKalemiId;
            _cariKasa.CariHesapId = cariKasa.CariHesapId;

            _cariKasaService.Update(_cariKasa);

            TempData.Put("message", new AlertMessage()
            {
                Title = "BAŞARILI",
                AlertType = "success",
                Message = $"{_cariKasa.Aciklama} GÜNCELLENDİ."

            });

            return RedirectToAction("CariKasa", new { carihesapid = cariKasa.CariHesapId });
        }
        #endregion
    }
}
