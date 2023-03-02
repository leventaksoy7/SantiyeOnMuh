using SantiyeOnMuh.DataAccess.Abstract;
using SantiyeOnMuh.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SantiyeOnMuh.DataAccess.Concrete.EfCore
{
    public class EfCoreSirketRepository : EfCoreGenericRepository<ESirket, Context>, ISirketRepository
    {
        public List<ESirket> GetAll(bool drm)
        {
            using (var context = new Context())
            {
                return context.Sirkets
                    .Where(i => i.Durum == drm)
                    .ToList();
            }
        }
    }
}
