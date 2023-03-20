using Microsoft.EntityFrameworkCore;
using SantiyeOnMuh.DataAccess.Concrete;
using SantiyeOnMuh.WebUI.Identity;

namespace SantiyeOnMuh.WebUI.Extensions
{
    public static class MigrationManager
    {
        public static IHost MigrateDatabase(this IHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                using(var applicationContext = scope.ServiceProvider.GetRequiredService<ApplicationContext>())
                {
                    try
                    {
                        applicationContext.Database.Migrate();
                    }
                    catch (System.Exception)
                    {
                        //Loglama için
                        throw;
                    }
                }
                using (var context = scope.ServiceProvider.GetRequiredService<Context>())
                {
                    try
                    {
                        context.Database.Migrate();
                    }
                    catch (System.Exception)
                    {
                        //Loglama için
                        throw;
                    }
                }
            }

            return host;
        }
    }
}
