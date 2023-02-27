using SantiyeOnMuh.DataAccess.Abstract;
using SantiyeOnMuh.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SantiyeOnMuh.DataAccess.Concrete.EfCore
{
    public class EfCoreCariGiderKalemiRepository : EfCoreGenericRepository<CariGiderKalemi, Context>, ICariGiderKalemiRepository
    {
        public List<CariGiderKalemi> GetAll(bool durum)
        {
            using (var context = new Context())
            {
                return context.CariGiderKalemis
                    .Where(i => i.Durum == durum)
                    .ToList();
            }
        }
    }
}
