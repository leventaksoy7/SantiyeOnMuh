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

        //[Display(Name = "Şirket Adı",Prompt ="ZORUNLU ALAN")]
        public string Ad { get; set; }

        //[Display(Name = "Vergi Numarası", Prompt ="LÜTFEN VERGİ NUMARASINI GİRİNİZ")]
        public string VergiNo { get; set; }
        public bool Durum { get; set; }
        public List<ECek> Ceks { get; set; }
        public List<ENakit> Nakits { get; set; }
        public Sirket()
        {
            Durum = true;
        }
    }
}
