using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;

namespace SantiyeOnMuh.Entity
{
    public class ECariHesap
    {
        public int Id { get; set; }
        public string Ad { get; set; }
        public string? Adres { get; set; }
        public string Telefon { get; set; }
        public string? VergiNo { get; set; }
        public string IlgiliKisi { get; set; }
        public string? IlgiliKisiTelefon { get; set; }
        public string? Odeme { get; set; }
        public string? Vade { get; set; }
        public bool Durum { get; set; }
        public List<ECek> Ceks { get; set; }
        public List<ENakit> Nakits { get; set; }
        public List<ECariKasa> CariKasas { get; set; }
        public int SantiyeId { get; set; }
        public ESantiye Santiye { get; set; }
        public ECariHesap()
        {
            Durum = true;
        }
    }
}
