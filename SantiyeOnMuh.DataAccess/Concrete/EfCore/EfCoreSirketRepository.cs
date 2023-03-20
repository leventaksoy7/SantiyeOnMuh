using SantiyeOnMuh.DataAccess.Abstract;
using SantiyeOnMuh.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SantiyeOnMuh.DataAccess.Concrete.EfCore
{
    public class EfCoreSirketRepository : EfCoreGenericRepository<ESirket>, ISirketRepository
    {
        public EfCoreSirketRepository(Context context):base(context)
        {
            
        }
        private Context Context
        {
            get { return context as Context; }
        }
        public List<ESirket> GetAll(bool drm)
        {

            return Context.Sirkets
                .Where(i => i.Durum == drm)
                .ToList();

        }
    }
}
