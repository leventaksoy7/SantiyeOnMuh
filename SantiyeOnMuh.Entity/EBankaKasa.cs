﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SantiyeOnMuh.Entity
{
    public class EBankaKasa
    {
        public int Id { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime Tarih { get; set; }
        public string Aciklama { get; set; }
        public string Nitelik { get; set; }
        public decimal Giren { get; set; }
        public decimal Cikan { get; set; }
        public bool Durum { get; set; }
        public int? CekKaynak { get; set; }
        public int? NakitKaynak { get; set; }
        public int? SantiyeKasaKaynak { get; set; }
        public int BankaHesapId { get; set; }
        public EBankaHesap BankaHesap { get; set; }
        public DateTime SistemeGiris { get; set; }
        public DateTime SonGuncelleme { get; set; }
        public EBankaKasa()
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
