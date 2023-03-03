using DocumentFormat.OpenXml.Wordprocessing;
using SantiyeOnMuh.Entity;
using SantiyeOnMuh.WebUI.Models.Modeller;
using System.ComponentModel.DataAnnotations;

namespace SantiyeOnMuh.WebUI.Models.Modeller
{
    public class CariHesap
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "CARİ HESAP ADI BOŞ BIRAKILAMAZ!")]
        [Display(Name = "CARİ HESAP ADI GİRİLMESİ ZORUNLUDUR")]
        [MinLength(5)]
        public string Ad { get; set; }

        [Display(Name = "ADRES")]
        public string? Adres { get; set; }

        [Required(ErrorMessage = "SADECE RAKAM GİRİNİZ, BOŞLUK BIRAKMAYINIZ.")]
        [Display(Name = "TELEFON NUMARASI GİRİLMESİ ZORUNLUDUR, BOŞLUK BIRAKMAYINIZ.")]
        [MinLength(10)]
        public string Telefon { get; set; }

        [Display(Name = "VERGİ NUMARASI")]
        public string? VergiNo { get; set; }

        [Required(ErrorMessage = "YETKİLİ KİŞİNİN ADININ YAZILMASI ZORUNLUDUR.")]
        [Display(Name = "YETKİLİ KİŞİNİN ADININ YAZILMASI ZORUNLUDUR.")]
        public string IlgiliKisi { get; set; }

        [Display(Name = "YETKİLİ KİŞİ TELEFON NUMARASI")]
        public string? IlgiliKisiTelefon { get; set; }

        [Display(Name = "ÖDEME")]
        public string? Odeme { get; set; }

        [Display(Name = "VADE")]
        public string? Vade { get; set; }

        public bool Durum { get; set; }

        public List<ECek> Ceks { get; set; }

        public List<ENakit> Nakits { get; set; }

        public List<ECariKasa> CariKasas { get; set; }

        public int SantiyeId { get; set; }

        public ESantiye Santiye { get; set; }

        public CariHesap()
        {
            Durum = true;
        }
    }
}
