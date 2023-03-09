using DocumentFormat.OpenXml.Wordprocessing;
using SantiyeOnMuh.Entity;
using SantiyeOnMuh.WebUI.Models.Modeller;
using System.ComponentModel.DataAnnotations;

namespace SantiyeOnMuh.WebUI.Models.Modeller
{
    public class Sirket
    {
        //DATA ANNOTATION
        //VALIDATION
        public int Id { get; set; }

        [Required(ErrorMessage = "ŞİRKET ADI BOŞ BIRAKILAMAZ!")]
        [Display(Name = "ŞİRKET ADI", Prompt = "ŞİRKET ADI BOŞ BIRAKILAMAZ!")]
        public string Ad { get; set; }

        [Display(Name = "VERGİ NUMARASI")]
        public string? VergiNo { get; set; }

        public bool Durum { get; set; }

        public List<ECek>? Ceks { get; set; }

        public List<ENakit>? Nakits { get; set; }
        public Sirket()
        {
            Durum = true;
        }
    }
}
