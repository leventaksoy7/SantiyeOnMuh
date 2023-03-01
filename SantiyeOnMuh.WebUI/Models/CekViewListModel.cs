using SantiyeOnMuh.Entity;
using System.Runtime.ConstrainedExecution;

namespace SantiyeOnMuh.WebUI.Models
{
    public class CekViewListModel
    {
        public PageInfo PageInfo { get; set; }
        public List<BankaHesap> BankaHesaps { get; set; }
        public List<Santiye> Santiyes { get; set; }
        public List<Sirket> Sirkets { get; set; }
        public List<Cek> Ceks { get; set; }
    }
}
