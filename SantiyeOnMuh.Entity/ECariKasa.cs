using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SantiyeOnMuh.Entity
{
    public class ECariKasa
    {
        public int Id { get; set; }
        public DateTime Tarih { get; set; }
        public string Aciklama { get; set; }
        public decimal Miktar { get; set; }
        public decimal BirimFiyat { get; set; }
        public decimal Borc { get; set; }
        public decimal Alacak { get; set; }
        public string? ImgUrl { get; set; }
        public int? CekKaynak { get; set; }
        public int? NakitKaynak { get; set; }
        public DateTime SistemeGiris { get; set; }
        public DateTime SonGuncelleme { get; set; }
        public bool Durum { get; set; }
        public int CariGiderKalemiId { get; set; }
        public ECariGiderKalemi CariGiderKalemi { get; set; }
        public int CariHesapId { get; set; }
        public ECariHesap CariHesap { get; set; }
        public ECariKasa()
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