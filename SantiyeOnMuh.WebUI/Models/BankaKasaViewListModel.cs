using SantiyeOnMuh.Entity;

namespace SantiyeOnMuh.WebUI.Models
{    
    public class PageInfo
    {
        public int TotalItem { get; set; }
        public int ItemPerPage { get; set; }
        public int CurrentPage { get; set; }
        public int? UrlInfo { get; set; }

        public int TotalPages()
        {
            return (int)Math.Ceiling(((decimal)TotalItem / ItemPerPage));
        }
    }
    public class BankaKasaViewListModel
    {
        public PageInfo PageInfo { get; set; }
        public List<EBankaKasa> BankaKasas { get; set; }
        public List<EBankaHesap> BankaHesaps { get; set; }
    }
}
