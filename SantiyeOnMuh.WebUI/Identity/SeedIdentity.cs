using Microsoft.AspNetCore.Identity;
using System.Linq;

namespace SantiyeOnMuh.WebUI.Identity
{
    public static class SeedIdentity
    {
        public static async Task Seed(UserManager<User> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
        {
            var roles = configuration.GetSection("Data:Roles").GetChildren().Select(x=>x.Value).ToArray();

            foreach(var rol in roles)
            {
                if(!await roleManager.RoleExistsAsync(rol))
                {
                    await roleManager.CreateAsync(new IdentityRole(rol));
                }
            }

            var username = configuration["Data:AdminUser:username"];
            var password = configuration["Data:AdminUser:password"];
            var role = configuration["Data:AdminUser:role"];

            if(await userManager.FindByIdAsync(username)==null)
            {
                //await roleManager.CreateAsync(new IdentityRole(role));

                var user = new User()
                {
                    UserName = username,
                    Ad = username,
                    SoyAd = username
                };

                var result = await userManager.CreateAsync(user, password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, role);
                }
            }
        }
    }
}
