using SantiyeOnMuh.Entity;
using SantiyeOnMuh.WebUI.Models.Modeller;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SantiyeOnMuh.WebUI.Models.Modeller
{
    public class Cek
    {
        public int Id { get; set; }

        [Display(Name = "TARİH")]
        [Required(ErrorMessage = "TARİH GİRMELİSİNİZ")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime Tarih { get; set; }

        [Display(Name = "AÇIKLAMA GİRİLMESİ ZORUNLUDUR")]
        [Required(ErrorMessage = "AÇIKLAMA GİRMELİSİNİZ")]
        public string Aciklama { get; set; }

        [Display(Name = "ÇEK NUMARASI GİRİLMESİ ZORUNLUDUR")]
        [Required(ErrorMessage = "ÇEK NUMARASI GİRMELİSİNİZ")]
        public string CekNo { get; set; }

        [Display(Name = "TUTAR GİRİLMESİ ZORUNLUDUR")]
        [Required(ErrorMessage = "TUTAR GİRMELİSİNİZ")]
        [Column(TypeName = "decimal(18, 2)")]
        public string Tutar { get; set; }
        public string? ImgUrl { get; set; }
        public int? BankaKasaKaynak { get; set; }
        public int? CariKasaKaynak { get; set; }
        public DateTime SistemeGiris { get; set; }
        public DateTime SonGuncelleme { get; set; }
        public bool Durum { get; set; }
        public bool OdemeDurumu { get; set; }
        public int CariHesapId { get; set; }
        public ECariHesap CariHesap { get; set; }
        public int SirketId { get; set; }
        public ESirket Sirket { get; set; }
        public int BankaHesapId { get; set; }
        public EBankaHesap BankaHesap { get; set; }
        public Cek()
        {
            Tarih = System.DateTime.Now;

            Durum = true;
            OdemeDurumu = false;

            Tutar = "0";

            BankaKasaKaynak = null;
            CariKasaKaynak = null;

            SistemeGiris = System.DateTime.Now;
            SonGuncelleme = System.DateTime.Now;
        }
    }
}
