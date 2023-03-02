using DocumentFormat.OpenXml.Wordprocessing;
using SantiyeOnMuh.WebUI.Models.Modeller;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SantiyeOnMuh.WebUI.Models.Modeller
{
    public class BankaHesap
    {
        public int Id { get; set; }

        [Display(Name="Hesap Adı")]
        public string HesapAdi { get; set; }

        [Display(Name = "Banka Adı")]
        public string? BankaAdi { get; set; }

        [Display(Name = "Hesap Numarası")]
        public string HesapNo { get; set; }
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
