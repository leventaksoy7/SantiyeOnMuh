using SantiyeOnMuh.WebUI.Models.Modeller;
using System.ComponentModel.DataAnnotations;

namespace SantiyeOnMuh.WebUI.Models.Modeller
{
    public class CariKasa
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "TARİH GİRMELİSİNİZ")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime Tarih { get; set; }
        public string Aciklama { get; set; }
        public decimal Miktar { get; set; }
        public decimal BirimFiyat { get; set; }
        public decimal Borc { get; set; }
        public decimal Alacak { get; set; }
        public string ImgUrl { get; set; }
        public int? CekKaynak { get; set; }
        public int? NakitKaynak { get; set; }
        public DateTime SistemeGiris { get; set; }
        public DateTime SonGuncelleme { get; set; }
        public bool Durum { get; set; }
        public int CariGiderKalemiId { get; set; }
        public CariGiderKalemi CariGiderKalemi { get; set; }
        public int CariHesapId { get; set; }
        public CariHesap CariHesap { get; set; }
        public CariKasa()
        {
            Tarih = System.DateTime.Now;
            Miktar = 1;
            BirimFiyat = 1;
            Borc = 0;
            Alacak = 0;
            CekKaynak = null;
            NakitKaynak = null;
            Durum = true;
            SistemeGiris = System.DateTime.Now;
            SonGuncelleme = System.DateTime.Now;
        }
    }
}
