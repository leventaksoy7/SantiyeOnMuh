using Microsoft.AspNetCore.Identity;

namespace SantiyeOnMuh.WebUI.Identity
{
    public class User: IdentityUser
    {
        public string Ad { get; set; }
        public string SoyAd { get; set; }
        public int? SantiyeId { get; set; }
    }
}
