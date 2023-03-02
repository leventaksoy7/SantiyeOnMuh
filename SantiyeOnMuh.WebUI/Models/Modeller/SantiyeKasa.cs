﻿using SantiyeOnMuh.Entity;
using SantiyeOnMuh.WebUI.Models.Modeller;
using System.ComponentModel.DataAnnotations;

namespace SantiyeOnMuh.WebUI.Models.Modeller
{
    public class SantiyeKasa
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "TARİH GİRMELİSİNİZ")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime Tarih { get; set; }
        public string Aciklama { get; set; }
        public string Kisi { get; set; }
        public string No { get; set; }//PLAKA-CEP-FATURA VEYA FİŞ NO
        public decimal Gelir { get; set; }
        public decimal Gider { get; set; }
        public string ImgUrl { get; set; }
        public bool Durum { get; set; }
        public int? BankaKasaKaynak { get; set; }
        public DateTime SistemeGiris { get; set; }
        public DateTime SonGuncelleme { get; set; }
        public int SantiyeGiderKalemiId { get; set; }
        public ESantiyeGiderKalemi SantiyeGiderKalemi { get; set; }
        public int SantiyeId { get; set; }
        public ESantiye Santiye { get; set; }
        public SantiyeKasa()
        {
            Tarih = DateTime.Today;

            Gelir = 0;
            Gider = 0;

            Durum = true;

            //BankaKasaKaynak = null;

            SistemeGiris = DateTime.Today;
            SonGuncelleme = DateTime.Today;
        }
    }
}
