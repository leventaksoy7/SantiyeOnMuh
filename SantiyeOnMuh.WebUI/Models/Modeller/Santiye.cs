using SantiyeOnMuh.WebUI.Models.Modeller;
using System.ComponentModel.DataAnnotations;

namespace SantiyeOnMuh.WebUI.Models.Modeller
{
    public class Santiye
    {
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        public string Ad { get; set; }
        public string Adres { get; set; }
        public bool Durum { get; set; }
        public List<SantiyeKasa> SantiyeKasas { get; set; }
        public List<CariHesap> CariHesaps { get; set; }
        public Santiye()
        {
            Durum = true;
            Ad = string.Empty;
        }
    }
}
