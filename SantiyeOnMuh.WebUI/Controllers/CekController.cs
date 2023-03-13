using ClosedXML.Excel;
using DocumentFormat.OpenXml.Office2010.Excel;
using MathNet.Numerics;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SantiyeOnMuh.Business.Abstract;
using SantiyeOnMuh.Entity;
using SantiyeOnMuh.WebUI.Models;
using SantiyeOnMuh.WebUI.Models.Modeller;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.ConstrainedExecution;

namespace SantiyeOnMuh.WebUI.Controllers
{
    public class CekController : Controller
    {
        // NESNELER ÜZERİNDEKİ İŞLEMLERİ _ OLAN NESNE ÜZERİNDE YAPIP SONRA AKTARIYORUZ - INJECTION
        private ICekService _cekService;
        private ISirketService _sirketService;
        private ICariHesapService _cariHesapService;
        private IBankaHesapService _bankaHesapService;
        private IBankaKasaService _bankaKasaService;
        private ICariKasaService _cariKasaService;
        private ISantiyeService _santiyeService;
        public CekController(
            ICekService cekService,
            ISirketService sirketService,
            ICariHesapService cariHesapService,
            IBankaHesapService bankaHesapService,
            IBankaKasaService bankaKasaService,
            ICariKasaService cariKasaService,
            ISantiyeService santiyeService)
        {
            this._cekService = cekService;
            this._sirketService = sirketService;
            this._cariHesapService = cariHesapService;
            this._bankaHesapService = bankaHesapService;
            this._bankaKasaService = bankaKasaService;
            this._cariKasaService = cariKasaService;
            this._santiyeService = santiyeService;
        }
        public IActionResult Index(int page = 1)
        {
            ViewBag.Sayfa = "ÇEKLER";
            const int pageSize = 10;
            var cekViewModel = new CekViewListModel()
            {
                PageInfo = new PageInfo
                {
                    TotalItem = _cekService.GetCount(null, null, null, true),
                    CurrentPage = page,
                    ItemPerPage = pageSize,
                    UrlInfo = null
                },
                Ceks = _cekService.GetAll(null, null, null, true, page, pageSize),
            };
            return View(cekViewModel);
        }

        [HttpGet]
        public IActionResult CekEkleme()
        {
            ViewBag.Sayfa = "YENİ ÇEK EKLEME";
            ViewBag.Sirket = _sirketService.GetAll(true);
            ViewBag.Cari = _cariHesapService.GetAll(null, true);
            ViewBag.Banka = _bankaHesapService.GetAll(true);

            return View(new Cek());
        }

        [HttpPost]
        public async Task<IActionResult> CekEkleme(Cek cek, IFormFile? file) 
        {
            #region RESİM VS. EKLENMEMİŞSE SAYFAYA GERİ GİDİYOR, GERİ GİDİLEN SAYFANIN İHTİYACI OLAN BİLGİLER
            ViewBag.Sayfa = "YENİ ÇEK EKLEME";
            ViewBag.Sirket = _sirketService.GetAll(true);
            ViewBag.Cari = _cariHesapService.GetAll(null, true);
            ViewBag.Banka = _bankaHesapService.GetAll(true);
            #endregion

            if (!ModelState.IsValid) { return View(cek); }

            #region RESİM EKLEME BÖLÜMÜ
            if (file != null)
            {
                var extension = Path.GetExtension(file.FileName);

                if (extension == ".jpg" || extension == ".png" || extension == ".pdf")
                {
                    var cekName = string.Format($"{cek.CekNo}{"-"}{cek.Aciklama}{"-"}{Guid.NewGuid()}{extension}");
                    cek.ImgUrl = cekName;
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\CekResim", cekName);

                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }
                }
                else { return View(cek); }
            }
            //else { return View(cek); }
            #endregion

            ECek _cek = new ECek()
            {
                Tarih = cek.Tarih,
                Aciklama = cek.Aciklama,
                CekNo = cek.CekNo,
                Tutar = Convert.ToDecimal(cek.Tutar.Replace(".",",")),
                ImgUrl = cek.ImgUrl,
                BankaKasaKaynak = cek.BankaKasaKaynak,
                CariKasaKaynak = cek.CariKasaKaynak,
                SistemeGiris = cek.SistemeGiris,
                SonGuncelleme = cek.SonGuncelleme,
                Durum = cek.Durum,
                OdemeDurumu = cek.OdemeDurumu,
                CariHesapId = cek.CariHesapId,
                CariHesap = cek.CariHesap,
                SirketId = cek.SirketId,
                Sirket = cek.Sirket,
                BankaHesapId = cek.BankaHesapId,
                BankaHesap = cek.BankaHesap,
            };

