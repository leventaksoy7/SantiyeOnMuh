using SantiyeOnMuh.Entity;

namespace SantiyeOnMuh.WebUI.Models
{
    public class AdminViewListModel
    {
        public List<ESantiyeGiderKalemi> SantiyeGiderKalemis { get; set; }
        public List<ECariGiderKalemi> CariGiderKalemis { get; set; }
        public List<EBankaHesap> BankaHesaps { get; set; }
        public List<ESirket> Sirkets { get; set; }
    }
}
