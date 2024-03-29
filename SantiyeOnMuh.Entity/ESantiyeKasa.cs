﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SantiyeOnMuh.Entity
{
    public class ESantiyeKasa
    {
        public int Id { get; set; }
        public DateTime Tarih { get; set; }
        public string Aciklama { get; set; }
        public string? Kisi { get; set; }
        public string? No { get; set; }//PLAKA-CEP-FATURA VEYA FİŞ NO
        public decimal Gelir { get; set; }
        public decimal Gider { get; set; }
        public string? ImgUrl { get; set; }
        public bool Durum { get; set; }
        public int? BankaKasaKaynak { get; set; }
        public DateTime SistemeGiris { get; set; }
        public DateTime SonGuncelleme { get; set; }
        public int SantiyeGiderKalemiId { get; set; }
        public ESantiyeGiderKalemi SantiyeGiderKalemi { get; set; }
        public int SantiyeId { get; set; }
        public ESantiye Santiye { get; set; }
        public ESantiyeKasa()
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
