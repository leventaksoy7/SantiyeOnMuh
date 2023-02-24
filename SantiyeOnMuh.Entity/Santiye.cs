using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SantiyeOnMuh.Entity
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
        //public List<CariHesap> CariHesaps { get; set; }
        public Santiye()
        {
            Durum = true;
            Ad = string.Empty;
        }
    }
}
