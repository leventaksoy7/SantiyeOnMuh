using SantiyeOnMuh.Entity;

namespace SantiyeOnMuh.WebUI.Models
{
    public class CariHesapViewListModel
    {
        public PageInfo PageInfo { get; set; }
        public List<CariHesap> CariHesaps { get; set; }
        public List<Santiye> Santiyes { get; set; }
    }
}
