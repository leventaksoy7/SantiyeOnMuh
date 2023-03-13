using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SantiyeOnMuh.Business.Abstract;
using SantiyeOnMuh.Entity;
using SantiyeOnMuh.WebUI.Models;
using SantiyeOnMuh.WebUI.Models.Modeller;

namespace SantiyeOnMuh.WebUI.Controllers
{
    public class CariHesapController : Controller
    {
        // _ olan nesnemizle artık işlemlerimizi gerçekleştireceğiz
        private ICariHesapService _cariHesapService;
        private ISantiyeService _santiyeService;
        public CariHesapController(
            ICariHesapService cariHesapService,
            ISantiyeService santiyeService)
        {
            this._cariHesapService = cariHesapService;
            this._santiyeService = santiyeService;
        }

        public IActionResult CariHesap(int? santiyeid, int page = 1)
        {
            const int pageSize = 10;

            var cariHesapViewModel = new CariHesapViewListModel()
            {
                PageInfo = new PageInfo
                {
                    TotalItem = _cariHesapService.GetCount(santiyeid, true),
                    CurrentPage = page,
                    ItemPerPage = pageSize,
                    UrlInfo = (int?)santiyeid
                },

                Santiyes = _santiyeService.GetAll(true),
                CariHesaps = _cariHesapService.GetAll(santiyeid, true, page, pageSize),
            };

            //BAŞLIKTA ŞANTİYENİN ADININ YAZMASI İÇİN
            if (santiyeid != null)
            {
                var santiye = _santiyeService.GetById((int)santiyeid);
                ViewBag.Sayfa = santiye.Ad + " CARİ HESAPLARI";
            }else{
                ViewBag.Sayfa = "CARİ HESAPLAR";
            };

            return View(cariHesapViewModel);
        }

        [HttpGet]
        public IActionResult CariHesapEkleme()
        {
            ViewBag.Sayfa = "YENİ CARİ HESAP AÇMA";
            ViewBag.Santiye = _santiyeService.GetAll(true);
            return View();
        }

        [HttpPost]
        public IActionResult CariHesapEkleme(CariHesap cariHesap)
        {
            if (!ModelState.IsValid) 
            {
                ViewBag.Sayfa = "YENİ CARİ HESAP AÇMA";
                ViewBag.Santiye = _santiyeService.GetAll(true);
                return View(cariHesap);
            }

            ECariHesap _cariHesap = new ECariHesap()
            {
                Ad = cariHesap.Ad,
                Adres = cariHesap.Adres,
                Telefon = cariHesap.Telefon,
                VergiNo = cariHesap.VergiNo,
                IlgiliKisi = cariHesap.IlgiliKisi,
                IlgiliKisiTelefon = cariHesap.IlgiliKisiTelefon,
                Odeme = cariHesap.Odeme,
                Vade = cariHesap.Vade,
                Durum = cariHesap.Durum,
                Ceks = cariHesap.Ceks,
                Nakits = cariHesap.Nakits,
                CariKasas = cariHesap.CariKasas,
                SantiyeId = cariHesap.SantiyeId,
                Santiye = cariHesap.Santiye,
            };

            if (_cariHesapService.Create(_cariHesap))
            {
                AlertMessage msg = new AlertMessage()
                {
                    Message = $"{_cariHesap.Ad} HESABI AÇILDI.",
                    AlertType = "success"
                };

                TempData["message"] = JsonConvert.SerializeObject(msg);

                return RedirectToAction("CariHesap");
            };
            return View(cariHesap);

            //_cariHesapService.Create(_cariHesap);

            //return RedirectToAction("CariHesap");
        }

        [HttpGet]
        public IActionResult CariHesapGuncelle(int? carihesapid)
        {
            ViewBag.Sayfa = "CARİ HESAP BİLGİLERİNİ GÜNCELLEME";
            ViewBag.Santiye = _santiyeService.GetAll(true);

            if (carihesapid == null){return NotFound();}

            ECariHesap cariHesap = _cariHesapService.GetById((int)carihesapid);

            if (cariHesap == null){return NotFound();}

            CariHesap _cariHesap = new CariHesap()
            {
                Id = cariHesap.Id,
                Ad = cariHesap.Ad,
                Adres = cariHesap.Adres,
                Telefon = cariHesap.Telefon,
                VergiNo = cariHesap.VergiNo,
                IlgiliKisi = cariHesap.IlgiliKisi,
                IlgiliKisiTelefon = cariHesap.IlgiliKisiTelefon,
                Odeme = cariHesap.Odeme,
                Vade = cariHesap.Vade,
                Durum = cariHesap.Durum,
                Ceks = cariHesap.Ceks,
                Nakits = cariHesap.Nakits,
                CariKasas = cariHesap.CariKasas,
                SantiyeId = cariHesap.SantiyeId,
                Santiye = cariHesap.Santiye,
            };

            return View(_cariHesap);
        }

        [HttpPost]
        public IActionResult CariHesapGuncelle(CariHesap cariHesap)
        {
            ViewBag.Sayfa = "CARİ HESAP BİLGİLERİNİ GÜNCELLEME";
            ViewBag.Santiye = _santiyeService.GetAll(true);

            if (!ModelState.IsValid) { return View(cariHesap); }

            var _cariHesap = _cariHesapService.GetById(cariHesap.Id);

            if (_cariHesap == null){return NotFound();}

            _cariHesap.Ad = cariHesap.Ad;
            _cariHesap.Adres = cariHesap.Adres;
            _cariHesap.Telefon = cariHesap.Telefon;
            _cariHesap.VergiNo = cariHesap.VergiNo;
            _cariHesap.IlgiliKisi = cariHesap.IlgiliKisi;
            _cariHesap.IlgiliKisiTelefon = cariHesap.IlgiliKisiTelefon;
            _cariHesap.Odeme = cariHesap.Odeme;
            _cariHesap.Vade = cariHesap.Vade;
            _cariHesap.SantiyeId = cariHesap.SantiyeId;

            _cariHesapService.Update(_cariHesap);

            return RedirectToAction("CariHesap");
        }

