using SantiyeOnMuh.Business.Abstract;
using SantiyeOnMuh.Business.Concrete;
using SantiyeOnMuh.DataAccess.Abstract;
using SantiyeOnMuh.DataAccess.Concrete.EfCore;
using SantiyeOnMuh.DataAccess.Concrete.SqlServer;

var builder = WebApplication.CreateBuilder(args);


//--EF CORE 6'DA DE���T�R�LM�� - B�T��E KADAR MS S�TEDEN COPY PASTE

// Add services to the container.
builder.Services.AddControllersWithViews();

//==>> ZATEN PROGRAM CS ��ER�S�NDE BUDASI TANIMLIYDI -var builder = WebApplication.CreateBuilder(args);-

// Add the memory cache services.
builder.Services.AddMemoryCache();

// Add a custom scoped service.
//SANT�YE
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
builder.Services.AddScoped<IOdemeCekRepository, EfCoreOdemeCekRepository>();
builder.Services.AddScoped<IOdemeCekService, OdemeCekManager>();

builder.Services.AddScoped<IOdemeNakitRepository, EfCoreOdemeNakitRepository>();
builder.Services.AddScoped<IOdemeNakitService, OdemeNakitManager>();

//S�RKET
builder.Services.AddScoped<ISirketRepository, EfCoreSirketRepository>();
builder.Services.AddScoped<ISirketService, SirketManager>();

var app = builder.Build();

//--B�T��

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    SeedDatabase.Seed();
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=AnaSayfa}/{action=Index}/{id?}");

app.Run();
