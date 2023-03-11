using System.ComponentModel.DataAnnotations;

namespace SantiyeOnMuh.WebUI.Models
{
    public class BankaKasaEftModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "TARİH GİRMELİSİNİZ")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        [Display(Name = "TARİH")]
        public DateTime Tarih { get; set; }

        [Required(ErrorMessage = "AÇIKLAMA BOŞ BIRAKILAMAZ!")]
        [Display(Name = "AÇIKLAMA", Prompt = "AÇIKLAMA BOŞ BIRAKILAMAZ!")]
        public string Aciklama { get; set; }
        public string? Nitelik { get; set; }
        [Display(Name = "TUTAR")]
        public string Tutar { get; set; }
        [Display(Name = "GÖNDEREN HESAP")]
        public int GonderenBanka { get; set; }
        [Display(Name = "ALICI HESAP")]
        public int AliciBanka { get; set; }
        public DateTime SistemeGiris { get; set; }
        public DateTime SonGuncelleme { get; set; }
        public bool Durum { get; set; }
        public BankaKasaEftModel()
        {
            Tarih = DateTime.Now;
            Nitelik = "EFT";
            Tutar = "0";
            Durum = true;

            SistemeGiris = DateTime.Now;
            SonGuncelleme = DateTime.Now;
        }
    }
}
