using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;
using SantiyeOnMuh.Business.Abstract;
using SantiyeOnMuh.Entity;
using SantiyeOnMuh.WebUI.Models;
using SantiyeOnMuh.WebUI.Models.Modeller;
using System.ComponentModel.DataAnnotations;

namespace SantiyeOnMuh.WebUI.Controllers
{
    public class SantiyeKasaController : Controller
    {
        // NESNELER ÜZERİNDEKİ İŞLEMLERİ _ OLAN NESNE ÜZERİNDE YAPIP SONRA AKTARIYORUZ - INJECTION
        private ISantiyeKasaService _santiyeKasaService;
        private ISantiyeGiderKalemiService _santiyeGiderKalemiService;
        private ISantiyeService _santiyeService;

        public SantiyeKasaController(
            ISantiyeKasaService santiyeKasaService,
            ISantiyeGiderKalemiService santiyeGiderKalemiService,
            ISantiyeService santiyeService)
        {
            this._santiyeKasaService = santiyeKasaService;
            this._santiyeGiderKalemiService = santiyeGiderKalemiService;
            this._santiyeService = santiyeService;
        }

        public IActionResult SantiyeKasa(int santiyeid, int? gkid, int page = 1)
        {
            //BAŞLIKTA ŞANTİYENİN ADININ YAZMASI İÇİN
            ViewBag.Sayfa = _santiyeService.GetById(santiyeid).Ad + " ŞANTİYE KASASI ";

            const int pageSize = 10;
            var santiyeKasaViewModel = new SantiyeKasaViewListModel()
            {
                PageInfo = new PageInfo
                {
                    TotalItem = _santiyeKasaService.GetCount((int)santiyeid, (int?)gkid, true),
                    CurrentPage = page,
                    ItemPerPage = pageSize,
                    UrlInfo = (int?)santiyeid
                },

                SantiyeKasas = _santiyeKasaService.GetAll((int)santiyeid, (int?)gkid, true, page, pageSize),
                SantiyeGiderKalemis = _santiyeGiderKalemiService.GetAll(true, true),
                Santiye = _santiyeService.GetById(santiyeid)
            };

            //gider kalemi olup olmadığını kontrol ediyoruz, ona göre alt taraftaki toplamın şekli değişecek
            ViewBag.gk = gkid;
            //
            ViewBag.toplamgider = _santiyeKasaService.GetAll((int)santiyeid, (int?)gkid, true).Sum(i => i.Gider);
            ViewBag.toplamgelir = _santiyeKasaService.GetAll((int)santiyeid, (int?)gkid, true).Sum(i => i.Gelir);

            return View(santiyeKasaViewModel);
        }
        [HttpGet]
        public IActionResult SantiyeKasaEkleme()
        {
            ViewBag.Sayfa = "GİDER EKLEME";

            ViewBag.GK = _santiyeGiderKalemiService.GetAll(true, true);
            ViewBag.Santiye = _santiyeService.GetAll(true);

            return View(new ESantiyeKasa());
        }
        [HttpPost]
        public async Task<IActionResult> SantiyeKasaEkleme(SantiyeKasa santiyeKasa, IFormFile file)
        {

            #region EĞER RESİM EKLİ DEĞİLSE GERİ DÖNÜŞTE GEREKLİ BİLGİLER
            ViewBag.Sayfa = "GİDER EKLEME";

            ViewBag.GK = _santiyeGiderKalemiService.GetAll(true, true);
            ViewBag.Santiye = _santiyeService.GetAll(true);
            #endregion
            #region RESİM EKLEME BÖLÜMÜ
            if (file != null)
            {
                var extension = Path.GetExtension(file.FileName);

                if (extension == ".jpg" || extension == ".png" || extension == ".pdf")
                {
                    var PicName = string.Format($"{santiyeKasa.Aciklama}{"-"}{Guid.NewGuid()}{extension}");
                    santiyeKasa.ImgUrl = PicName;
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\SantiyeKasaResim", PicName);

                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }
                }
                else { return View(santiyeKasa); }
            }
            //else { return View(santiyeKasa); }
            //FATURA EKLENMESE BİLE SİSTEME FATURA GİRİLEBİLSİN DİYE ELSE KISMINI ÇIKARDIM
            #endregion

