using Microsoft.AspNetCore.Mvc;
using SantiyeOnMuh.Business.Abstract;
using SantiyeOnMuh.WebUI.Models;

namespace SantiyeOnMuh.WebUI.Controllers
{
    //[ValidateAntiForgeryToken]
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
        public IActionResult AnaSayfa()
        {
            var anaSayfaViewModel = new AnaSayfaViewListModel()
            {
                Santiyes = _santiyeService.GetAll(true),
                SantiyeKasas = _santiyeKasaService.GetAll(null, null, true),
                SantiyeGiderKalemis = _santiyeGiderKalemiService.GetAll(),
                BankaHesaps = _bankaHesapService.GetAll(true),
                BankaKasas = _bankaKasaService.GetAll(null, true),
            };

            //ŞANTİYELER VE ŞANTİYE KASASI GİDER KALEMLERİ
            var aktifsantiyesayisi = anaSayfaViewModel.Santiyes.Count();
            var aktifgidersayisi = anaSayfaViewModel.SantiyeGiderKalemis.Count();

            decimal?[] kalemtoplami = new decimal?[aktifgidersayisi * aktifsantiyesayisi];

            int sayi = 0;
            foreach (var santiye in anaSayfaViewModel.Santiyes)
            {
                foreach (var giderkalemi in anaSayfaViewModel.SantiyeGiderKalemis)
                {
                    kalemtoplami[sayi] =
                        (decimal?)_santiyeKasaService.GetAll((int)santiye.Id, (int)giderkalemi.Id, true).Sum(i => i.Gider)
                        +
                        (decimal?)_santiyeKasaService.GetAll((int)santiye.Id, (int)giderkalemi.Id, true).Sum(i => i.Gelir);

                    sayi ++;
                }
            }
            ViewBag.gider = kalemtoplami;

            //BANKA HESAPLARI VE BANKA BAKİYELERİ
            var aktifbankahesabisayisi = anaSayfaViewModel.BankaHesaps.Count();

            decimal?[] bankahesabigiren = new decimal?[aktifbankahesabisayisi];
            decimal?[] bankahesabicikan = new decimal?[aktifbankahesabisayisi];
            decimal?[] bankahesabinet = new decimal?[aktifbankahesabisayisi];

            int sayi2 = 0;
            foreach (var bankahesabi in anaSayfaViewModel.BankaHesaps)
            {
                bankahesabigiren[sayi2] = (decimal?)_bankaKasaService.GetAll((int)bankahesabi.Id, true).Sum(i => i.Giren);

                bankahesabicikan[sayi2] = (decimal?)_bankaKasaService.GetAll((int)bankahesabi.Id, true).Sum(i => i.Cikan);

                bankahesabinet[sayi2] = bankahesabigiren[sayi2] - bankahesabicikan[sayi2];

                sayi2 ++;
            }
            ViewBag.net = bankahesabinet;

            ViewBag.Sayfa = "ANA SAYFA";

            return View(anaSayfaViewModel);
        }
    }
}