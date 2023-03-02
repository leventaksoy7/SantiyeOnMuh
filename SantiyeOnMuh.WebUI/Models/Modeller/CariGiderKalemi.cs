using SantiyeOnMuh.WebUI.Models.Modeller;
using System.ComponentModel.DataAnnotations;

namespace SantiyeOnMuh.WebUI.Models.Modeller
{
    public class CariGiderKalemi
    {
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        public string Ad { get; set; }
        public bool Durum { get; set; }
        public bool Tur { get; set; }
        public List<CariKasa> CariKasas { get; set; }
        public CariGiderKalemi()
        {
            Durum = true;
            Tur = true;
        }
    }
}
