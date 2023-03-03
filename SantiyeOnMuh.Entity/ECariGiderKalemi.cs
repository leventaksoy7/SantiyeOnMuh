using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SantiyeOnMuh.Entity
{
    public class ECariGiderKalemi
    {
        public int Id { get; set; }

        public string Ad { get; set; }

        public bool Durum { get; set; }

        public bool Tur { get; set; }

        public List<ECariKasa> CariKasas { get; set; }

        public ECariGiderKalemi()
        {
            Durum = true;
            Tur = true;
        }
    }
}
