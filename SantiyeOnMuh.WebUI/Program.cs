using SantiyeOnMuh.Business.Abstract;
using SantiyeOnMuh.Business.Concrete;
using SantiyeOnMuh.DataAccess.Abstract;
using SantiyeOnMuh.DataAccess.Concrete.EfCore;
using SantiyeOnMuh.DataAccess.Concrete.SqlServer;

var builder = WebApplication.CreateBuilder(args);


//--EF CORE 6'DA DEÐÝÞTÝRÝLMÝÞ - BÝTÝÞE KADAR MS SÝTEDEN COPY PASTE

// Add services to the container.
builder.Services.AddControllersWithViews();

//==>> ZATEN PROGRAM CS ÝÇERÝSÝNDE BUDASI TANIMLIYDI -var builder = WebApplication.CreateBuilder(args);-

// Add the memory cache services.
builder.Services.AddMemoryCache();

// Add a custom scoped service.
//SANTÝYE
builder.Services.AddScoped<ISantiyeService, SantiyeManager>();
builder.Services.AddScoped<ISantiyeRepository, EfCoreSantiyeRepository>();

builder.Services.AddScoped<ISantiyeKasaRepository, EfCoreSantiyeKasaRepository>();
builder.Services.AddScoped<ISantiyeKasaService, SantiyeKasaManager>();

builder.Services.AddScoped<ISantiyeGiderKalemiRepository, EfCoreSantiyeGiderKalemiRepository>();
builder.Services.AddScoped<ISantiyeGiderKalemiService, SantiyeGiderKalemiManager>();

//CARÝ
builder.Services.AddScoped<ICariHesapRepository, EfCoreCariHesapRepository>();
builder.Services.AddScoped<ICariHesapService, CariHesapManager>();

builder.Services.AddScoped<ICariKasaRepository, EfCoreCariKasaRepository>();
builder.Services.AddScoped<ICariKasaService, CariKasaManager>();

builder.Services.AddScoped<ICariGiderKalemiRepository, EfCoreCariGiderKalemiRepository>();
builder.Services.AddScoped<ICariGiderKalemiService, CariGiderKalemiManager>();

//BANKA
builder.Services.AddScoped<IBankaHesapRepository, EfCoreBankaHesapRepository>();
builder.Services.AddScoped<IBankaHesapService, BankaHesapManager>();

builder.Services.AddScoped<IBankaKasaRepository, EfCoreBankaKasaRepository>();
builder.Services.AddScoped<IBankaKasaService, BankaKasaManager>();

//ODEME
builder.Services.AddScoped<ICekRepository, EfCoreCekRepository>();
builder.Services.AddScoped<ICekService, CekManager>();

builder.Services.AddScoped<INakitRepository, EfCoreNakitRepository>();
builder.Services.AddScoped<INakitService, NakitManager>();

//SÝRKET
builder.Services.AddScoped<ISirketRepository, EfCoreSirketRepository>();
builder.Services.AddScoped<ISirketService, SirketManager>();

var app = builder.Build();

//--BÝTÝÞ

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

SeedDatabase.Seed();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

//app.MapControllerRoute(
//    name: "default",
//    pattern: "{controller=AnaSayfa}/{action=AnaSayfa}/{id?}");

//INDEX
//EKLE
//GUNCELLE
//DETAY
//SÝL
    app.MapControllerRoute(
        name: "default",
        pattern: "{controller=AnaSayfa}/{action=Anasayfa}/{id?}");
    #region ÞANTÝYE KASA
    //INDEX
    app.MapControllerRoute(
            name: "SantiyeKasa",
            pattern: "SantiyeKasa/SantiyeKasa/{santiyeid}/{gkid?}",
            defaults: new { controller = "SantiyeKasa", action = "SantiyeKasa" }
            );
        app.MapControllerRoute(
            name: "SantiyeKasaArsiv",
            pattern: "SantiyeKasa/SantiyeKasaArsiv/santiyeid/{gkid?}",
            defaults: new { controller = "SantiyeKasa", action = "SantiyeKasaArsiv" }
            );
        //EKLE
        app.MapControllerRoute(
            name: "SantiyeKasaEkleme",
            pattern: "SantiyeKasa/SantiyeKasaEkleme",
            defaults: new { controller = "SantiyeKasa", action = "SantiyeKasaEkleme" }
            );
        app.MapControllerRoute(
            name: "SantiyeKasaEklemeFromSantiye",
            pattern: "SantiyeKasa/SantiyeKasaFaturaEklemeFromSantiye/{santiyeid}",
            defaults: new { controller = "SantiyeKasa", action = "SantiyeKasaEklemeFromSantiye" }
            );
        //GUNCELLE
        app.MapControllerRoute(
            name: "SantiyeKasaGuncelleFromSantiye",
            pattern: "SantiyeKasa/SantiyeKasaGuncelleFromSantiye/{santiyekasaid?}",
            defaults: new { controller = "SantiyeKasa", action = "SantiyeKasaGuncelleFromSantiye" }
            );
        //DETAY
        app.MapControllerRoute(
            name: "SantiyeKasaDetay",
            pattern: "SantiyeKasa/SantiyeKasaDetay/{id}",
            defaults: new { controller = "SantiyeKasa", action = "SantiyeKasaDetay" }
            );
        //SÝL
        app.MapControllerRoute(
            name: "SantiyeKasaSil",
            pattern: "SantiyeKasa/SantiyeKasaSil/{santiyekasaid?}",
            defaults: new { controller = "SantiyeKasa", action = "SantiyeKasaSil" }
            );
        //GERÝ YÜKLE
        app.MapControllerRoute(
            name: "SantiyeKasaSil",
            pattern: "SantiyeKasa/SantiyeKasaGeriYukle/id",
            defaults: new { controller = "SantiyeKasa", action = "SantiyeKasaGeriYukle" }
            );
