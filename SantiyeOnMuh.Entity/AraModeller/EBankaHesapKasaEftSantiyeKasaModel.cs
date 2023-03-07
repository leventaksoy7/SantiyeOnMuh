using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SantiyeOnMuh.Entity.AraModeller
{
    public class EBankaHesapKasaEftSantiyeKasaModel
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
        public EBankaHesapKasaEftSantiyeKasaModel()
        {
            BankaKasaId = null;
            SantiyeKasaId = null;
        }
    }
}
