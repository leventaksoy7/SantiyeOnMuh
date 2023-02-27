using System.ComponentModel.DataAnnotations;

namespace SantiyeOnMuh.WebUI.Models.Ara_Entityler
{
    public class BankaKasaEftModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "TARİH GİRMELİSİNİZ")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime Tarih { get; set; }
        public string Aciklama { get; set; }
        public string Nitelik { get; set; }
        public decimal Tutar { get; set; }
        public int GonderenBanka { get; set; }
        public int AliciBanka { get; set; }
        public DateTime SistemeGiris { get; set; }
        public DateTime SonGuncelleme { get; set; }
        public bool Durum { get; set; }
        public BankaKasaEftModel()
        {
            Tarih = System.DateTime.Now;
            Nitelik = "EFT";
            Tutar = 0;
            Durum = true;

            SistemeGiris = System.DateTime.Now;
            SonGuncelleme = System.DateTime.Now;
        }
    }
}
