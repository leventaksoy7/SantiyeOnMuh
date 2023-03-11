using System.ComponentModel.DataAnnotations;

namespace SantiyeOnMuh.WebUI.Models
{
    public class BankaKasaEftSantiyeModel
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
        [Display(Name = "TUTAR")]
        public string Tutar { get; set; }
        [Display(Name = "GÖNDEREN HESAP")]
        public int BankaHesapId { get; set; }
        [Display(Name = "ALICI ŞANTİYE")]
        public int SantiyeId { get; set; }
        public int? BankaKasaId { get; set; }
        public int? SantiyeKasaId { get; set; }
        public BankaKasaEftSantiyeModel()
        {
            BankaKasaId = null;
            SantiyeKasaId = null;
        }
    }
}