            if (_cekService.Create(_cek))
            {
                //CARİ KASA İÇİN ÇEK OLUŞTURULDU VE ÇEK KAYNAĞI İLE EKLENDİ
                ECariKasa _cariKasa = new ECariKasa()
                {
                    Tarih = cek.Tarih,
                    Aciklama = cek.CekNo + " NOLU CEK ÖDEMESİ " + cek.Aciklama,
                    Miktar = 1,
                    BirimFiyat = 1,
                    Borc = Convert.ToDecimal(cek.Tutar.Replace(".", ",")),
                    Alacak = 0,
                    ImgUrl = null,
                    CekKaynak = _cek.Id,
                    CariGiderKalemiId = 1,
                    CariHesapId = cek.CariHesapId
                };

                _cariKasaService.Create(_cariKasa);

                #region ÇEKE-CARİ KASA BAĞLANTISI
                //ŞİMDİ DE ÇEKE, CARİ KAYNAĞI EKLENİYOR-GÜNCELLENİYOR
                var _eklenenCek = _cekService.GetById(_cek.Id);

                if (_eklenenCek == null) { return NotFound(); }

                _eklenenCek.CariKasaKaynak = _cariKasa.Id;

                _cekService.Update(_eklenenCek);
                #endregion

                AlertMessage msg = new AlertMessage()
                {
                    Message = $"{_cek.CekNo} NUMARALI ÇEK EKLENDİ.",
                    AlertType = "success"
                };

                TempData["message"] = JsonConvert.SerializeObject(msg);

                return RedirectToAction("Index");
            };
            return View(cek);
        }

