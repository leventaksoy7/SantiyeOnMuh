using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SantiyeOnMuh.Business.Abstract;
using SantiyeOnMuh.Business.Concrete;
using SantiyeOnMuh.DataAccess.Abstract;
using SantiyeOnMuh.DataAccess.Concrete;
using SantiyeOnMuh.DataAccess.Concrete.EfCore;
using SantiyeOnMuh.DataAccess.Concrete.SqlServer;
using SantiyeOnMuh.WebUI.Extensions;
using SantiyeOnMuh.WebUI.Identity;
using System.Configuration;

IConfiguration configuration;

var builder = WebApplication.CreateBuilder(args);

configuration = builder.Configuration;



//--EF CORE 6'DA DE���T�R�LM�� - B�T��E KADAR MS S�TEDEN COPY PASTE

// Add services to the container.
builder.Services.AddControllersWithViews();

//==>> ZATEN PROGRAM CS ��ER�S�NDE BUDASI TANIMLIYDI -var builder = WebApplication.CreateBuilder(args);-

// Add the memory cache services.
builder.Services.AddMemoryCache();

// Add a custom scoped service.

//builder.Services.AddDbContext<ApplicationContext>
//    (options => options.UseSqlServer(@"Server=localhost\SQLEXPRESS; Database=SantiyeOnMuhasebe; Trusted_Connection=True;Encrypt=false;TrustServerCertificate=true;"));
//builder.Services.AddDbContext<Context>
//    (options => options.UseSqlServer(@"Server=localhost\SQLEXPRESS; Database=SantiyeOnMuhasebe; Trusted_Connection=True;Encrypt=false;TrustServerCertificate=true;"));

//USER
builder.Services.AddDbContext<ApplicationContext>
    (options => options.UseSqlServer(configuration.GetConnectionString("MsSqlServerCon")));
//MAINCONTEX
builder.Services.AddDbContext<Context>
    (options => options.UseSqlServer(configuration.GetConnectionString("MsSqlServerCon")));

//builder.Build().MigrateDatabase().Run();


builder.Services.AddIdentity<User,IdentityRole>().AddEntityFrameworkStores<ApplicationContext>().AddDefaultTokenProviders();

builder.Services.Configure<IdentityOptions>(options =>
{
    //PASSWORD
    options.Password.RequireDigit = false; // RAKAM ZORUNLU
    options.Password.RequireLowercase = false; // K���K HARF DE�ER ZORUNLU
    options.Password.RequireUppercase = false; // B�Y�K HARF DE�ER ZORUNLU
    options.Password.RequiredLength = 6; // M�N 6 KARAKTER ZORUNLU
    options.Password.RequireNonAlphanumeric = false; // �EK�L DE�ER ZORUNLU

    //LOCKOUT
    options.Lockout.MaxFailedAccessAttempts = 5; // 5 KERE YANLI� PAROLA G�R�� HAKKI MAX�MUM
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15); // 5 DK HESAP K�L�D�
    options.Lockout.AllowedForNewUsers = true; // LOCKOUT AKT�F OLMASI ���N

    //USER
    //options.User.AllowedUserNameCharacters = ""; // ZORUNLU OLMASINI �STED���M�Z KARAKTERLER L�STES�
    options.User.RequireUniqueEmail = false; // HER KULLANICININ FARKLI MA�L ADRES� OLMASI ���N (BEN FALSE YAPTIM, AYNI MA�L FARKLI KULLANICILAR ���N KULLANILAB�L�R)
    options.SignIn.RequireConfirmedEmail = false; // G�R�� ���N MA�LDEN ONAYLANMI� OLMAK ZORUNLU (MA�L DO�RULAMASI)
    options.SignIn.RequireConfirmedPhoneNumber = false; // G�R�� ���N TELEFON ONAYLANMI� OLMAK ZORUNLU (MA�L DO�RULAMASI)   
});

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/account/login";
    options.LogoutPath = "/account/logout";
    options.AccessDeniedPath = "/account/accessdenied";
    options.SlidingExpiration = false; //COOKIE ZAMAN SIFIRLAMASINI KAPATTIM. DEFAULT OLARAK HER ISTEKTE SIFIRLANIR
    options.ExpireTimeSpan = TimeSpan.FromMinutes(1); //COOKIE ZAMANI
    options.Cookie = new CookieBuilder
    {
        HttpOnly = true, //SADECE HTTP TALEB� �LE COOKIE ALINAB�LS�N D�YE
        Name = ".santiyeonmuh.security.cookie", // �ZEL ADLANDIRMA ���N
        SameSite = SameSiteMode.Strict, //cookie 
    };
});

