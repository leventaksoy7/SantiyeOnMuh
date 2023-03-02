using SantiyeOnMuh.Entity;
using System.Runtime.ConstrainedExecution;

namespace SantiyeOnMuh.WebUI.Models
{
    public class CekViewListModel
    {
        public PageInfo PageInfo { get; set; }
        public List<EBankaHesap> BankaHesaps { get; set; }
        public List<ESantiye> Santiyes { get; set; }
        public List<ESirket> Sirkets { get; set; }
        public List<ECek> Ceks { get; set; }
    }
}
