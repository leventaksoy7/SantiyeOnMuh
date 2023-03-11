using SantiyeOnMuh.Entity;
using SantiyeOnMuh.WebUI.Models.Modeller;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SantiyeOnMuh.WebUI.Models.Modeller
{
    public class BankaKasa
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "TARİH GİRMELİSİNİZ")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        [Display(Name = "TARİH")]
        public DateTime Tarih { get; set; }

        [Required(ErrorMessage = "AÇIKLAMA BOŞ BIRAKILAMAZ!")]
        [Display(Name = "AÇIKLAMA", Prompt = "AÇIKLAMA BOŞ BIRAKILAMAZ!")]
        [MinLength(3)]
        public string Aciklama { get; set; }

        //PARA AKIŞININ NİCELİĞİ ÖRN: EFT HAVALE ÖDEME VB
        [Required(ErrorMessage = "NİTELİK BOŞ BIRAKILAMAZ!")]
        [Display(Name = "NİTELİK", Prompt = "HAKEDİŞ-EFT-HAVALE-FATURA ÖDEMESİ VB. OLARAK BELİRTİNİZ")]
        [MinLength(3)]
        public string Nitelik { get; set; }

        [Required(ErrorMessage = "TUTAR BOŞ BIRAKILAMAZ!")]
        [Display(Name = "GELİR",Prompt ="GELİR TUTARI BOŞ BIRAKILAMAZ!")]
        
        public string Giren { get; set; }

        [Required(ErrorMessage = "TUTAR BOŞ BIRAKILAMAZ!")]
        [Display(Name = "GİDER", Prompt = "GİDER TUTARI BOŞ BIRAKILAMAZ!")]
        
        public string Cikan { get; set; }

        public bool Durum { get; set; }

        public int? CekKaynak { get; set; }

        public int? NakitKaynak { get; set; }

        public int? SantiyeKasaKaynak { get; set; }

        public EBankaHesap? BankaHesap { get; set; }

        [Display(Name = "BANKA HESABI")]
        public int BankaHesapId { get; set; }

        public DateTime SistemeGiris { get; set; }

        public DateTime SonGuncelleme { get; set; }

        public BankaKasa()
        {
            Durum = true;

            CekKaynak = null;
            NakitKaynak = null;
            SantiyeKasaKaynak = null;

            Giren = "0";
            Cikan = "0";

            Tarih = System.DateTime.Now;
            SistemeGiris = System.DateTime.Now;
            SonGuncelleme = System.DateTime.Now;
        }
    }
}
