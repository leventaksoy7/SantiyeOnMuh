using SantiyeOnMuh.Entity;

namespace SantiyeOnMuh.WebUI.Models
{
    public class CariHesapViewListModel
    {
        public PageInfo PageInfo { get; set; }
        public List<ECariHesap> CariHesaps { get; set; }
        public List<ESantiye> Santiyes { get; set; }
    }
}
