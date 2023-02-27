using SantiyeOnMuh.DataAccess.Abstract;
using SantiyeOnMuh.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SantiyeOnMuh.DataAccess.Concrete.EfCore
{
    public class EfCoreSirketRepository : EfCoreGenericRepository<Sirket, Context>, ISirketRepository
    {
        public List<Sirket> GetAll(bool durum)
        {
            using (var context = new Context())
            {
                return context.Sirkets
                    .Where(i => i.Durum == durum)
                    .ToList();
            }
        }
    }
}
