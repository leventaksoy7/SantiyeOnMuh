using SantiyeOnMuh.DataAccess.Abstract;
using SantiyeOnMuh.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SantiyeOnMuh.DataAccess.Concrete.EfCore
{
    public class EfCoreSantiyeKasaRepository : EfCoreGenericRepository<SantiyeKasa, Context>, ISantiyeKasaRepository
    {
        public List<SantiyeKasa> GetAll(bool durum)
        {
            using (var context = new Context())
            {
                return context.SantiyesKasas
                    .Where(i => i.Durum == durum)
                    .ToList();
            }
        }
    }
}
