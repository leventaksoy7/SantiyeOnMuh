using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;

namespace SantiyeOnMuh.Entity
{
    public class EBankaHesap
    {
        public int Id { get; set; }
        public string HesapAdi { get; set; }
        public string BankaAdi { get; set; }
        public string? HesapNo{ get; set; }
        public string? IbanNo { get; set; }
        public bool Durum { get; set; }
        public List<ECek> Ceks { get; set; }
        public List<ENakit> Nakits { get; set; }
        public List<EBankaKasa> BankaKasas { get; set; }
        public EBankaHesap()
        {
            Durum = true;
        }
    }
}