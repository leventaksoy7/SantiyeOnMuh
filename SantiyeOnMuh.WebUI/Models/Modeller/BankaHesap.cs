using DocumentFormat.OpenXml.Wordprocessing;
using SantiyeOnMuh.Entity;
using SantiyeOnMuh.WebUI.Models.Modeller;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SantiyeOnMuh.WebUI.Models.Modeller
{
    public class BankaHesap
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "HESAP ADI BOŞ BIRAKILAMAZ!")]
        [Display(Name = "Hesap Adı", Prompt = "HESAP ADI BOŞ BIRAKILAMAZ!")]
        public string HesapAdi { get; set; }

        [Required(ErrorMessage = "BANKA ADI BOŞ BIRAKILAMAZ!")]
        [Display(Name = "Banka Adı")]
        public string BankaAdi { get; set; }

        [Display(Name = "Hesap Numarası")]
        public string? HesapNo { get; set; }

        [Display(Name = "Iban Numarası")]
        public string? IbanNo { get; set; }

        public bool Durum { get; set; }

        public List<ECek> Ceks { get; set; }

        public List<ENakit> Nakits { get; set; }

        public List<EBankaKasa> BankaKasas { get; set; }

        public BankaHesap()
        {
            Durum = true;
        }
    }
}
