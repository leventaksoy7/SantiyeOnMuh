using SantiyeOnMuh.Entity;

namespace SantiyeOnMuh.WebUI.Models
{
    public class SantiyeKasaViewListModel
    {
        public PageInfo PageInfo { get; set; }
        public ESantiye Santiye { get; set; }
        public List<ESantiyeKasa> SantiyeKasas { get; set; }
        public List<ESantiyeGiderKalemi> SantiyeGiderKalemis { get; set; }
    }
}
