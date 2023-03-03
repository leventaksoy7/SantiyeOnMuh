using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;

namespace SantiyeOnMuh.Entity
{
    public class ESirket
    {
        public int Id { get; set; }
        public string Ad { get; set; }
        public string? VergiNo { get; set; }
        public bool Durum { get; set; }
        public List<ECek> Ceks { get; set; }
        public List<ENakit> Nakits { get; set; }
        public ESirket()
        {
            Durum = true;
        }
    }
}
