using SantiyeOnMuh.Entity;
using SantiyeOnMuh.WebUI.Models.Modeller;
using System.ComponentModel.DataAnnotations;

namespace SantiyeOnMuh.WebUI.Models.Modeller
{
    public class BankaKasa
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "TARİH GİRMELİSİNİZ")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime Tarih { get; set; }

        [Required(ErrorMessage = "AÇIKLAMA BOŞ BIRAKILAMAZ!")]
        [Display(Name = "AÇIKLAMA GİRİLMESİ ZORUNLUDUR")]
        [MinLength(10)]
        public string Aciklama { get; set; }

        //PARA AKIŞININ NİCELİĞİ ÖRN: EFT HAVALE ÖDEME VB
        [Required(ErrorMessage = "NİTELİK BOŞ BIRAKILAMAZ!")]
        [Display(Name = "EFT-HAVALE-FATURA ÖDEMESİ VB. OLARAK BELİRTİNİZ")]
        [MinLength(3)]
        public string Nitelik { get; set; }

        [Required(ErrorMessage = "TUTAR BOŞ BIRAKILAMAZ!")]
        [Display(Name = "TUTAR BOŞ BIRAKILAMAZ!")]
        public decimal Giren { get; set; }

        [Required(ErrorMessage = "TUTAR BOŞ BIRAKILAMAZ!")]
        [Display(Name = "TUTAR BOŞ BIRAKILAMAZ!")]
        public decimal Cikan { get; set; }

        public bool Durum { get; set; }

        public int? CekKaynak { get; set; }

        public int? NakitKaynak { get; set; }

        public int? SantiyeKasaKaynak { get; set; }

        public int BankaHesapId { get; set; }

        public EBankaHesap BankaHesap { get; set; }

        public DateTime SistemeGiris { get; set; }

        public DateTime SonGuncelleme { get; set; }

        public BankaKasa()
        {
            Durum = true;

            CekKaynak = null;
            NakitKaynak = null;
            SantiyeKasaKaynak = null;

            Giren = 0;
            Cikan = 0;

            Tarih = System.DateTime.Now;
            SistemeGiris = System.DateTime.Now;
            SonGuncelleme = System.DateTime.Now;
        }
    }
}
