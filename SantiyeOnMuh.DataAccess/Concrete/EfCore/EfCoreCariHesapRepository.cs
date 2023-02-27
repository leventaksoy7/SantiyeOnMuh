using SantiyeOnMuh.DataAccess.Abstract;
using SantiyeOnMuh.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SantiyeOnMuh.DataAccess.Concrete.EfCore
{
    public class EfCoreCariHesapRepository : EfCoreGenericRepository<CariHesap, Context>, ICariHesapRepository
    {
        public List<CariHesap> GetAll(bool durum)
        {
            using (var context = new Context())
            {
                return context.CariHesaps
                    .Where(i => i.Durum == durum)
                    .ToList();
            }
        }
    }
}