#endregion

#region CARÝ HESAP
//INDEX
app.MapControllerRoute(
        name: "CariHesap",
        pattern: "CariHesap/CariHesap/{santiyeid?}",
        defaults: new { controller = "CariHesap", action = "CariHesap" }
        );
    app.MapControllerRoute(
        name: "CariHesapArsiv",
        pattern: "CariHesap/CariHesapArsiv/{santiyeid?}",
        defaults: new { controller = "CariHesap", action = "CariHesapArsiv" }
        );
    //EKLE
    app.MapControllerRoute(
        name: "CariHesapEkleme",
        pattern: "CariHesap/CariHesapEkleme",
        defaults: new { controller = "CariHesap", action = "CariHesapEkleme" }
        );
    //GUNCELLE
    app.MapControllerRoute(
        name: "CariHesapGuncelle",
        pattern: "CariHesap/CariHesapGuncelle/{carihesapid?}",
        defaults: new { controller = "CariHesap", action = "CariHesapGuncelle" }
        );
    //DETAY
    //SÝL
    app.MapControllerRoute(
        name: "CariHesapSil",
        pattern: "CariHesap/CariHesapSil/{carihesapid?}",
        defaults: new { controller = "CariHesap", action = "CariHesapSil" }
        );
    #endregion

    #region CARÝ KASA
    //INDEX
    app.MapControllerRoute(
        name: "CariKasa",
        pattern: "CariKasa/CariKasa/{carihesapid}/{gkid?}",
        defaults: new { controller = "CariKasa", action = "CariKasa" }
        );
    //EKLE
    app.MapControllerRoute(
        name: "CariKasaEkleme",
        pattern: "CariKasa/CariKasaEkleme",
        defaults: new { controller = "CariKasa", action = "CariKasaEkleme" }
        );
    app.MapControllerRoute(
        name: "CariKasaEklemeFromCari",
        pattern: "CariKasa/CariKasaFaturaEklemeFromCari/{carihesapid}",
        defaults: new { controller = "CariKasa", action = "CariKasaEklemeFromCari" }
        );
    //GUNCELLE
    app.MapControllerRoute(
        name: "CariKasaGuncelle",
        pattern: "CariKasa/CariKasaGuncelle/{carikasaid?}",
        defaults: new { controller = "CariKasa", action = "CariKasaGuncelle" }
        );
    app.MapControllerRoute(
        name: "CariKasa",
        pattern: "CariKasa/CariKasaFaturaGuncelleFromCari/{carikasaid?}",
        defaults: new { controller = "CariKasa", action = "CariKasaFaturaGuncelleFromCari" }
        );
    //DETAY
    app.MapControllerRoute(
        name: "CariKasaDetay",
        pattern: "CariKasa/CariKasaDetay/{carikasaid?}",
        defaults: new { controller = "CariKasa", action = "CariKasaDetay" }
        );
    //SÝL
    app.MapControllerRoute(
        name: "CariKasaSil",
        pattern: "CariKasa/CariKasaSil/{carikasaid?}",
        defaults: new { controller = "CariKasa", action = "CariKasaSil" }
        );







    #endregion

    #region BANKA KASA
    //INDEX
    app.MapControllerRoute(
        name: "BankaKasa",
        pattern: "BankaKasa/BankaKasa/{bankahesapid?}",
        defaults: new { controller = "BankaKasa", action = "BankaKasa" }
        );
    //EKLE
    app.MapControllerRoute(
        name: "BankaKasa",
        pattern: "BankaKasa/BankaKasaEkleme/",
        defaults: new { controller = "BankaKasa", action = "BankaKasaEkleme" }
        );
    app.MapControllerRoute(
        name: "BankaKasa",
        pattern: "BankaKasa/BankaKasaEklemeSantiyeEft/",
        defaults: new { controller = "BankaKasa", action = "BankaKasaEklemeSantiyeEft" }
        );
    app.MapControllerRoute(
        name: "BankaKasa",
        pattern: "BankaKasa/BankaKasaEklemeEft/",
        defaults: new { controller = "BankaKasa", action = "BankaKasaEklemeEft" }
        );
    //GUNCELLE
    app.MapControllerRoute(
        name: "BankaKasa",
        pattern: "BankaKasa/BankaKasaGuncelle/{bankakasaid?}",
        defaults: new { controller = "BankaKasa", action = "BankaKasaGuncelle" }
        );
    //DETAY
    app.MapControllerRoute(
        name: "BankaKasa",
        pattern: "BankaKasa/BankaKasaDetay/{bankakasaid?}",
        defaults: new { controller = "BankaKasa", action = "BankaKasaDetay" }
        );
    //SÝL
    app.MapControllerRoute(
        name: "BankaKasa",
        pattern: "BankaKasa/BankaKasaSil/{bankakasaid?}",
        defaults: new { controller = "BankaKasa", action = "BankaKasaSil" }
        );
    app.MapControllerRoute(
        name: "BankaKasa",
        pattern: "BankaKasa/BankaKasaSilSantiyeEft/{bankakasasantiyeid?}",
        defaults: new { controller = "BankaKasa", action = "BankaKasaSilSantiyeEft" }
        );
    #endregion

    #region ÇEK
    //INDEX
    app.MapControllerRoute(
        name: "Cek",
        pattern: "Cek/Index",
        defaults: new { controller = "Cek", action = "Index" }
        );
    //EKLEME
    app.MapControllerRoute(
        name: "Cek",
        pattern: "Cek/CekEklemeFromCari/{carihesapid?}",
        defaults: new { controller = "Cek", action = "CekEklemeFromCari" }
        );
    //TAHSÝL
    app.MapControllerRoute(
        name: "Cek",
        pattern: "Cek/CekTahsil/{cekid?}",
        defaults: new { controller = "Cek", action = "CekTahsil" }
        );
    //GUNCELLE
    app.MapControllerRoute(
        name: "Cek",
        pattern: "Cek/CekGuncelle/{cekid?}",
        defaults: new { controller = "Cek", action = "CekGuncelle" }
        );
    //DETAY
    app.MapControllerRoute(
        name: "Cek",
        pattern: "Cek/CekDetay/{cekid?}",
        defaults: new { controller = "Cek", action = "CekDetay" }
        );
    //SÝL
    app.MapControllerRoute(
        name: "Cek",
        pattern: "Cek/CekSil/{cekid?}",
        defaults: new { controller = "Cek", action = "CekSil" }
        );
    //FÝLTRE
    app.MapControllerRoute(
        name: "Cek",
        pattern: "Cek/CekSantiye/{santiyeid?}",
        defaults: new { controller = "Cek", action = "CekSantiye" }
        );
    app.MapControllerRoute(
        name: "Cek",
        pattern: "Cek/CekBanka/{bankahesapid?}",
        defaults: new { controller = "Cek", action = "CekBanka" }
        );
    app.MapControllerRoute(
        name: "Cek",
        pattern: "Cek/CekSirket/{sirketid?}",
        defaults: new { controller = "Cek", action = "CekSirket" }
        );
    #endregion

    #region NAKÝT
    //EKLEME
    app.MapControllerRoute(
        name: "Nakit",
        pattern: "Nakit/NakitEklemeFromCari/{carihesapid?}",
        defaults: new { controller = "Nakit", action = "NakitEklemeFromCari" }
        );
    //GÜNCELLE
    app.MapControllerRoute(
        name: "Nakit",
        pattern: "Nakit/NakitGuncelle/{nakitid?}",
        defaults: new { controller = "Nakit", action = "NakitGuncelle" }
        );
    //DETAY
    app.MapControllerRoute(
        name: "Nakit",
        pattern: "Nakit/NakitDetay/{nakitid?}",
        defaults: new { controller = "Nakit", action = "NakitDetay" }
        );
    //SÝL
    app.MapControllerRoute(
        name: "Nakit",
        pattern: "Nakit/NakitSil/{nakitid?}",
        defaults: new { controller = "Nakit", action = "NakitSil" }
        );
    //FÝLTRE
    app.MapControllerRoute(
        name: "Nakit",
        pattern: "Nakit/NakitSantiye/{santiyeid?}",
        defaults: new { controller = "Nakit", action = "NakitSantiye" }
        );
    app.MapControllerRoute(
        name: "Nakit",
        pattern: "Nakit/NakitBanka/{bankahesapid?}",
        defaults: new { controller = "Nakit", action = "NakitBanka" }
        );
    app.MapControllerRoute(
        name: "Nakit",
        pattern: "Nakit/NakitSirket/{sirketid?}",
        defaults: new { controller = "Nakit", action = "NakitSirket" }
        );
    #endregion

    


app.Run();
