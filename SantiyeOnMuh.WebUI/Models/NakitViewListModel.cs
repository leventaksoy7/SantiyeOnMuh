using SantiyeOnMuh.Entity;

namespace SantiyeOnMuh.WebUI.Models
{
    public class NakitViewListModel
    {
        public PageInfo PageInfo { get; set; }
        public List<EBankaHesap> BankaHesaps { get; set; }
        public List<ESantiye> Santiyes { get; set; }
        public List<ESirket> Sirkets { get; set; }
        public List<ENakit> Nakits { get; set; }
    }
}
