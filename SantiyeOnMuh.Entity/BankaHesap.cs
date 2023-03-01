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
        public string HesapAdi { get; set; }
        public string BankaAdi { get; set; }
        public string HesapNo{ get; set; }
        public string IbanNo { get; set; }
        public bool Durum { get; set; }
        public List<Cek> Ceks { get; set; }
        public List<Nakit> Nakits { get; set; }
        public List<BankaKasa> BankaKasas { get; set; }
        public BankaHesap()
        {
            Durum = true;
        }
    }
}