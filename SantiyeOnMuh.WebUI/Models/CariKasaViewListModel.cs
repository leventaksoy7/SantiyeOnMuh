using SantiyeOnMuh.Entity;

namespace SantiyeOnMuh.WebUI.Models
{
    public class CariKasaViewListModel
    {
        public PageInfo PageInfo { get; set; }
        public List<ECariKasa> CariKasas { get; set; }
        public List<ECariGiderKalemi> CariGiderKalemis { get; set; }
        public ECariHesap CariHesap { get; set; }
    }
}
