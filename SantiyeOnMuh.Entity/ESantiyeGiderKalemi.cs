using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SantiyeOnMuh.Entity
{
    public class ESantiyeGiderKalemi
    {
        public int Id { get; set; }

        [StringLength(30)]
        public string Ad { get; set; }

        public bool Durum { get; set; }

        public bool Tur { get; set; }

        public List<ESantiyeKasa> SantiyeKasas { get; set; }
        public ESantiyeGiderKalemi()
        {
            Durum = true;
            Tur = true;
        }
    }
}
