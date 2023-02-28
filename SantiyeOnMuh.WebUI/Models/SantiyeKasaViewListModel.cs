using SantiyeOnMuh.Entity;

namespace SantiyeOnMuh.WebUI.Models
{
    public class SantiyeKasaViewListModel
    {
        public PageInfo PageInfo { get; set; }
        public Santiye Santiye { get; set; }
        public List<SantiyeKasa> SantiyeKasas { get; set; }
        public List<SantiyeGiderKalemi> SantiyeGiderKalemis { get; set; }
    }
}