            ESantiyeKasa _santiyeKasa = new ESantiyeKasa()
            {
                Tarih = santiyeKasa.Tarih,
                Aciklama = santiyeKasa.Aciklama,
                Kisi = santiyeKasa.Kisi,
                No = santiyeKasa.No,

                #region VİRGÜL VEYA NOKTA KULLANIMININ İKİSİNİN DE SERBEST OLMASINI SAĞLAMAK İÇİN
                Gelir = Convert.ToDecimal(santiyeKasa.Gelir.Replace(".", ",")),
                Gider = Convert.ToDecimal(santiyeKasa.Gider.Replace(".", ",")),
                #endregion

                ImgUrl = santiyeKasa.ImgUrl,
                Durum = santiyeKasa.Durum,
                BankaKasaKaynak = santiyeKasa.BankaKasaKaynak,
                SistemeGiris = santiyeKasa.SistemeGiris,
                SonGuncelleme = santiyeKasa.SonGuncelleme,
                SantiyeGiderKalemiId = santiyeKasa.SantiyeGiderKalemiId,
                SantiyeGiderKalemi = santiyeKasa.SantiyeGiderKalemi,
                SantiyeId = santiyeKasa.SantiyeId,
                Santiye = santiyeKasa.Santiye,
            };

            _santiyeKasaService.Create(_santiyeKasa);

            return RedirectToAction("Index", "Santiye");
        }
        [HttpGet]
        public IActionResult SantiyeKasaDetay(int? id)
        {
            ViewBag.Sayfa = "GİDER DETAYI";

            if (id == null){ return NotFound();}

            ESantiyeKasa santiyeKasa = _santiyeKasaService.GetById((int)id);

            if (santiyeKasa == null){return NotFound();}

            SantiyeKasa _santiyeKasa = new SantiyeKasa()
            {
                Id = santiyeKasa.Id,
                Tarih = santiyeKasa.Tarih,
                Aciklama = santiyeKasa.Aciklama,
                Kisi = santiyeKasa.Kisi,
                No = santiyeKasa.No,

                Gelir = Convert.ToString(santiyeKasa.Gelir),
                Gider = Convert.ToString(santiyeKasa.Gider),

                ImgUrl = santiyeKasa.ImgUrl,
                Durum = santiyeKasa.Durum,
                BankaKasaKaynak = santiyeKasa.BankaKasaKaynak,
                SistemeGiris = santiyeKasa.SistemeGiris,
                SonGuncelleme = santiyeKasa.SonGuncelleme,
                SantiyeGiderKalemiId = santiyeKasa.SantiyeGiderKalemiId,
                SantiyeGiderKalemi = santiyeKasa.SantiyeGiderKalemi,
                SantiyeId = santiyeKasa.SantiyeId,
                Santiye = santiyeKasa.Santiye,
            };

            return View(_santiyeKasa);
        }
        [HttpGet]
        public IActionResult SantiyeKasaSil(int? santiyekasaid)
        {
            ViewBag.Sayfa = "FATURAYI SİL";
            ViewBag.GK = _santiyeGiderKalemiService.GetAll(true, true);

            if (santiyekasaid == null){return NotFound();}

            ESantiyeKasa santiyeKasa = _santiyeKasaService.GetById((int)santiyekasaid);
            ViewBag.SantiyeId = santiyeKasa.SantiyeId;

            if (santiyeKasa == null){return NotFound();}

            return View(santiyeKasa);
        }
        [HttpPost]
        public IActionResult SantiyeKasaSil(ESantiyeKasa s)
        {
            var entity = _santiyeKasaService.GetById(s.Id);

            if (entity == null){return NotFound();}

            entity.Durum = false;

            entity.SonGuncelleme = DateTime.Today;

            _santiyeKasaService.Update(entity);

            return RedirectToAction("SantiyeKasa", new { santiyeid = entity.SantiyeId });
        }