//SANT�YE
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddScoped<ISantiyeService, SantiyeManager>();
builder.Services.AddScoped<ISantiyeRepository, EfCoreSantiyeRepository>();

builder.Services.AddScoped<ISantiyeKasaRepository, EfCoreSantiyeKasaRepository>();
builder.Services.AddScoped<ISantiyeKasaService, SantiyeKasaManager>();

builder.Services.AddScoped<ISantiyeGiderKalemiRepository, EfCoreSantiyeGiderKalemiRepository>();
builder.Services.AddScoped<ISantiyeGiderKalemiService, SantiyeGiderKalemiManager>();

//CAR�
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

//S�RKET
builder.Services.AddScoped<ISirketRepository, EfCoreSirketRepository>();
builder.Services.AddScoped<ISirketService, SirketManager>();

var app = builder.Build();

//--B�T��

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

//SeedDatabase.Seed();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseAuthentication();

app.UseRouting();

app.UseAuthorization();

//app.MapControllerRoute(
//    name: "default",
//    pattern: "{controller=AnaSayfa}/{action=AnaSayfa}/{id?}");

//INDEX
//EKLE
//GUNCELLE
//DETAY
//S�L
app.MapControllerRoute(
       name: "default",
       pattern: "{controller=Account}/{action=Login}");

app.MapControllerRoute(
        name: "adminroleedit",
        pattern: "Admin/RoleEdit/{id?}",
        defaults: new { controller = "Admin", action = "RoleEdit" }
        );

app.MapControllerRoute(
        name: "default2",
        pattern: "{controller=AnaSayfa}/{action=Anasayfa}/{id?}");
    #region �ANT�YE KASA
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
        //S�L
        app.MapControllerRoute(
            name: "SantiyeKasaSil",
            pattern: "SantiyeKasa/SantiyeKasaSil/{santiyekasaid?}",
            defaults: new { controller = "SantiyeKasa", action = "SantiyeKasaSil" }
            );
        //GER� Y�KLE
        app.MapControllerRoute(
            name: "SantiyeKasaSil",
            pattern: "SantiyeKasa/SantiyeKasaGeriYukle/id",
            defaults: new { controller = "SantiyeKasa", action = "SantiyeKasaGeriYukle" }
            );
#endregion

    #region CAR� HESAP
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
    //S�L
    app.MapControllerRoute(
        name: "CariHesapSil",
        pattern: "CariHesap/CariHesapSil/{carihesapid?}",
        defaults: new { controller = "CariHesap", action = "CariHesapSil" }
        );
    #endregion

    #region CAR� KASA
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
    //S�L
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
    //S�L
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

    #region �EK
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
    //TAHS�L
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
    //S�L
    app.MapControllerRoute(
        name: "Cek",
        pattern: "Cek/CekSil/{cekid?}",
        defaults: new { controller = "Cek", action = "CekSil" }
        );
    //F�LTRE
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

    #region NAK�T
    //EKLEME
    app.MapControllerRoute(
        name: "Nakit",
        pattern: "Nakit/NakitEklemeFromCari/{carihesapid?}",
        defaults: new { controller = "Nakit", action = "NakitEklemeFromCari" }
        );
    //G�NCELLE
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
    //S�L
    app.MapControllerRoute(
        name: "Nakit",
        pattern: "Nakit/NakitSil/{nakitid?}",
        defaults: new { controller = "Nakit", action = "NakitSil" }
        );
    //F�LTRE
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

var scopeFactory = app.Services.GetRequiredService<IServiceScopeFactory>();
using (var scope = scopeFactory.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
    await SeedIdentity.Seed(userManager, roleManager, configuration);
}

app.Run();
