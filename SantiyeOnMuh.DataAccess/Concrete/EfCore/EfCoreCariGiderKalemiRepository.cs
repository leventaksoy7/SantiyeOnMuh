using SantiyeOnMuh.DataAccess.Abstract;
using SantiyeOnMuh.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SantiyeOnMuh.DataAccess.Concrete.EfCore
{
    public class EfCoreCariGiderKalemiRepository : EfCoreGenericRepository<ECariGiderKalemi>, ICariGiderKalemiRepository
    {
        public EfCoreCariGiderKalemiRepository(Context context) : base(context)
        {
            
        }
        private Context Context
        {
            get { return context as Context; }
        }
        public List<ECariGiderKalemi> GetAll(bool drm)
        {

            return Context.CariGiderKalemis
                .Where(i => i.Durum == drm)
                .ToList();

        }

        public List<ECariGiderKalemi> GetAll(bool drm, bool tur)
        {

            return Context.CariGiderKalemis
                .Where(i => i.Tur == drm)
                .Where(i => i.Durum == drm)
                .OrderBy(i => i.Ad)
                .ToList();

        }
    }
}
