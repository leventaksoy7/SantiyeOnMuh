using SantiyeOnMuh.WebUI.Models.Modeller;
using System.ComponentModel.DataAnnotations;

namespace SantiyeOnMuh.WebUI.Models.Modeller
{
    public class SantiyeGiderKalemi
    {
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        public string Ad { get; set; }
        public bool Durum { get; set; }
        public bool Tur { get; set; }
        public List<SantiyeKasa> SantiyeKasas { get; set; }
        public SantiyeGiderKalemi()
        {
            Durum = true;
            Tur = true;
        }
    }
}