        [HttpGet]
        public IActionResult CekTahsil(int? cekid)
        {
            ViewBag.Sayfa = "ÇEK TAHSİL";

            if (cekid == null) { return NotFound(); }

            ECek c = _cekService.GetByIdDetay((int)cekid);

            if (c == null) { return NotFound(); }

            var entity = _cekService.GetById(c.Id);

            if (entity == null) { return NotFound(); }

            //CARİ KASA İÇİN ÇEK OLUŞTURULDU VE ÇEK KAYNAĞI İLE EKLENDİ
            String FirmaAdiForAciklama = _cariHesapService.GetById((int)c.CariHesapId).Ad;

            EBankaKasa EntityBankaKasa = new EBankaKasa()
            {
                Tarih = entity.Tarih,
                Aciklama = FirmaAdiForAciklama + " AİT " + entity.CekNo + " NOLU ÇEK ÖDEMESİ. " + " - " + entity.Aciklama,
                Nitelik = "ÇEK",
                Cikan = entity.Tutar,
                Giren = 0,
                CekKaynak = entity.Id,
                BankaHesapId = entity.BankaHesapId,
                Durum = true
            };

            _bankaKasaService.Create(EntityBankaKasa);

            entity.Durum = true;
            entity.OdemeDurumu = true;
            entity.SonGuncelleme = DateTime.Now;
            entity.BankaKasaKaynak = EntityBankaKasa.Id;

            _cekService.Update(entity);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult CekGuncelle(int? cekid)
        {
            ViewBag.Sayfa = "ÇEK BİLGİLERİNİ GÜNCELLEME";
            ViewBag.Sirket = _sirketService.GetAll(true);
            ViewBag.Cari = _cariHesapService.GetAll(null, true);
            ViewBag.Banka = _bankaHesapService.GetAll(true);

            if (cekid == null) { return NotFound();}

            ECek cek = _cekService.GetById((int)cekid);

            if (cek == null) { return NotFound(); }

            Cek _cek = new Cek()
            {
                Id= cek.Id,
                Tarih = cek.Tarih,
                Aciklama = cek.Aciklama,
                CekNo = cek.CekNo,
                Tutar = Convert.ToString(cek.Tutar),
                ImgUrl = cek.ImgUrl,
                BankaKasaKaynak = cek.BankaKasaKaynak,
                CariKasaKaynak = cek.CariKasaKaynak,
                SistemeGiris = cek.SistemeGiris,
                SonGuncelleme = cek.SonGuncelleme,
                Durum = cek.Durum,
                OdemeDurumu = cek.OdemeDurumu,
                CariHesapId = cek.CariHesapId,
                CariHesap = cek.CariHesap,
                SirketId = cek.SirketId,
                Sirket = cek.Sirket,
                BankaHesapId = cek.BankaHesapId,
                BankaHesap = cek.BankaHesap,
            };
            

            return View(_cek);
        }

        [HttpPost]
        public IActionResult CekGuncelle(Cek c)
        {
            ViewBag.Sayfa = "ÇEK BİLGİLERİNİ GÜNCELLEME";
            ViewBag.Sirket = _sirketService.GetAll(true);
            ViewBag.Cari = _cariHesapService.GetAll(null, true);
            ViewBag.Banka = _bankaHesapService.GetAll(true);

            if (!ModelState.IsValid) { return View(c); }

            ECek _cek = _cekService.GetById(c.Id);

            if (_cek == null) { return NotFound(); }

            int? bankakasaid = _cek.BankaKasaKaynak;
            int? carikasaid = _cek.CariKasaKaynak;

            if (bankakasaid != null)
            {
                String FirmaAdiForAciklama = _cariHesapService.GetById((int)c.CariHesapId).Ad;
                EBankaKasa _bankaKasa = _bankaKasaService.GetById((int)bankakasaid);

                if (_bankaKasa == null)
                { return NotFound(); }

                _bankaKasa.Tarih = c.Tarih;
                _bankaKasa.Aciklama = FirmaAdiForAciklama + " AİT " + c.CekNo + " NOLU ÇEK ÖDEMESİ" + " - " + c.Aciklama;
                _bankaKasa.Cikan = Convert.ToDecimal(c.Tutar.Replace(".",","));
                _bankaKasa.BankaHesapId = c.BankaHesapId;
                _bankaKasa.SonGuncelleme = DateTime.Now;

                _bankaKasaService.Update(_bankaKasa);
            }

            if (carikasaid != null)
            {
                ECariKasa _cariKasa = _cariKasaService.GetById((int)carikasaid);

                if (_cariKasa == null) { return NotFound(); }

                _cariKasa.Tarih = c.Tarih;
                _cariKasa.Aciklama = c.CekNo + " NOLU CEK ÖDEMESİ" + " - " + c.Aciklama;
                _cariKasa.Borc = Convert.ToDecimal(c.Tutar.Replace(".", ","));
                _cariKasa.SonGuncelleme = DateTime.Now;
                _cariKasa.CariHesapId = c.CariHesapId;

                _cariKasaService.Update(_cariKasa);
            }

            _cek.Tarih = c.Tarih;
            _cek.Aciklama = c.Aciklama;
            _cek.CekNo = c.CekNo;
            _cek.Tutar = Convert.ToDecimal(c.Tutar.Replace(".",","));
            _cek.ImgUrl = c.ImgUrl;
            _cek.SonGuncelleme = System.DateTime.Now;
            _cek.CariHesapId = c.CariHesapId;
            _cek.SirketId = c.SirketId;
            _cek.BankaHesapId = c.BankaHesapId;

            _cekService.Update(_cek);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult CekDetay(int? cekid)
        {
            ViewBag.Sayfa = "ÇEK DETAYI";

            if (cekid == null) { return NotFound(); }

            ECek cek = _cekService.GetByIdDetay((int)cekid);

            if (cek == null) { return NotFound(); }

            Cek _cek = new Cek()
            {
                Id = cek.Id,
                Tarih = cek.Tarih,
                Aciklama = cek.Aciklama,
                CekNo = cek.CekNo,
                Tutar = Convert.ToString(cek.Tutar),
                ImgUrl = cek.ImgUrl,
                BankaKasaKaynak = cek.BankaKasaKaynak,
                CariKasaKaynak = cek.CariKasaKaynak,
                SistemeGiris = cek.SistemeGiris,
                SonGuncelleme = cek.SonGuncelleme,
                Durum = cek.Durum,
                OdemeDurumu = cek.OdemeDurumu,
                CariHesapId = cek.CariHesapId,
                CariHesap = cek.CariHesap,
                SirketId = cek.SirketId,
                Sirket = cek.Sirket,
                BankaHesapId = cek.BankaHesapId,
                BankaHesap = cek.BankaHesap,
            };

            return View(_cek);
        }

        [HttpGet]
        public IActionResult CekSil(int? cekid)
        {
            ViewBag.Sayfa = "ÇEKİ SİL";

            if (cekid == null) { return NotFound(); }

            ECek cek = _cekService.GetByIdDetay((int)cekid);

            if (cek == null) { return NotFound(); }

            Cek _cek = new Cek()
            {
                Id=cek.Id,
                Tarih = cek.Tarih,
                Aciklama = cek.Aciklama,
                CekNo = cek.CekNo,
                Tutar = Convert.ToString(cek.Tutar),
                ImgUrl = cek.ImgUrl,
                BankaKasaKaynak = cek.BankaKasaKaynak,
                CariKasaKaynak = cek.CariKasaKaynak,
                SistemeGiris = cek.SistemeGiris,
                SonGuncelleme = cek.SonGuncelleme,
                Durum = cek.Durum,
                OdemeDurumu = cek.OdemeDurumu,
                CariHesapId = cek.CariHesapId,
                CariHesap = cek.CariHesap,
                SirketId = cek.SirketId,
                Sirket = cek.Sirket,
                BankaHesapId = cek.BankaHesapId,
                BankaHesap = cek.BankaHesap,
            };

            ViewBag.HesapAdi = _bankaHesapService.GetById(cek.BankaHesapId).HesapAdi;
            return View(_cek);
        }

        [HttpPost]
        public IActionResult CekSil(Cek c)
        {

            ECek _cek = _cekService.GetByIdDetay(c.Id);

            if (_cek == null) { return NotFound(); }

            _cek.SonGuncelleme = System.DateTime.Now;
            _cek.Durum = false;

            int? bankakasaid = _cek.BankaKasaKaynak;
            int? carikasaid = _cek.CariKasaKaynak;

            if (bankakasaid != null)
            {
                EBankaKasa entityBankaKasa = _bankaKasaService.GetById((int)bankakasaid);

                if (entityBankaKasa == null) { return NotFound(); }

                entityBankaKasa.SonGuncelleme = System.DateTime.Now;
                entityBankaKasa.Durum = false;

                _bankaKasaService.Update(entityBankaKasa);
            }

            if (carikasaid != null)
            {
                ECariKasa entityCariKasa = _cariKasaService.GetById((int)carikasaid);

                if (entityCariKasa == null) { return NotFound(); }

                entityCariKasa.SonGuncelleme = System.DateTime.Now;
                entityCariKasa.Durum = false;

                _cariKasaService.Update(entityCariKasa);
            }

            _cekService.Update(_cek);

            return RedirectToAction("Index");
        }

        public IActionResult CekGeriYukle(int cekid)
        {

            ECek _cek = _cekService.GetByIdDetay(cekid);

            if (_cek == null) { return NotFound(); }

            _cek.SonGuncelleme = System.DateTime.Now;
            _cek.Durum = true;

            _cekService.Update(_cek);

            int? bankakasaid = _cek.BankaKasaKaynak;
            int? carikasaid = _cek.CariKasaKaynak;

            if (bankakasaid != null)
            {
                EBankaKasa entityBankaKasa = _bankaKasaService.GetById((int)bankakasaid);

                if (entityBankaKasa == null) { return NotFound(); }

                entityBankaKasa.SonGuncelleme = System.DateTime.Now;
                entityBankaKasa.Durum = true;

                _bankaKasaService.Update(entityBankaKasa);
            }

            if (carikasaid != null)
            {
                ECariKasa entityCariKasa = _cariKasaService.GetById((int)carikasaid);

                if (entityCariKasa == null) { return NotFound(); }

                entityCariKasa.SonGuncelleme = System.DateTime.Now;
                entityCariKasa.Durum = true;

                _cariKasaService.Update(entityCariKasa);
            }

            return RedirectToAction("Index");
        }

        //EXCEL
        public IActionResult CekExcel()
        {
            var cekViewModel = new CekViewListModel()
            {
                Ceks = _cekService.GetAll(null, null, null, true),
            };

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("ÇEK LİSTESİ");
                var currentRow = 1;
                decimal toplamodenmis = 0;
                decimal toplamodenmemis = 0;
                decimal toplam = 0;

                #region Header
                worksheet.Cell(currentRow, 1).Value = "TARİH";
                worksheet.Cell(currentRow, 2).Value = "ÇEK NO";
                worksheet.Cell(currentRow, 3).Value = "AÇIKLAMA";
                worksheet.Cell(currentRow, 4).Value = "İŞLENEN ÇEK";
                worksheet.Cell(currentRow, 5).Value = "ÖDENEN ÇEK";
                worksheet.Cell(currentRow, 6).Value = "TOPLAM";

                for (int i = 1; i < 7; i++)
                {
                    worksheet.Cell(currentRow, i).Style.Font.Bold = true;
                    worksheet.Cell(currentRow, i).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                }
                #endregion

                #region Body
                foreach (var cek in cekViewModel.Ceks)
                {
                    currentRow++;
                    worksheet.Cell(currentRow, 1).Value = cek.Tarih;
                    worksheet.Cell(currentRow, 2).Value = cek.CekNo;
                    worksheet.Cell(currentRow, 3).Value = cek.Aciklama;

                    if (cek.OdemeDurumu == false)
                    {
                        worksheet.Cell(currentRow, 4).Value = cek.Tutar;
                        toplamodenmemis = toplamodenmemis + cek.Tutar;
                    }
                    else
                    {
                        worksheet.Cell(currentRow, 5).Value = cek.Tutar;
                        toplamodenmis = toplamodenmis + cek.Tutar;
                    }

                    toplam = toplam + cek.Tutar;
                    worksheet.Cell(currentRow, 6).Value = toplam;

                    for (int i = 1; i < 7; i++)
                    {
                        worksheet.Cell(currentRow, i).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    }

                }
                #endregion

                #region FOOTER
                worksheet.Cell(currentRow + 2, 4).Value = toplamodenmemis;
                worksheet.Cell(currentRow + 2, 5).Value = toplamodenmis;
                worksheet.Cell(currentRow + 2, 6).Value = toplam;
                for (int i = 4; i < 7; i++)
                {
                    worksheet.Cell(currentRow + 2, i).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    worksheet.Cell(currentRow + 2, i).Style.Font.Bold = true;
                }

                #endregion

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();

                    return File(
                        content,
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        "CekListesi.xlsx"
                        );
                }
            }
        }

