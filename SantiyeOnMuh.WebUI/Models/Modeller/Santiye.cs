using SantiyeOnMuh.Entity;
using SantiyeOnMuh.WebUI.Models.Modeller;
using System.ComponentModel.DataAnnotations;

namespace SantiyeOnMuh.WebUI.Models.Modeller
{
    public class Santiye
    {
        public int Id { get; set; }

        [StringLength(30)]
        [Display(Name = "ŞANTİYE ADI GİRİLMESİ ZORUNLUDUR")]
        [Required(ErrorMessage = "ŞANTİYE ADI GİRMELİSİNİZ")]
        public string Ad { get; set; }

        [Display(Name = "ŞANTİYE ADRESİ")]
        public string? Adres { get; set; }

        public bool Durum { get; set; }

        public List<ESantiyeKasa> SantiyeKasas { get; set; }

        public List<ECariHesap> CariHesaps { get; set; }

        public Santiye()
        {
            Durum = true;
            Ad = string.Empty;
            Adres = string.Empty;
        }
    }
}
