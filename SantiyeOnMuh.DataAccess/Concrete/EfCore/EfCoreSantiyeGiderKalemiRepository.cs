using SantiyeOnMuh.DataAccess.Abstract;
using SantiyeOnMuh.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SantiyeOnMuh.DataAccess.Concrete.EfCore
{
    public class EfCoreSantiyeGiderKalemiRepository : EfCoreGenericRepository<SantiyeGiderKalemi, Context>, ISantiyeGiderKalemiRepository
    {
        public List<SantiyeGiderKalemi> GetAll(bool durum)
        {
            using (var context = new Context())
            {
                return context.SantiyeGiderKalemis
                    .Where(i => i.Durum == durum)
                    .ToList();
            }
        }
    }
}
