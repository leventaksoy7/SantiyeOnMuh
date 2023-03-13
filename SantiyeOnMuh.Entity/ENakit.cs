using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SantiyeOnMuh.Entity
{
    public class ENakit
    {
        public int Id { get; set; }
        public DateTime Tarih { get; set; }
        public string Aciklama { get; set; }
        public decimal Tutar { get; set; }
        public string? ImgUrl { get; set; }
        public int? BankaKasaKaynak { get; set; }
        public int? CariKasaKaynak { get; set; }
        public DateTime SistemeGiris { get; set; }
        public DateTime SonGuncelleme { get; set; }
        public bool Durum { get; set; }
        public int CariHesapId { get; set; }
        public ECariHesap CariHesap { get; set; }
        public int SirketId { get; set; }
        public ESirket Sirket { get; set; }
        public int BankaHesapId { get; set; }
        public EBankaHesap BankaHesap { get; set; }
        public ENakit()
        {
            Tarih = System.DateTime.Now;

            BankaKasaKaynak = null;
            CariKasaKaynak = null;

            Durum = true;

            SistemeGiris = System.DateTime.Now;
            SonGuncelleme = System.DateTime.Now;
        }
    }
}