        //EXCEL
        public IActionResult SantiyeKasaExcel(int santiyeid, int? gkid)
        {

            var santiyeKasaViewModel = new SantiyeKasaViewListModel()
            {
                SantiyeKasas = _santiyeKasaService.GetAll((int)santiyeid, (int?)gkid, true),
                SantiyeGiderKalemis = _santiyeGiderKalemiService.GetAll(true, true),
                Santiye = _santiyeService.GetById(santiyeid)
            };

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("ŞANTİYE KASA");
                var currentRow = 1;
                decimal? toplamgelir = 0;
                decimal? toplamgider = 0;
                decimal? toplam = 0;

                #region Header
                worksheet.Cell(currentRow, 1).Value = "ŞANTİYE";
                worksheet.Cell(currentRow, 2).Value = "KALEM";
                worksheet.Cell(currentRow, 3).Value = "TARİH";
                worksheet.Cell(currentRow, 4).Value = "AÇIKLAMA";
                worksheet.Cell(currentRow, 5).Value = "KİŞİ";
                worksheet.Cell(currentRow, 6).Value = "NO";
                worksheet.Cell(currentRow, 7).Value = "GELİR";
                worksheet.Cell(currentRow, 8).Value = "GİDER";

                for (int i = 1; i < 9; i++)
                {
                    worksheet.Cell(currentRow, i).Style.Font.Bold = true;
                    worksheet.Cell(currentRow, i).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                }
                #endregion

                #region Body
                foreach (var santiyekasa in santiyeKasaViewModel.SantiyeKasas)
                {
                    currentRow++;
                    worksheet.Cell(currentRow, 1).Value = santiyekasa.Santiye.Ad;
                    worksheet.Cell(currentRow, 2).Value = santiyekasa.SantiyeGiderKalemi.Ad;
                    worksheet.Cell(currentRow, 3).Value = santiyekasa.Tarih;
                    worksheet.Cell(currentRow, 4).Value = santiyekasa.Aciklama;
                    worksheet.Cell(currentRow, 5).Value = santiyekasa.Kisi;
                    worksheet.Cell(currentRow, 6).Value = santiyekasa.No;
                    worksheet.Cell(currentRow, 7).Value = santiyekasa.Gelir;
                    worksheet.Cell(currentRow, 8).Value = santiyekasa.Gider;

                    if (santiyekasa.Gelir != 0)
                    {
                        toplamgelir = toplamgelir + santiyekasa.Gelir;
                    }

                    if (santiyekasa.Gider != 0)
                    {
                        toplamgider = toplamgider + santiyekasa.Gider;
                    }

                    toplam = santiyekasa.Gelir + santiyekasa.Gider;
                }

                for (int i = 1; i < 9; i++)
                {
                    worksheet.Cell(currentRow, i).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                }
                #endregion

                #region FOOTER
                worksheet.Cell(currentRow + 2, 8).Value = "TOPLAM GİDER";
                worksheet.Cell(currentRow + 2, 9).Value = toplamgider;

                worksheet.Cell(currentRow + 3, 8).Value = "TOPLAM GELİR";
                worksheet.Cell(currentRow + 3, 9).Value = toplamgelir;

                worksheet.Cell(currentRow + 4, 8).Value = "NET";
                worksheet.Cell(currentRow + 4, 9).Value = toplam;

                for (int i = 8; i < 10; i++)
                {
                    for (int j = 2; j < 5; j++)
                    {
                        worksheet.Cell(currentRow + j, i).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
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
                        "SantiyeKasa.xlsx"
                        );
                }
            }
        }
        //GERİ YÜKLEME
        [HttpGet]
        public IActionResult SantiyeKasaGeriYukle(int? santiyekasaid)
        {

            if (santiyekasaid == null)
            {
                return NotFound();
            }

            ESantiyeKasa santiyeKasa = _santiyeKasaService.GetById((int)santiyekasaid);

            if (santiyeKasa == null)
            {
                return NotFound();
            }

            santiyeKasa.Durum = false;
            santiyeKasa.SonGuncelleme = DateTime.Today;

            _santiyeKasaService.Update(santiyeKasa);

            return RedirectToAction("SantiyeKasa", new { santiyeid = santiyeKasa.SantiyeId });
        }
        #region ŞANTİYEDEN
        [HttpGet]
        public IActionResult SantiyeKasaEklemeFromSantiye(int santiyeid)
        {
            ViewBag.Sayfa = _santiyeService.GetById(santiyeid).Ad + " ŞANTİYE KASASI GİDER EKLE";
            ViewBag.GK = _santiyeGiderKalemiService.GetAll(true, true);

            ViewBag.SantiyeId = santiyeid;
            return View(new SantiyeKasa());
        }
        [HttpPost]
        public async Task<IActionResult> SantiyeKasaEklemeFromSantiye(SantiyeKasa santiyeKasa, IFormFile file)
        {
            #region EĞER RESİM EKLİ DEĞİLSE GERİ DÖNÜŞTE GEREKLİ BİLGİLER
            ViewBag.Sayfa = _santiyeService.GetById(santiyeKasa.SantiyeId).Ad + " ŞANTİYE KASASI GİDER EKLE";
            ViewBag.GK = _santiyeGiderKalemiService.GetAll(true, true);
            ViewBag.SantiyeId = santiyeKasa.SantiyeId;
            #endregion

            ESantiyeKasa _santiyeKasa = new ESantiyeKasa()
            {
                Tarih = santiyeKasa.Tarih,
                Aciklama = santiyeKasa.Aciklama,
                Kisi = santiyeKasa.Kisi,
                No = santiyeKasa.No,

                #region VİRGÜL VEYA NOKTA KULLANIMININ İKİSİNİN DE SERBEST OLMASINI SAĞLAMAK İÇİN
                Gelir = Convert.ToDecimal(santiyeKasa.Gelir.Replace(".", ",")),
                Gider = Convert.ToDecimal(santiyeKasa.Gider.Replace(".", ",")),
                #endregion

                ImgUrl = santiyeKasa.ImgUrl,
                Durum = santiyeKasa.Durum,
                BankaKasaKaynak = santiyeKasa.BankaKasaKaynak,
                SistemeGiris = santiyeKasa.SistemeGiris,
                SonGuncelleme = santiyeKasa.SonGuncelleme,
                SantiyeGiderKalemiId = santiyeKasa.SantiyeGiderKalemiId,
                SantiyeGiderKalemi = santiyeKasa.SantiyeGiderKalemi,
                SantiyeId = santiyeKasa.SantiyeId,
                Santiye = santiyeKasa.Santiye,
            };

            #region RESİM EKLEME BÖLÜMÜ

            if (file != null)
            {
                var extension = Path.GetExtension(file.FileName);

                if (extension == ".jpg" || extension == ".png" || extension == ".pdf")
                {
                    var picName = string.Format($"{santiyeKasa.Aciklama}{"-"}{Guid.NewGuid()}{extension}");
                    santiyeKasa.ImgUrl = picName;
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\SantiyeKasaResim", picName);

                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }
                }
                else { return View(santiyeKasa); }
            }
            //else { return View(santiyeKasa); }
            //FATURA EKLENMESE BİLE SİSTEME FATURA GİRİLEBİLSİN DİYE ELSE KISMINI ÇIKARDIM
            #endregion

            _santiyeKasaService.Create(_santiyeKasa);

            return RedirectToAction("SantiyeKasa", new { santiyeid = santiyeKasa.SantiyeId });
        }

        [HttpGet]
        public IActionResult SantiyeKasaGuncelleFromSantiye(int? santiyekasaid)
        {

            ViewBag.Sayfa = "FATURA BİLGİLERİNİ GÜNCELLE";
            ViewBag.GK = _santiyeGiderKalemiService.GetAll(true, true);

            if (santiyekasaid == null){return NotFound();}

            ESantiyeKasa santiyeKasa = _santiyeKasaService.GetById((int)santiyekasaid);
            ViewBag.SantiyeId = santiyeKasa.SantiyeId;

            if (santiyeKasa == null){return NotFound();}

            SantiyeKasa _santiyeKasa = new SantiyeKasa()
            {
                Id = santiyeKasa.Id,
                Tarih = santiyeKasa.Tarih,
                Aciklama = santiyeKasa.Aciklama,
                Kisi = santiyeKasa.Kisi,
                No = santiyeKasa.No,

                Gelir = Convert.ToString(santiyeKasa.Gelir),
                Gider = Convert.ToString(santiyeKasa.Gider),

                ImgUrl = santiyeKasa.ImgUrl,
                Durum = santiyeKasa.Durum,
                BankaKasaKaynak = santiyeKasa.BankaKasaKaynak,
                SistemeGiris = santiyeKasa.SistemeGiris,
                SonGuncelleme = santiyeKasa.SonGuncelleme,
                SantiyeGiderKalemiId = santiyeKasa.SantiyeGiderKalemiId,
                SantiyeGiderKalemi = santiyeKasa.SantiyeGiderKalemi,
                SantiyeId = santiyeKasa.SantiyeId,
                Santiye = santiyeKasa.Santiye,
            };

            return View(_santiyeKasa);
        }
        [HttpPost]
        public IActionResult SantiyeKasaGuncelleFromSantiye(ESantiyeKasa s)
        {
            var entitySantiyeKasa = _santiyeKasaService.GetById(s.Id);

            if (entitySantiyeKasa == null)
            {
                return NotFound();
            }

            entitySantiyeKasa.Tarih = s.Tarih;
            entitySantiyeKasa.Aciklama = s.Aciklama;
            entitySantiyeKasa.Kisi = s.Kisi;
            entitySantiyeKasa.No = s.No;
            entitySantiyeKasa.Gider = s.Gider;
            entitySantiyeKasa.ImgUrl = s.ImgUrl;
            entitySantiyeKasa.SantiyeGiderKalemiId = s.SantiyeGiderKalemiId;
            entitySantiyeKasa.SonGuncelleme = DateTime.Today;

            _santiyeKasaService.Update(entitySantiyeKasa);

            return RedirectToAction("SantiyeKasa", new { santiyeid = s.SantiyeId });
        }
        #endregion
    }
}
