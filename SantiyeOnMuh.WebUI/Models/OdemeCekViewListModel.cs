using SantiyeOnMuh.Entity;
using System.Runtime.ConstrainedExecution;

namespace SantiyeOnMuh.WebUI.Models
{
    public class OdemeCekViewListModel
    {
        public PageInfo PageInfo { get; set; }
        public List<BankaHesap> BankaHesaps { get; set; }
        public List<Santiye> Santiyes { get; set; }
        public List<Sirket> Sirkets { get; set; }
        public List<OdemeCek> OdemeCeks { get; set; }
    }
}
