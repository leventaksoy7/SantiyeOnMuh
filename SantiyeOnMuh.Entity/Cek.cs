using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SantiyeOnMuh.Entity
{
    public class Cek
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "TARİH GİRMELİSİNİZ")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime Tarih { get; set; }
        public string Aciklama { get; set; }
        public string CekNo { get; set; }
        public decimal Tutar { get; set; }
        public string ImgUrl { get; set; }
        public int? BankaKasaKaynak { get; set; }
        public int? CariKasaKaynak { get; set; }
        public DateTime SistemeGiris { get; set; }
        public DateTime SonGuncelleme { get; set; }
        public bool Durum { get; set; }
        public bool OdemeDurumu { get; set; }
        public int CariHesapId { get; set; }
        public CariHesap CariHesap { get; set; }
        public int SirketId { get; set; }
        public Sirket Sirket { get; set; }
        public int BankaHesapId { get; set; }
        public BankaHesap BankaHesap { get; set; }
        public Cek()
        {
            Tarih = System.DateTime.Now;

            Durum = true;
            OdemeDurumu = false;

            Tutar = 0;

            BankaKasaKaynak = null;
            CariKasaKaynak = null;

            SistemeGiris = System.DateTime.Now;
            SonGuncelleme = System.DateTime.Now;
        }
    }
}
