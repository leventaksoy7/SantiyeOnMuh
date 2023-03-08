using SantiyeOnMuh.Entity;
using SantiyeOnMuh.WebUI.Models.Modeller;
using System.ComponentModel.DataAnnotations;

namespace SantiyeOnMuh.WebUI.Models.Modeller
{
    public class SantiyeGiderKalemi
    {
        public int Id { get; set; }

        [StringLength(30)]
        [Display(Name ="ŞANTİYE GİDER KALEMİ",Prompt = "ŞANTİYE GİDER KALEMİ ADI GİRİLMESİ ZORUNLUDUR!")]
        [Required(ErrorMessage = "ŞANTİYE GİDER KALEMİ ADI GİRİLMESİ ZORUNLUDUR!")]
        public string Ad { get; set; }

        public bool Durum { get; set; }

        public bool Tur { get; set; }

        public List<ESantiyeKasa> SantiyeKasas { get; set; }

        public SantiyeGiderKalemi()
        {
            Durum = true;
            Tur = true;
        }
    }
}
