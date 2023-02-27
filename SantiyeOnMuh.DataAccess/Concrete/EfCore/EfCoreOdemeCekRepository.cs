using SantiyeOnMuh.DataAccess.Abstract;
using SantiyeOnMuh.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SantiyeOnMuh.DataAccess.Concrete.EfCore
{
    public class EfCoreOdemeCekRepository : EfCoreGenericRepository<OdemeCek, Context>, IOdemeCekRepository
    {
        public List<OdemeCek> GetAll(bool durum)
        {
            using (var context = new Context())
            {
                return context.OdemeCeks
                    .Where(i => i.Durum == durum)
                    .ToList();
            }
        }
    }
}
