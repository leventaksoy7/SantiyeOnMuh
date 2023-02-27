using SantiyeOnMuh.Entity;

namespace SantiyeOnMuh.WebUI.Models
{
    public class AnaSayfaViewListModel
    {
        public List<Santiye> Santiyes { get; set; }
        public List<SantiyeGiderKalemi> SantiyeGiderKalemis { get; set; }
        public List<BankaHesap> BankaHesaps { get; set; }
        public List<BankaKasa> BankaKasas { get; set; }
        public List<SantiyeKasa> SantiyeKasas { get; set; }
    }
}
