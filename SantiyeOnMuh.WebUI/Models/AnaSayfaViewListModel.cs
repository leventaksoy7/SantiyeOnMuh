using SantiyeOnMuh.Entity;

namespace SantiyeOnMuh.WebUI.Models
{
    public class AnaSayfaViewListModel
    {
        public List<ESantiye> Santiyes { get; set; }
        public List<ESantiyeGiderKalemi> SantiyeGiderKalemis { get; set; }
        public List<EBankaHesap> BankaHesaps { get; set; }
        public List<EBankaKasa> BankaKasas { get; set; }
        public List<ESantiyeKasa> SantiyeKasas { get; set; }
    }
}
