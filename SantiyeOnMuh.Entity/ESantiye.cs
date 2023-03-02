using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SantiyeOnMuh.Entity
{
    public class ESantiye
    {
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        public string Ad { get; set; }
        public string Adres { get; set; }
        public bool Durum { get; set; }
        public List<ESantiyeKasa> SantiyeKasas { get; set; }
        public List<ECariHesap> CariHesaps { get; set; }
        public ESantiye()
        {
            Durum = true;
            Ad = string.Empty;
        }
    }
}
