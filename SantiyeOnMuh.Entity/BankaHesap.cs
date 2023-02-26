using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;

namespace SantiyeOnMuh.Entity
{
    public class BankaHesap
    {
        public int Id { get; set; }
        public string HesapAd { get; set; }
        public string BankaAd { get; set; }
        public string HesapNo{ get; set; }
        public string Iban { get; set; }
        public bool Durum { get; set; }
        public List<OdemeCek> OdemeCeks { get; set; }
        public List<OdemeNakit> OdemeNakits { get; set; }
        public List<BankaKasa> BankaHesapKasas { get; set; }
        public BankaHesap()
        {
            Durum = true;
        }
    }
}
