using Microsoft.AspNetCore.Mvc;
using SantiyeOnMuh.Business.Abstract;
using SantiyeOnMuh.WebUI.Models;

namespace SantiyeOnMuh.WebUI.Controllers
{
    public class AnaSayfaController : Controller
    {
        private ISantiyeService _santiyeService;
        private ISantiyeGiderKalemiService _santiyeGiderKalemiService;
        private ICariGiderKalemiService _cariGiderKalemiService;
        private IBankaHesapService _bankaHesapService;
        private IBankaKasaService _bankaKasaService;
        private ISantiyeKasaService _santiyeKasaService;
        private ICariKasaService _cariKasaService;
        public AnaSayfaController(
            ISantiyeService santiyeService,
            ISantiyeGiderKalemiService santiyeGiderKalemiService,
            ICariGiderKalemiService cariGiderKalemiService,
            IBankaHesapService bankaHesapService,
            IBankaKasaService bankaKasaService,
            ISantiyeKasaService santiyeKasaService,
            ICariKasaService cariKasaService)
        {
            this._santiyeService = santiyeService;
            this._santiyeGiderKalemiService = santiyeGiderKalemiService;
            this._cariGiderKalemiService = cariGiderKalemiService;
            this._bankaHesapService = bankaHesapService;
            this._bankaKasaService = bankaKasaService;
            this._santiyeKasaService = santiyeKasaService;
            this._cariKasaService = cariKasaService;
        }
        public IActionResult Index()
        {

            var anaSayfaViewModel = new AnaSayfaViewListModel()
            {
                Santiyes = _santiyeService.GetAll(true),
                SantiyeKasas = _santiyeKasaService.GetAll(null, null, true),
                //SantiyeGiderKalemi = _santiyeGiderKalemiService.GetAll(),
                //BankaHesaps = _bankaHesapService.GetAllDetay(true),
                //BankaKasas = _bankaKasaService.GetAllDetayBankaHesap(null, true),
            };

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

            return View(anaSayfaViewModel);

        }
    }
}