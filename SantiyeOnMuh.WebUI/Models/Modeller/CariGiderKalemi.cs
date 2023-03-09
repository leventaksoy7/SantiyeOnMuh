using SantiyeOnMuh.Entity;
using SantiyeOnMuh.WebUI.Models.Modeller;
using System.ComponentModel.DataAnnotations;

namespace SantiyeOnMuh.WebUI.Models.Modeller
{
    public class CariGiderKalemi
    {
        public int Id { get; set; }

        [StringLength(30)]
        [Required(ErrorMessage = "EN FAZLA 30 KARAKTER GİRMELİSİNİZ, GİDER KALEMİ ADINI GİRİNİZ!")]
        [Display(Name = "GİDER KALEMİ ADI",
            Prompt = "EN FAZLA 30 KARAKTER GİRMELİSİNİZ, GİDER KALEMİ ADINI GİRİNİZ!")]
        public string Ad { get; set; }

        public bool Durum { get; set; }

        public bool Tur { get; set; }

        public List<ECariKasa>? CariKasas { get; set; }

        public CariGiderKalemi()
        {
            Durum = true;
            Tur = true;
        }
    }
}
