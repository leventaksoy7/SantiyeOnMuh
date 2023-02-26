using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SantiyeOnMuh.Entity
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
