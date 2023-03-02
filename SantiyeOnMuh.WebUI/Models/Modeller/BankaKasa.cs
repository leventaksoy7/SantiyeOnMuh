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
        public string Aciklama { get; set; }
        //PARA AKIŞININ NİCELİĞİ ÖRN: EFT HAVALE ÖDEME VB
        public string Nitelik { get; set; }
        public decimal Giren { get; set; }
        public decimal Cikan { get; set; }
        public bool Durum { get; set; }
        public int? CekKaynak { get; set; }
        public int? NakitKaynak { get; set; }
        public int? SantiyeKasaKaynak { get; set; }
        public int BankaHesapId { get; set; }
        public BankaHesap BankaHesap { get; set; }
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