        [HttpGet]
        public IActionResult CariHesapSil(int? carihesapid)
        {
            ViewBag.Sayfa = "CARİ HESABI SİL";

            if (carihesapid == null){return NotFound();}

            ECariHesap cariHesap = _cariHesapService.GetById((int)carihesapid);

            if (cariHesap == null){return NotFound();}

            CariHesap _cariHesap = new CariHesap()
            {
                Id=cariHesap.Id,
                Ad = cariHesap.Ad,
                Adres = cariHesap.Adres,
                Telefon = cariHesap.Telefon,
                VergiNo = cariHesap.VergiNo,
                IlgiliKisi = cariHesap.IlgiliKisi,
                IlgiliKisiTelefon = cariHesap.IlgiliKisiTelefon,
                Odeme = cariHesap.Odeme,
                Vade = cariHesap.Vade,
                Durum = cariHesap.Durum,
                Ceks = cariHesap.Ceks,
                Nakits = cariHesap.Nakits,
                CariKasas = cariHesap.CariKasas,
                SantiyeId = cariHesap.SantiyeId,
                Santiye = cariHesap.Santiye,
            };

            return View(_cariHesap);
        }

        [HttpPost]
        public IActionResult CariHesapSil(CariHesap c)
        {
            var entity = _cariHesapService.GetById(c.Id);

            if (entity == null) { return NotFound(); }

            entity.Durum = false;

            _cariHesapService.Update(entity);
            return RedirectToAction("CariHesap");
        }

        //EXCEL
        public IActionResult CariHesapExcel(int? santiyeid, int page = 1)
        {

            const int pageSize = 10;

            var cariHesapViewModel = new CariHesapViewListModel()
            {
                PageInfo = new PageInfo
                {
                    TotalItem = _cariHesapService.GetCount(santiyeid, true),
                    CurrentPage = page,
                    ItemPerPage = pageSize,
                    UrlInfo = (int?)santiyeid
                },

                Santiyes = _santiyeService.GetAll(true),
                CariHesaps = _cariHesapService.GetAll(santiyeid, true),
            };

            using (var workbook = new XLWorkbook())
            {

                var worksheet = workbook.Worksheets.Add("CARİ HESAPLAR");
                var currentRow = 1;

                #region Header
                worksheet.Cell(currentRow, 1).Value = "FİRMA ADI";
                worksheet.Cell(currentRow, 2).Value = "ADRES";
                worksheet.Cell(currentRow, 3).Value = "TELEFON";
                worksheet.Cell(currentRow, 4).Value = "VERGİ NO";
                worksheet.Cell(currentRow, 5).Value = "İLGİLİ KİŞİ";
                worksheet.Cell(currentRow, 6).Value = "TELEFON";
                worksheet.Cell(currentRow, 7).Value = "ÖDEME";
                worksheet.Cell(currentRow, 8).Value = "VADE";
                #endregion

                #region Body
                foreach (var carihesap in cariHesapViewModel.CariHesaps)
                {
                    currentRow++;
                    worksheet.Cell(currentRow, 1).Value = carihesap.Ad;
                    worksheet.Cell(currentRow, 2).Value = carihesap.Adres;
                    worksheet.Cell(currentRow, 3).Value = carihesap.Telefon;
                    worksheet.Cell(currentRow, 4).Value = carihesap.VergiNo;
                    worksheet.Cell(currentRow, 5).Value = carihesap.IlgiliKisi;
                    worksheet.Cell(currentRow, 6).Value = carihesap.IlgiliKisiTelefon;
                    worksheet.Cell(currentRow, 7).Value = carihesap.Odeme;
                    worksheet.Cell(currentRow, 8).Value = carihesap.Vade;
                }
                #endregion

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();

                    return File(
                        content,
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        "CariHesap.xlsx"
                        );
                }
            }
        }

        //ARŞİV
        public IActionResult CariHesapArsiv(int? santiyeid, int page = 1)
        {
            const int pageSize = 10;

            var cariHesapViewModel = new CariHesapViewListModel()
            {
                PageInfo = new PageInfo
                {
                    TotalItem = _cariHesapService.GetCount(santiyeid, false),
                    CurrentPage = page,
                    ItemPerPage = pageSize,
                    UrlInfo = (int?)santiyeid
                },
                Santiyes = _santiyeService.GetAll(),
                CariHesaps = _cariHesapService.GetAll(santiyeid, false, page, pageSize),
            };

            //BAŞLIKTA ŞANTİYENİN ADININ YAZMASI İÇİN
            if (santiyeid != null)
            {
                var santiye = _santiyeService.GetById((int)santiyeid);
                ViewBag.Sayfa = santiye.Ad + " CARİ HESAPLARI";
            }
            else
            {
                ViewBag.Sayfa = "CARİ HESAPLAR";

            };

            return View(cariHesapViewModel);
        }

        //GERİ YÜKLEME
        [HttpGet]
        public IActionResult CariHesapGeriYukle(int? carihesapid)
        {
            if (carihesapid == null)
            {
                return NotFound();
            }

            ECariHesap cariHesap = _cariHesapService.GetById((int)carihesapid);

            if (cariHesap == null)
            {
                return NotFound();
            }

            cariHesap.Durum = true;

            _cariHesapService.Update(cariHesap);
            return RedirectToAction("CariHesap");
        }
    }
}