        //ARŞİV
        public IActionResult CekArsiv(int page = 1)
        {
            ViewBag.Sayfa = "SİLİNMİŞ ÇEKLER";
            const int pageSize = 10;
            var cekViewModel = new CekViewListModel()
            {
                PageInfo = new PageInfo
                {
                    TotalItem = _cekService.GetCount(null, null, null, false),
                    CurrentPage = page,
                    ItemPerPage = pageSize,
                    UrlInfo = null
                },
                Ceks = _cekService.GetAll(null, null, null, false, page, pageSize),
            };
            return View(cekViewModel);
        }

        #region CARİ ÜZERİNDEN
        [HttpGet]
        public IActionResult CekEklemeFromCari(int carihesapid)
        {
            ViewBag.Sayfa = _cariHesapService.GetById((int)carihesapid).Ad + " CARİSİNE YENİ ÇEK GİRİŞİ";

            ViewBag.Sirket = _sirketService.GetAll(true);
            ViewBag.Banka = _bankaHesapService.GetAll(true);

            ViewBag.CariHesapId = carihesapid;

            return View(new Cek());
        }

        [HttpPost]
        public async Task<IActionResult> CekEklemeFromCari(Cek cek, IFormFile file)
        {
            if (!ModelState.IsValid) { return View(cek); }

            #region  RESİM VS. EKLENMEMİŞSE SAYFAYA GERİ GİDİYOR, GERİ GİDİLEN SAYFANIN İHTİYACI OLAN BİLGİLER
            ViewBag.Sayfa = _cariHesapService.GetById((int)cek.CariHesapId).Ad + " CARİSİNE YENİ ÇEK GİRİŞİ";

            ViewBag.Sirket = _sirketService.GetAll(true);
            ViewBag.Banka = _bankaHesapService.GetAll(true);

            ViewBag.CariHesapId = cek.CariHesapId;
            //ÜSTTEKİ SIFIRDAN EKLEMENİN BİREBİR AYNISI, GEREKLİ BİLGİLER
            #endregion
            #region RESİM EKLEME BÖLÜMÜ
            if (file != null)
            {
                var extension = Path.GetExtension(file.FileName);

                if (extension == ".jpg" || extension == ".png" || extension == ".pdf")
                {
                    var cekName = string.Format($"{cek.CekNo}{"-"}{cek.Aciklama}{"-"}{Guid.NewGuid()}{extension}");
                    cek.ImgUrl = cekName;
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\CekResim", cekName);

                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }
                }
                else { return View(cek); }
            }
            //else { return View(cek); }
            #endregion
            ECek _cek = new ECek()
            {
                Tarih = cek.Tarih,
                Aciklama = cek.Aciklama,
                CekNo = cek.CekNo,
                Tutar = Convert.ToDecimal(cek.Tutar.Replace(".",",")),
                ImgUrl = cek.ImgUrl,
                BankaKasaKaynak = cek.BankaKasaKaynak,
                CariKasaKaynak = cek.CariKasaKaynak,
                SistemeGiris = cek.SistemeGiris,
                SonGuncelleme = cek.SonGuncelleme,
                Durum = cek.Durum,
                OdemeDurumu = cek.OdemeDurumu,
                CariHesapId = cek.CariHesapId,
                CariHesap = cek.CariHesap,
                SirketId = cek.SirketId,
                Sirket = cek.Sirket,
                BankaHesapId = cek.BankaHesapId,
                BankaHesap = cek.BankaHesap,
            };
            _cekService.Create(_cek);
            //CARİ KASA İÇİN ÇEK OLUŞTURULDU VE ÇEK KAYNAĞI İLE EKLENDİ
            ECariKasa EntityCariKasa = new ECariKasa()
            {
                Tarih = cek.Tarih,
                Aciklama = cek.CekNo + " NOLU CEK ÖDEMESİ " + cek.Aciklama,
                Miktar = 1,
                BirimFiyat = 1,
                Borc = Convert.ToDecimal(cek.Tutar.Replace(".",",")),
                Alacak = 0,
                ImgUrl = null,
                CekKaynak = cek.Id,
                CariGiderKalemiId = 1,
                CariHesapId = cek.CariHesapId
            };
            _cariKasaService.Create(EntityCariKasa);
            //ŞİMDİ DE ÇEKE, CARİ KAYNAĞI EKLENİYOR-GÜNCELLENİYOR
            var EntityEklenenCek = _cekService.GetById(cek.Id);

