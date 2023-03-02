using SantiyeOnMuh.WebUI.Models.Modeller;

namespace SantiyeOnMuh.WebUI.Models.Modeller
{
    public class CariHesap
    {
        public int Id { get; set; }
        public string Ad { get; set; }
        public string Adres { get; set; }
        public string Telefon { get; set; }
        public string? VergiNo { get; set; }
        public string IlgiliKisi { get; set; }
        public string? IlgiliKisiTelefon { get; set; }
        public string Odeme { get; set; }
        public string? Vade { get; set; }
        public bool Durum { get; set; }
        public List<Cek> Ceks { get; set; }
        public List<Nakit> Nakits { get; set; }
        public List<CariKasa> CariKasas { get; set; }
        public int SantiyeId { get; set; }
        public Santiye Santiye { get; set; }
        public CariHesap()
        {
            Durum = true;
        }
    }
}
