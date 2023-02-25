using SantiyeOnMuh.Business.Abstract;
using SantiyeOnMuh.Business.Concrete;
using SantiyeOnMuh.DataAccess.Abstract;
using SantiyeOnMuh.DataAccess.Concrete.SqlServer;

var builder = WebApplication.CreateBuilder(args);


//--EF CORE 6'DA DE���T�R�LM�� - B�T��E KADAR MS S�TEDEN COPY PASTE

// Add services to the container.
builder.Services.AddControllersWithViews();

//==>> ZATEN PROGRAM CS ��ER�S�NDE BUDASI TANIMLIYDI -var builder = WebApplication.CreateBuilder(args);-

// Add the memory cache services.
builder.Services.AddMemoryCache();

// Add a custom scoped service.
builder.Services.AddScoped<ISantiyeService, SantiyeManager>();
builder.Services.AddScoped<ISantiyeRepository, EfCoreSantiyeRepository>();
//builder.Services.AddScoped < ISantiyeRepository,


var app = builder.Build();

//--B�T��

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
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
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