            if (EntityEklenenCek == null){ return NotFound(); }

            EntityEklenenCek.CariKasaKaynak = EntityCariKasa.Id;

            _cekService.Update(EntityEklenenCek);

            return RedirectToAction("CariKasa", "CariKasa", new { carihesapid = cek.CariHesapId });
        }
        #endregion

        #region BASİT FİLTRE
        public IActionResult AraSecim()
        {
            ViewBag.Sayfa = "ÇEKLER";
            var cekViewModel = new CekViewListModel()
            {
                Santiyes = _santiyeService.GetAll(true),
                BankaHesaps = _bankaHesapService.GetAll(true),
                Sirkets = _sirketService.GetAll(true),
            };

            return View(cekViewModel);
        }

        public IActionResult CekSantiye(int santiyeid, int page = 1)
        {
            ViewBag.Sayfa = "ÇEKLER";
            const int pageSize = 10;
            var cekViewModel = new CekViewListModel()
            {
                PageInfo = new PageInfo
                {
                    TotalItem = _cekService.GetCount(santiyeid, null, null, true),
                    CurrentPage = page,
                    ItemPerPage = pageSize,
                    UrlInfo = santiyeid
                },
                Ceks = _cekService.GetAll(santiyeid, null, null, true, page, pageSize),
            };
            return View(cekViewModel);
        }

        public IActionResult CekSirket(int sirketid, int page = 1)
        {
            ViewBag.Sayfa = "ÇEKLER";
            const int pageSize = 10;
            var cekViewModel = new CekViewListModel()
            {
                PageInfo = new PageInfo
                {
                    TotalItem = _cekService.GetCount(null, sirketid, null, true),
                    CurrentPage = page,
                    ItemPerPage = pageSize,
                    UrlInfo = sirketid
                },
                Ceks = _cekService.GetAll(null, sirketid, null, true, page, pageSize),
            };
            return View(cekViewModel);
        }

        public IActionResult CekBanka(int bankahesapid, int page = 1)
        {
            ViewBag.Sayfa = "ÇEKLER";
            const int pageSize = 10;
            var cekViewModel = new CekViewListModel()
            {
                PageInfo = new PageInfo
                {
                    TotalItem = _cekService.GetCount(null, null, bankahesapid, true),
                    CurrentPage = page,
                    ItemPerPage = pageSize,
                    UrlInfo = bankahesapid
                },
                Ceks = _cekService.GetAll(null, null, bankahesapid, true, page, pageSize),
            };
            return View(cekViewModel);
        }

        //BASİT FİLTRE EXCEL
        public IActionResult CekSantiyeExcel(int santiyeid)
        {
            var cekViewModel = new CekViewListModel()
            {
                Ceks = _cekService.GetAll(santiyeid, null, null, true),
            };

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("ÇEK LİSTESİ");
                var currentRow = 1;
                decimal toplamodenmis = 0;
                decimal toplamodenmemis = 0;
                decimal toplam = 0;

                #region Header
                worksheet.Cell(currentRow, 1).Value = "TARİH";
                worksheet.Cell(currentRow, 2).Value = "ÇEK NO";
                worksheet.Cell(currentRow, 3).Value = "AÇIKLAMA";
                worksheet.Cell(currentRow, 4).Value = "İŞLENEN ÇEK";
                worksheet.Cell(currentRow, 5).Value = "ÖDENEN ÇEK";
                worksheet.Cell(currentRow, 6).Value = "TOPLAM";

                for (int i = 1; i < 7; i++)
                {
                    worksheet.Cell(currentRow, i).Style.Font.Bold = true;
                    worksheet.Cell(currentRow, i).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                }
                #endregion

                #region Body
                foreach (var cek in cekViewModel.Ceks)
                {
                    currentRow++;
                    worksheet.Cell(currentRow, 1).Value = cek.Tarih;
                    worksheet.Cell(currentRow, 2).Value = cek.CekNo;
                    worksheet.Cell(currentRow, 3).Value = cek.Aciklama;

                    if (cek.OdemeDurumu == false)
                    {
                        worksheet.Cell(currentRow, 4).Value = cek.Tutar;
                        toplamodenmemis = toplamodenmemis + cek.Tutar;
                    }
                    else
                    {
                        worksheet.Cell(currentRow, 5).Value = cek.Tutar;
                        toplamodenmis = toplamodenmis + cek.Tutar;
                    }

                    toplam = toplam + cek.Tutar;
                    worksheet.Cell(currentRow, 6).Value = toplam;

                    for (int i = 1; i < 7; i++)
                    {
                        worksheet.Cell(currentRow, i).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    }

                }
                #endregion

                #region FOOTER
                worksheet.Cell(currentRow + 2, 4).Value = toplamodenmemis;
                worksheet.Cell(currentRow + 2, 5).Value = toplamodenmis;
                worksheet.Cell(currentRow + 2, 6).Value = toplam;
                for (int i = 4; i < 7; i++)
                {
                    worksheet.Cell(currentRow + 2, i).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    worksheet.Cell(currentRow + 2, i).Style.Font.Bold = true;
                }

                #endregion

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();

                    return File(
                        content,
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        "CekListesi.xlsx"
                        );
                }
            }
        }

        public IActionResult CekSirketExcel(int sirketid)
        {
            var cekViewModel = new CekViewListModel()
            {
                Ceks = _cekService.GetAll(null, sirketid, null, true),
            };

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("ÇEK LİSTESİ");
                var currentRow = 1;
                decimal toplamodenmis = 0;
                decimal toplamodenmemis = 0;
                decimal toplam = 0;

                #region Header
                worksheet.Cell(currentRow, 1).Value = "TARİH";
                worksheet.Cell(currentRow, 2).Value = "ÇEK NO";
                worksheet.Cell(currentRow, 3).Value = "AÇIKLAMA";
                worksheet.Cell(currentRow, 4).Value = "İŞLENEN ÇEK";
                worksheet.Cell(currentRow, 5).Value = "ÖDENEN ÇEK";
                worksheet.Cell(currentRow, 6).Value = "TOPLAM";

                for (int i = 1; i < 7; i++)
                {
                    worksheet.Cell(currentRow, i).Style.Font.Bold = true;
                    worksheet.Cell(currentRow, i).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                }
                #endregion

                #region Body
                foreach (var cek in cekViewModel.Ceks)
                {
                    currentRow++;
                    worksheet.Cell(currentRow, 1).Value = cek.Tarih;
                    worksheet.Cell(currentRow, 2).Value = cek.CekNo;
                    worksheet.Cell(currentRow, 3).Value = cek.Aciklama;

                    if (cek.OdemeDurumu == false)
                    {
                        worksheet.Cell(currentRow, 4).Value = cek.Tutar;
                        toplamodenmemis = toplamodenmemis + cek.Tutar;
                    }
                    else
                    {
                        worksheet.Cell(currentRow, 5).Value = cek.Tutar;
                        toplamodenmis = toplamodenmis + cek.Tutar;
                    }

                    toplam = toplam + cek.Tutar;
                    worksheet.Cell(currentRow, 6).Value = toplam;

                    for (int i = 1; i < 7; i++)
                    {
                        worksheet.Cell(currentRow, i).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    }

                }
                #endregion

                #region FOOTER
                worksheet.Cell(currentRow + 2, 4).Value = toplamodenmemis;
                worksheet.Cell(currentRow + 2, 5).Value = toplamodenmis;
                worksheet.Cell(currentRow + 2, 6).Value = toplam;
                for (int i = 4; i < 7; i++)
                {
                    worksheet.Cell(currentRow + 2, i).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    worksheet.Cell(currentRow + 2, i).Style.Font.Bold = true;
                }

                #endregion

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();

                    return File(
                        content,
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        "CekListesi.xlsx"
                        );
                }
            }
        }

        public IActionResult CekBankaExcel(int bankahesapid)
        {
            var cekViewModel = new CekViewListModel()
            {
                Ceks = _cekService.GetAll(null, null, bankahesapid, true),
            };

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("ÇEK LİSTESİ");
                var currentRow = 1;
                decimal toplamodenmis = 0;
                decimal toplamodenmemis = 0;
                decimal toplam = 0;

                #region Header
                worksheet.Cell(currentRow, 1).Value = "TARİH";
                worksheet.Cell(currentRow, 2).Value = "ÇEK NO";
                worksheet.Cell(currentRow, 3).Value = "AÇIKLAMA";
                worksheet.Cell(currentRow, 4).Value = "İŞLENEN ÇEK";
                worksheet.Cell(currentRow, 5).Value = "ÖDENEN ÇEK";
                worksheet.Cell(currentRow, 6).Value = "TOPLAM";

                for (int i = 1; i < 7; i++)
                {
                    worksheet.Cell(currentRow, i).Style.Font.Bold = true;
                    worksheet.Cell(currentRow, i).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                }
                #endregion

                #region Body
                foreach (var cek in cekViewModel.Ceks)
                {
                    currentRow++;
                    worksheet.Cell(currentRow, 1).Value = cek.Tarih;
                    worksheet.Cell(currentRow, 2).Value = cek.CekNo;
                    worksheet.Cell(currentRow, 3).Value = cek.Aciklama;

                    if (cek.OdemeDurumu == false)
                    {
                        worksheet.Cell(currentRow, 4).Value = cek.Tutar;
                        toplamodenmemis = toplamodenmemis + cek.Tutar;
                    }
                    else
                    {
                        worksheet.Cell(currentRow, 5).Value = cek.Tutar;
                        toplamodenmis = toplamodenmis + cek.Tutar;
                    }

                    toplam = toplam + cek.Tutar;
                    worksheet.Cell(currentRow, 6).Value = toplam;

                    for (int i = 1; i < 7; i++)
                    {
                        worksheet.Cell(currentRow, i).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    }

                }
                #endregion

                #region FOOTER
                worksheet.Cell(currentRow + 2, 4).Value = toplamodenmemis;
                worksheet.Cell(currentRow + 2, 5).Value = toplamodenmis;
                worksheet.Cell(currentRow + 2, 6).Value = toplam;
                for (int i = 4; i < 7; i++)
                {
                    worksheet.Cell(currentRow + 2, i).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    worksheet.Cell(currentRow + 2, i).Style.Font.Bold = true;
                }

                #endregion

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();

                    return File(
                        content,
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        "CekListesi.xlsx"
                        );
                }
            }
        }

        #endregion

    }
}
