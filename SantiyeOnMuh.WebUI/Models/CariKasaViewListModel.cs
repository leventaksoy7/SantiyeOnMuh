using SantiyeOnMuh.Entity;

namespace SantiyeOnMuh.WebUI.Models
{
    public class CariKasaViewListModel
    {
        public PageInfo PageInfo { get; set; }
        public List<CariKasa> CariKasas { get; set; }
        public List<CariGiderKalemi> CariGiderKalemis { get; set; }
        public CariHesap CariHesap { get; set; }
    }
}
