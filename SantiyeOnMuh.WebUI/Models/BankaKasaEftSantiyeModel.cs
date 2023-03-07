using System.ComponentModel.DataAnnotations;

namespace SantiyeOnMuh.WebUI.Models
{
    public class BankaKasaEftSantiyeModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "TARİH GİRMELİSİNİZ")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime Tarih { get; set; }
        public string Aciklama { get; set; }
        public string Tutar { get; set; }
        public int BankaHesapId { get; set; }
        public int SantiyeId { get; set; }
        public int? BankaKasaId { get; set; }
        public int? SantiyeKasaId { get; set; }
        public BankaKasaEftSantiyeModel()
        {
            BankaKasaId = null;
            SantiyeKasaId = null;
        }
    }
}
