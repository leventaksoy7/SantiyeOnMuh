using ClosedXML.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
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
    
    public class SantiyeKasaController : Controller
    {
        // NESNELER ÜZERİNDEKİ İŞLEMLERİ _ OLAN NESNE ÜZERİNDE YAPIP SONRA AKTARIYORUZ - INJECTION
        private ISantiyeKasaService _santiyeKasaService;
        private ISantiyeGiderKalemiService _santiyeGiderKalemiService;
        private ISantiyeService _santiyeService;
        private UserManager<User> _userManager;

        public SantiyeKasaController(
            UserManager<User> userManager,
            ISantiyeKasaService santiyeKasaService,
            ISantiyeGiderKalemiService santiyeGiderKalemiService,
            ISantiyeService santiyeService)
        {
            this._userManager = userManager;
            this._santiyeKasaService = santiyeKasaService;
            this._santiyeGiderKalemiService = santiyeGiderKalemiService;
            this._santiyeService = santiyeService;
        }

        [Authorize(Roles = "Santiye")]
        public async Task<IActionResult> SantiyeKasaYonlendirme()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            return RedirectToAction("SantiyeKasa", "SantiyeKasa", new { @santiyeid = user.SantiyeId });
        }
            [Authorize(Roles = "Admin,Ofis,Santiye")]
        public async Task<IActionResult> SantiyeKasa(int santiyeid, int? gkid, int page = 1)
        {
            if (await SantiyeKontrol(santiyeid)) { }
            else
            {
                return RedirectToAction("LogOut", "Account");
            }

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
                SantiyeGiderKalemis = _santiyeGiderKalemiService.GetAll(true),
                //SantiyeGiderKalemis = _santiyeGiderKalemiService.GetAll(true, true),
                Santiye = _santiyeService.GetById(santiyeid)
            };

            //gider kalemi olup olmadığını kontrol ediyoruz, ona göre alt taraftaki toplamın şekli değişecek
            ViewBag.gk = gkid;
            //
            ViewBag.toplamgider = _santiyeKasaService.GetAll((int)santiyeid, (int?)gkid, true).Sum(i => i.Gider);
            ViewBag.toplamgelir = _santiyeKasaService.GetAll((int)santiyeid, (int?)gkid, true).Sum(i => i.Gelir);

            return View(santiyeKasaViewModel);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult SantiyeKasaArsiv(int santiyeid, int? gkid, int page = 1)
        {
            //BAŞLIKTA ŞANTİYENİN ADININ YAZMASI İÇİN
            ViewBag.Sayfa = _santiyeService.GetById(santiyeid).Ad + " ŞANTİYE KASASI - SİLİNMİŞ VERİLER ";

            const int pageSize = 10;
            var santiyeKasaViewModel = new SantiyeKasaViewListModel()
            {
                PageInfo = new PageInfo
                {
                    TotalItem = _santiyeKasaService.GetCount((int)santiyeid, (int?)gkid, false),
                    CurrentPage = page,
                    ItemPerPage = pageSize,
                    UrlInfo = (int?)santiyeid
                },

                SantiyeKasas = _santiyeKasaService.GetAll((int)santiyeid, (int?)gkid, false, page, pageSize),
                SantiyeGiderKalemis = _santiyeGiderKalemiService.GetAll(true, true),
                Santiye = _santiyeService.GetById(santiyeid)
            };

            //gider kalemi olup olmadığını kontrol ediyoruz, ona göre alt taraftaki toplamın şekli değişecek
            ViewBag.gk = gkid;
            //
            ViewBag.toplamgider = _santiyeKasaService.GetAll((int)santiyeid, (int?)gkid, false).Sum(i => i.Gider);
            ViewBag.toplamgelir = _santiyeKasaService.GetAll((int)santiyeid, (int?)gkid, false).Sum(i => i.Gelir);

            return View(santiyeKasaViewModel);
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Ofis,Santiye")]
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
        [Authorize(Roles = "Admin,Ofis")]
        public IActionResult SantiyeKasaSil(int? santiyekasaid)
        {
            ViewBag.Sayfa = "FATURAYI SİL";
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
        [Authorize(Roles = "Admin,Ofis")]
        public IActionResult SantiyeKasaSil(SantiyeKasa santiyeKasa)
        {

            var entity = _santiyeKasaService.GetById(santiyeKasa.Id);

            if (entity == null){return NotFound();}

            entity.Durum = false;

            entity.SonGuncelleme = DateTime.Today;

            _santiyeKasaService.Update(entity);

            TempData.Put("message", new AlertMessage()
            {
                Title = "BAŞARILI",
                AlertType = "danger",
                Message = $"{santiyeKasa.Aciklama} SİLİNDİ."
            });

            return RedirectToAction("SantiyeKasa", new { santiyeid = entity.SantiyeId });
        }

        [Authorize(Roles = "Admin,Ofis,Santiye")]
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
        [Authorize(Roles = "Admin")]
        public IActionResult SantiyeKasaGeriYukle(int id)
        {
            ESantiyeKasa santiyeKasa = _santiyeKasaService.GetById(id);

            if (santiyeKasa == null){return NotFound();}

            santiyeKasa.Durum = true;
            santiyeKasa.SonGuncelleme = DateTime.Today;

            _santiyeKasaService.Update(santiyeKasa);

            TempData.Put("message", new AlertMessage()
            {
                Title = "BAŞARILI",
                AlertType = "success",
                Message = $"{santiyeKasa.Aciklama} GERİ EKLENDİ."
            });

            return RedirectToAction("SantiyeKasa", new { santiyeid = santiyeKasa.SantiyeId });
        }

        #region ŞANTİYEDEN
        [Authorize(Roles = "Admin,Ofis,Santiye")]
        [HttpGet]
        public IActionResult SantiyeKasaEklemeFromSantiye(int santiyeid)
        {
            ViewBag.Sayfa = _santiyeService.GetById(santiyeid).Ad + " ŞANTİYE KASASI GİDER EKLE";
            ViewBag.GK = _santiyeGiderKalemiService.GetAll(true);
            //ViewBag.GK = _santiyeGiderKalemiService.GetAll(true, true);
            ViewBag.SantiyeId = santiyeid;

            return View(new SantiyeKasa());
        }

        [Authorize(Roles = "Admin,Ofis,Santiye")]
        [HttpPost]
        public async Task<IActionResult> SantiyeKasaEklemeFromSantiye(SantiyeKasa santiyeKasa, IFormFile? file)
        {
            if (await SantiyeKontrol(santiyeKasa.SantiyeId)) { }
            else
            {
                return RedirectToAction("LogOut", "Account");
            }

            #region EĞER RESİM EKLİ DEĞİLSE GERİ DÖNÜŞTE GEREKLİ BİLGİLER
            ViewBag.Sayfa = _santiyeService.GetById(santiyeKasa.SantiyeId).Ad + " ŞANTİYE KASASI GİDER EKLE";
            ViewBag.GK = _santiyeGiderKalemiService.GetAll(true, true);
            ViewBag.SantiyeId = santiyeKasa.SantiyeId;
            #endregion

            if (!ModelState.IsValid) { return View(santiyeKasa); }

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

            if (_santiyeKasaService.Create(_santiyeKasa))
            {
                TempData.Put("message", new AlertMessage()
                {
                    Title = "BAŞARILI",
                    AlertType = "success",
                    Message = $"{santiyeKasa.Aciklama} EKLENDİ."
                });

                return RedirectToAction("SantiyeKasa", new { santiyeid = santiyeKasa.SantiyeId });
            };

            TempData.Put("message", new AlertMessage()
            {
                Title = "BAŞARILI",
                AlertType = "danger",
                Message = _santiyeKasaService.ErrorMessage
            });

            return View(santiyeKasa);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
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
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> SantiyeKasaGuncelleFromSantiye(SantiyeKasa s, IFormFile? file)
        {
            if (await SantiyeKontrol(s.SantiyeId)) { }
            else
            {
                return RedirectToAction("LogOut", "Account");
            }

            ViewBag.Sayfa = "FATURA BİLGİLERİNİ GÜNCELLE";
            ViewBag.GK = _santiyeGiderKalemiService.GetAll(true, true);
            ViewBag.SantiyeId = s.SantiyeId;

            //#region
            ///*GÜNCELLEME EKRARNINDA MODELDEN GELEN VERİYİ İNPUT İÇERİSİNE BİR TÜRLÜ YAZDIRAMADIM
            // * BENDE İNPUT İÇERİSİNE VERİNİN SANAL GÖRÜNTÜSÜNÜ KOYDUM (placeholder)
            // * BU GÖRÜNTÜYÜ NERNEYE ATMIYOR, İLLA Kİ ELLE DEĞER GİRMEK GEREKİYOR
            // * GÜNCELLEMEDE HER SEFERİNDE TEKRAR TEKRAR İNPUTA ESKİ VERİYİ YAZDIRMAK YERİNE
            // * YAZILMADAN GÜNCELLEME YAPILDIĞI TAKDİRDE
            // * DATABASE'DEN DEĞİŞMEMİŞ VERİYİ İÇERİSİNE KOYUYOR, SİSTEM VALİD TRUE OLUYOR
            //*/
            //if (s.Gider == null){
            //    ESantiyeKasa _santiyeKasa = _santiyeKasaService.GetById(s.Id);
            //    s.Gider = Convert.ToString(_santiyeKasa.Gider);
            //}
            //#endregion
            if (!ModelState.IsValid) { return View(s); }

            var entitySantiyeKasa = _santiyeKasaService.GetById(s.Id);

            if (entitySantiyeKasa == null) { return NotFound(); }

            if (file != null)
            {
                var extension = Path.GetExtension(file.FileName);

                if (extension == ".jpg" || extension == ".png" || extension == ".pdf")
                {
                    var picName = string.Format($"{s.Aciklama}{"-"}{Guid.NewGuid()}{extension}");
                    s.ImgUrl = picName;
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\SantiyeKasaResim", picName);

                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }
                }
                else { return View(s); }
            }

            entitySantiyeKasa.Tarih = s.Tarih;
            entitySantiyeKasa.Aciklama = s.Aciklama;
            entitySantiyeKasa.Kisi = s.Kisi;
            entitySantiyeKasa.No = s.No;
            entitySantiyeKasa.Gider = Convert.ToDecimal(s.Gider.Replace(".",","));
            entitySantiyeKasa.ImgUrl = s.ImgUrl;
            entitySantiyeKasa.SantiyeGiderKalemiId = s.SantiyeGiderKalemiId;
            entitySantiyeKasa.SonGuncelleme = DateTime.Today;

            _santiyeKasaService.Update(entitySantiyeKasa);

            TempData.Put("message", new AlertMessage()
            {
                Title = "BAŞARILI",
                AlertType = "success",
                Message = $"{entitySantiyeKasa.Aciklama} GÜNCELLENDİ."
            });

            return RedirectToAction("SantiyeKasa", new { santiyeid = s.SantiyeId });
        }

        //ÇALIŞIYOR - GEREKSİZLERİ KİCKLEMEK İÇİN
        public async Task<bool> SantiyeKontrol(int santiyeid)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);

            if (user == null) 
            {
                TempData.Put("message", new AlertMessage()
                {
                    Title = "HATA",
                    AlertType = "danger",
                    Message = "OTURUM KAPATILDI"
                });

                return false; 
            }
            else
            {
                if ((await _userManager.IsInRoleAsync(user, "Santiye")))
                {
                    if (user.SantiyeId != santiyeid)
                    {
                        TempData.Put("message", new AlertMessage()
                        {
                            Title = "HATA",
                            AlertType = "danger",
                            Message = "OTURUM KAPATILDI"
                        });
                        return false;
                    }
                    else
                    { return true; }
                }
                else
                { return true; }
            }
        }
        #endregion
    }
}
