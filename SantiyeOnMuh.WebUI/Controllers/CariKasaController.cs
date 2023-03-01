using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;
using SantiyeOnMuh.Business.Abstract;
using SantiyeOnMuh.Entity;
using SantiyeOnMuh.WebUI.Models;

namespace SantiyeOnMuh.WebUI.Controllers
{
    public class CariKasaController : Controller
    {
        // NESNELER ÜZERİNDEKİ İŞLEMLERİ _ OLAN NESNE ÜZERİNDE YAPIP SONRA AKTARIYORUZ - INJECTION
        private ICariKasaService _cariKasaService;
        private ICariHesapService _cariHesapService;
        private ICariGiderKalemiService _cariGiderKalemiService;
        private ISantiyeService _cariService;
        public CariKasaController(
            ICariKasaService cariKasaService,
            ICariHesapService cariHesapService,
            ICariGiderKalemiService cariGiderKalemiService,
            ISantiyeService cariService)
        {
            this._cariKasaService = cariKasaService;
            this._cariHesapService = cariHesapService;
            this._cariGiderKalemiService = cariGiderKalemiService;
            this._cariService = cariService;
        }
        public IActionResult CariKasa(int carihesapid, int? gkid, int page = 1)
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

            //gider kalemi olup olmadığını kontrol ediyoruz, ona göre alt taraftaki toplamın şekli değişecek
            //BAŞLIK

            ViewBag.Sayfa = cariKasaViewModel.CariHesap.Ad + " CARİ HESABI";

            ViewBag.gk = gkid;

            ViewBag.toplamgider = _cariKasaService.GetAll((int)carihesapid, (int?)gkid, true).Sum(i => i.Borc);
            ViewBag.toplamgelir = _cariKasaService.GetAll((int)carihesapid, (int?)gkid, true).Sum(i => i.Alacak);

            return View(cariKasaViewModel);
        }
        [HttpGet]
        public IActionResult CariKasaEkleme()
        {
            ViewBag.Sayfa = "CARİ HESABA FATURA GİRİŞİ";
            ViewBag.CariHesap = _cariHesapService.GetAll(null, true);
            ViewBag.CariGK = _cariGiderKalemiService.GetAll(true, true);
            return View(new CariKasa());
        }
        [HttpPost]
        public async Task<IActionResult> CariKasaEkleme(CariKasa c, IFormFile file)
        {
            #region ESKİ TİP RESİM EKLEME
            //if (file != null)
            //{
            //    string[] permittedExtensions = { ".jpeg", ".pdf", ".png" };
            //    var extention = Path.GetExtension(file.FileName);
            //    if (string.IsNullOrEmpty(extention) || !permittedExtensions.Contains(extention))
            //    {
            //        ViewBag.Sayfa = "FATURA GİRİŞİ";
            //        ViewBag.CariHesap = _cariHesapService.GetAllWithSantiye(null, true);
            //        ViewBag.CariGK = _cariGKService.GetAllWithDetay(true);
            //        c.ImgUrl = null;
            //        return View(c);
            //    }
            //    else
            //    {
            //        var isim = string.Format($"{c.CariHesap.FirmaAdi}{c.Tarih.ToString("yyyy-MM-dd")}{extention}");
            //        c.ImgUrl = isim;
            //        var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\CariKasaResim", isim);

            //        using (var stream = new FileStream(path, FileMode.Create))
            //        {
            //            await file.CopyToAsync(stream);
            //        }
            //    }
            //}
            //else
            //{
            //    ViewBag.Sayfa = "FATURA GİRİŞİ";
            //    ViewBag.CariHesap = _cariHesapService.GetAllWithSantiye(null, true);
            //    ViewBag.CariGK = _cariGKService.GetAllWithDetay(true);
            //    c.ImgUrl = null;
            //    return View(c);
            //};
            #endregion
            #region  RESİM VS. EKLENMEMİŞSE SAYFAYA GERİ GİDİYOR, GERİ GİDİLEN SAYFANIN İHTİYACI OLAN BİLGİLER
            ViewBag.Sayfa = "CARİ HESABA FATURA GİRİŞİ";
            ViewBag.CariHesap = _cariHesapService.GetAll(null, true);
            ViewBag.CariGK = _cariGiderKalemiService.GetAll(true, true);
            //ÜSTTEKİ SIFIRDAN EKLEMENİN BİREBİR AYNISI, GEREKLİ BİLGİLER
            #endregion
            #region RESİM EKLEME BÖLÜMÜ
            if (file != null)
            {
                var extension = Path.GetExtension(file.FileName);

                if (extension == ".jpg" || extension == ".png" || extension == ".pdf")
                {
                    var cariKasaName = string.Format($"{c.Aciklama}{"-"}{Guid.NewGuid()}{extension}");
                    c.ImgUrl = cariKasaName;
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\CariKasaResim", cariKasaName);

                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }
                }
                else { return View(c); }
            }
            else { return View(c); }
            #endregion

