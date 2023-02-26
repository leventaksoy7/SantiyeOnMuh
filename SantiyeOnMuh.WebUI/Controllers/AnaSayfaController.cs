using Microsoft.AspNetCore.Mvc;
using SantiyeOnMuh.Business.Abstract;

namespace SantiyeOnMuh.WebUI.Controllers
{
    public class AnaSayfaController : Controller
    {
        public IActionResult Index()
        {
            //var anaSayfaViewModel = new AnaSayfaViewListModel()
            //{
            //    Santiyes = _santiyeService.GetAllDetay(true),
            //    SantiyeKasas = _santiyeKasaService.GetAllDetaySantiyeGK(null, null, true),
            //    SantiyeKasaGKs = _santiyeKasaGKService.GetAll(),
            //    BankaHesaps = _bankaHesapService.GetAllDetay(true),
            //    BankaKasas = _bankaKasaService.GetAllDetayBankaHesap(null, true),
            //};

            ////ŞANTİYELER VE ŞANTİYE KASASI GİDER KALEMLERİ
            //var aktifsantiyesayisi = anaSayfaViewModel.Santiyes.Count();
            //var aktifgidersayisi = anaSayfaViewModel.SantiyeKasaGKs.Count();

            //decimal?[] kalemtoplami = new decimal?[aktifgidersayisi * aktifsantiyesayisi];

            //int sayi = 0;
            //foreach (var santiye in anaSayfaViewModel.Santiyes)
            //{
            //    foreach (var giderkalemi in anaSayfaViewModel.SantiyeKasaGKs)
            //    {
            //        kalemtoplami[sayi] =
            //            (decimal?)_santiyeKasaService.GetAllDetaySantiyeGK((int)santiye.Id, (int)giderkalemi.Id, true).Sum(i => i.Gider)
            //            +
            //            (decimal?)_santiyeKasaService.GetAllDetaySantiyeGK((int)santiye.Id, (int)giderkalemi.Id, true).Sum(i => i.Gelir);

            //        sayi = sayi + 1;
            //    }
            //}
            //ViewBag.gider = kalemtoplami;

            ////BANKA HESAPLARI VE BANKA BAKİYELERİ
            //var aktifbankahesabisayisi = anaSayfaViewModel.BankaHesaps.Count();

            //decimal?[] bankahesabigiren = new decimal?[aktifbankahesabisayisi];
            //decimal?[] bankahesabicikan = new decimal?[aktifbankahesabisayisi];
            //decimal?[] bankahesabinet = new decimal?[aktifbankahesabisayisi];

            //int sayi2 = 0;
            //foreach (var bankahesabi in anaSayfaViewModel.BankaHesaps)
            //{
            //    bankahesabigiren[sayi2] = (decimal?)_bankaKasaService.GetAllDetayBankaHesap((int)bankahesabi.Id, true).Sum(i => i.Giren);

            //    bankahesabicikan[sayi2] = (decimal?)_bankaKasaService.GetAllDetayBankaHesap((int)bankahesabi.Id, true).Sum(i => i.Cikan);

            //    bankahesabinet[sayi2] = bankahesabigiren[sayi2] - bankahesabicikan[sayi2];

            //    sayi2 = sayi2 + 1;
            //}
            //ViewBag.net = bankahesabinet;

            //ViewBag.Sayfa = "ANA SAYFA";

            //return View(anaSayfaViewModel);

            return View();
        }
    }
}
