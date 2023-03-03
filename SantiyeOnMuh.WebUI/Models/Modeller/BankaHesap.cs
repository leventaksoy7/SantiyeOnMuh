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
        [Display(Name = "HESAP ADI GİRİLMESİ ZORUNLUDUR")]
        public string HesapAdi { get; set; }

        [Required(ErrorMessage = "BANKA ADI BOŞ BIRAKILAMAZ!")]
        [Display(Name = "BANKA ADI GİRİLMESİ ZORUNLUDUR")]
        public string BankaAdi { get; set; }

        [Display(Name = "HESAP NUMARASI")]
        public string? HesapNo { get; set; }

        [Display(Name = "IBAN NUMARASI")]
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