            _cariKasaService.Create(c);
            return RedirectToAction("CariKasa", "CariKasa", new { carihesapid = c.CariHesap.Id });
        }
        [HttpGet]
        public IActionResult CariKasaDetay(int? carikasaid)
        {
            ViewBag.Sayfa = "FATURA DETAYI";
            if (carikasaid == null)
            { return NotFound(); }
            CariKasa cariKasa = _cariKasaService.GetById((int)carikasaid);
            if (cariKasa == null)
            { return NotFound(); }
            return View(cariKasa);
        }
        [HttpGet]
        public IActionResult CariKasaSil(int? carikasaid)
        {
            ViewBag.Sayfa = "FATURAYI SİL";

            if (carikasaid == null)
            {
                return NotFound();
            }

            CariKasa cariKasa = _cariKasaService.GetByIdDetay((int)carikasaid);

            if (cariKasa == null)
            {
                return NotFound();
            }
            return View(cariKasa);
        }
        [HttpPost]
        public IActionResult CariKasaSil(CariKasa c)
        {
            var entity = _cariKasaService.GetByIdDetay(c.Id);

            if (entity == null)
            {
                return NotFound();
            }

            entity.SonGuncelleme = System.DateTime.Now;
            entity.Durum = false;

            _cariKasaService.Update(entity);

            return RedirectToAction("CariKasa", "CariKasa", new { carihesapid = entity.CariHesapId });
        }

        //EXCEL
        public IActionResult CariKasaExcel(int carihesapid, int? gkid)
        {
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
        public IActionResult CariKasaGeriYukle(int? carikasaid)
        {
            ViewBag.Sayfa = "FATURAYI SİL";

            if (carikasaid == null)
            {
                return NotFound();
            }

            CariKasa cariKasa = _cariKasaService.GetByIdDetay((int)carikasaid);

            if (cariKasa == null)
            {
                return NotFound();
            }
            return View(cariKasa);
        }
        [HttpPost]
        public IActionResult CariKasaGeriYukle(CariKasa c)
        {
            var entity = _cariKasaService.GetByIdDetay(c.Id);

            if (entity == null)
            {
                return NotFound();
            }

            entity.SonGuncelleme = System.DateTime.Now;
            entity.Durum = true;

            _cariKasaService.Update(entity);

            return RedirectToAction("CariKasa", "CariKasa", new { carihesapid = entity.CariHesapId });
        }

        #region CARİDEN
        [HttpGet]
        public IActionResult CariKasaFaturaEklemeFromCari(int carihesapid)
        {
            ViewBag.Sayfa = _cariHesapService.GetById(carihesapid).Ad + " FİRMA CARİSİNE FATURA GİRİŞİ";
            ViewBag.CariGK = _cariGiderKalemiService.GetAll(true, true);
            ViewBag.CariHesapId = carihesapid;

            return View(new CariKasa());
        }
        [HttpPost]
        public async Task<IActionResult> CariKasaFaturaEklemeFromCari(CariKasa c, IFormFile file)
        {
            #region  RESİM VS. EKLENMEMİŞSE SAYFAYA GERİ GİDİYOR, GERİ GİDİLEN SAYFANIN İHTİYACI OLAN BİLGİLER
            ViewBag.Sayfa = _cariHesapService.GetById(c.CariHesapId).Ad + " FİRMA CARİSİNE FATURA GİRİŞİ";
            ViewBag.CariGK = _cariGiderKalemiService.GetAll(true, true);

            ViewBag.CariHesapId = c.CariHesapId;
            //ÜSTTEKİ SIFIRDAN EKLEMENİN BİREBİR AYNISI, GEREKLİ BİLGİLER
            #endregion
            #region RESİM EKLEME BÖLÜMÜ
            if (file != null)
            {
                var extension = Path.GetExtension(file.FileName);

                if (extension == ".jpg" || extension == ".png" || extension == ".pdf")
                {
                    var cariKasaName = string.Format($"{c.Aciklama}{"-"}{Guid.NewGuid()}{extension}");
                    c.ImgUrl = cariKasaName;
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\CariKasaResim", cariKasaName);

                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }
                }
                else { return View(c); }
            }
            else { return View(c); }
            #endregion

            _cariKasaService.Create(c);

            return RedirectToAction("CariKasa", new { carihesapid = c.CariHesapId });
        }
        [HttpGet]
        public IActionResult CariKasaFaturaGuncelleFromCari(int? carikasaid)
        {

            ViewBag.Sayfa = "FATURA BİLGİLERİNİ GÜNCELLEME";

            ViewBag.CariGK = _cariGiderKalemiService.GetAll(true, true);

            if (carikasaid == null)
            {
                return NotFound();
            }

            CariKasa cariKasa = _cariKasaService.GetByIdDetay((int)carikasaid);

            if (cariKasa == null)
            {
                return NotFound();
            }

            return View(cariKasa);
        }
        [HttpPost]
        public IActionResult CariKasaFaturaGuncelleFromCari(CariKasa c)
        {
            ViewBag.Sayfa = "FATURA BİLGİLERİNİ GÜNCELLEME";
            ViewBag.CariGK = _cariGiderKalemiService.GetAll(true, true);

            var entityCariKasa = _cariKasaService.GetById(c.Id);
            if (entityCariKasa == null)
            {
                return NotFound();
            }

            entityCariKasa.Tarih = c.Tarih;
            entityCariKasa.Aciklama = c.Aciklama;
            entityCariKasa.Miktar = c.Miktar;
            entityCariKasa.BirimFiyat = c.BirimFiyat;
            entityCariKasa.Alacak = c.Alacak;
            entityCariKasa.ImgUrl = c.ImgUrl;
            entityCariKasa.SonGuncelleme = System.DateTime.Today;

            entityCariKasa.CariGiderKalemiId = c.CariGiderKalemiId;
            entityCariKasa.CariHesapId = c.CariHesapId;

            _cariKasaService.Update(entityCariKasa);
            return RedirectToAction("CariKasa", new { carihesapid = c.CariHesapId });
        }
        #endregion

        #region ESKİDEN YAZMIŞIM VEYA KOPYALAMIŞIM TAM BİLMEDİĞİM İÇİN SİLMEDİM
        //[HttpGet]
        //public IActionResult CariKasaGuncelleFromSantiye(int? carikasaid)
        //{

        //    ViewBag.Sayfa = "FATURA BİLGİLERİNİ GÜNCELLEME";
        //    ViewBag.GK = _cariGKService.GetAllWithDetay(true);

        //    if (carikasaid == null)
        //    {
        //        return NotFound();
        //    }

        //    CariKasa cariKasa = _cariKasaService.GetById((int)carikasaid);
        //    ViewBag.CariHesapId = cariKasa.CariHesapId;

        //    if (cariKasa == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(cariKasa);
        //}
        //[HttpPost]
        //public IActionResult CariKasaGuncelleFromSantiye(CariKasa c)
        //{
        //    var entityCariKasa = _cariKasaService.GetById(c.Id);
        //    if (entityCariKasa == null)
        //    {
        //        return NotFound();
        //    }

        //    entityCariKasa.Tarih = c.Tarih;
        //    entityCariKasa.Aciklama = c.Aciklama;
        //    entityCariKasa.Miktar = c.Miktar;
        //    entityCariKasa.BirimFiyat = c.BirimFiyat;
        //    entityCariKasa.Alacak = c.Alacak;
        //    entityCariKasa.ImgUrl = c.ImgUrl;
        //    entityCariKasa.SonGuncelleme = System.DateTime.Today;
        //    entityCariKasa.CariGKId = c.CariHesapId;

        //    _cariKasaService.Update(entityCariKasa);
        //    return RedirectToAction("CariKasa", new { carihesapid = c.CariHesapId });
        //}
        #endregion
    }
}
