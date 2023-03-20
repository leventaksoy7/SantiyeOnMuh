using SantiyeOnMuh.DataAccess.Abstract;
using SantiyeOnMuh.DataAccess.Concrete.EfCore;
using SantiyeOnMuh.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SantiyeOnMuh.DataAccess.Concrete.SqlServer
{
    public class EfCoreSantiyeRepository : EfCoreGenericRepository<ESantiye>, ISantiyeRepository
    {
        public EfCoreSantiyeRepository(Context context):base(context)
        {
            
        }
        private Context Context
        {
            get { return context as Context; }
        }
        public List<ESantiye> GetAll(bool drm)
        {

            return Context.Santiyes
                .Where(i => i.Durum == drm)
                .ToList();

        }
    }
}
